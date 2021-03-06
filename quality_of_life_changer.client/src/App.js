import React, { Component } from "react";
import LoginForm from "./components/auth/login-form";
import RegisterForm from "./components/auth/register-form";
import { Route, Routes } from "react-router-dom";
import HomePage from "./components/home/home-page";
import Navbar from "./components/navbar/navbar";
import TodayEventsPage from "./components/events/today-events-page";
import ProfilePage from "./components/profile/profile-page";

class App extends Component {
  render() {
    return (
      <div className="App">
        <Navbar />
        <Routes>
          <Route path="/" element={<HomePage />} />
          <Route path="/login" element={<LoginForm />} />
          <Route path="/register" element={<RegisterForm />} />
          <Route path="/events/today" element={<TodayEventsPage />} />
          <Route path="/user-profile" element={<ProfilePage />} />
        </Routes>
      </div>
    );
  }
}

export default App;
