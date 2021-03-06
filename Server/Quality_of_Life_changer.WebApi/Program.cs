using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using Quality_of_Life_changer.Adapter;
using Quality_of_Life_changer.Adapter.Services;
using Quality_of_Life_changer.Contracts.Commands;
using Quality_of_Life_changer.Contracts.Interfaces;
using Quality_of_Life_changer.Contracts.Queries;
using Quality_of_Life_changer.Data;
using Quality_of_Life_changer.Implementation.Handlers.QueryHandlers;
using Quality_of_Life_changer.WebApi;
using Quality_of_Life_changer.WebApi.Validators;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Json.Serialization;

Logger.Initial();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog(Logger.Configure);

    builder.Services.AddDbContext<QolcDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

    const string allowSpecificOrigins = "_allowSpecificOrigins";

    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("JWTSecretKey"))
                )
            };
        }).AddCookie().AddGoogle(options =>
        {
            var fileName = Directory.GetFiles(@".", "client_secret*").First();
            var jsonString = File.ReadAllText(fileName);
            var installedApplicationCredits = JObject.Parse(jsonString).GetValue("installed");

            options.ClientId = installedApplicationCredits?["client_id"]?.ToString() ?? string.Empty;
            options.ClientSecret = installedApplicationCredits?["client_secret"]?.ToString() ?? string.Empty;
        });

    builder.WebHost.UseUrls("http://localhost:5145");

    builder.Services.AddScoped<ICalendarAdapter, CalendarAdapter>();

    builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

    builder.Services.AddSingleton(new JwtSecurityTokenHandler());

    builder.Services.AddMediatR(typeof(GetUserByEmailQuery).Assembly, typeof(AddUserCommand).Assembly,
        typeof(GetAllUsersQueryHandler).Assembly);

    builder.Services.AddSingleton<IAuthService>(
        new AuthService(builder.Configuration.GetValue<string>("JWTSecretKey"),
            builder.Configuration.GetValue<int>("JWTLifespan"))
    );

    builder.AddCors(allowSpecificOrigins);

    builder.Services.AddControllers()
        .AddFluentValidation(s =>
        {
            s.RegisterValidatorsFromAssemblyContaining<RegisterModelValidator>();
            s.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
        })
        .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true)
        .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    app.UseSerilogRequestLogging();

    // Configure the HTTP request pipeline.

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseCors(allowSpecificOrigins);

    app.ConfigureExceptionMiddleware();

    app.UseHttpsRedirection();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    var type = ex.GetType().Name;
    if (type.Equals("StopTheHostException", StringComparison.Ordinal))
    {
        Log.Information(ex, "-program cs");
        throw;
    }

    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}