﻿namespace Quality_of_Life_changer.Contracts.Queries;

using MediatR;
using Model.CalendarAdapterModel;

public record GetTodayEventsQuery : IRequest<GetTodayEventsResponse>;

public record GetTodayEventsResponse(IReadOnlyCollection<CalendarEvent> Events);