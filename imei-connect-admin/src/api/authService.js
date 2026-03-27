import api from './axiosInstance';

export const webLogin = (mobileNo, password, countryCode = '1') =>
  api.post('/Login/WebLogin', { mobileNo, password, countryCode });

export const login = (mobileNo, countryCode = '1', loginType = '0', deviceToken = '') =>
  api.post('/Login/UserLogin', { mobileNo, countryCode, loginType, deviceToken });

export const verifyOtp = (data) =>
  api.post('/Login/PostOTP', data);

export const getWelcomeScreen = (masterUID) =>
  api.post('/Login/GetWelcomeScreen', { masterUID });