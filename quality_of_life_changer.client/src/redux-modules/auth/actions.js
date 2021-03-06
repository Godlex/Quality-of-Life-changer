import { authConstants } from "./constants";

export function login(email, password) {
  return { type: authConstants.LOGIN_ATTEMPT, email, password };
}

export function register(username, email, password, confirmPassword) {
  return {
    type: authConstants.REGISTER_ATTEMPT,
    username,
    email,
    password,
    confirmPassword,
  };
}

export function logout() {
  return { type: authConstants.LOGOUT_SUCCESS };
}

export function setUser(id, name, email, isAuth) {
  return {
    type: authConstants.SET_USER,
    id,
    name,
    email,
    isAuth,
  };
}
