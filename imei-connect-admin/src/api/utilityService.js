import api from './axiosInstance';

export const getCountries = () =>
  api.post('/Group/GetAllCountriesAndCategories', {});

export const getDistricts = () =>
  api.post('/Group/GetAllCountriesAndCategories', {});

export const getYears = (groupId) =>
  api.post('/Gallery/Fillyearlist', { grpID: groupId });

export const getNotifications = (groupId, memberProfileId) =>
  api.post('/Group/GetNotificationCount', { groupId, memberProfileId });