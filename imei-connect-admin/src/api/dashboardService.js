import api from './axiosInstance';

export const getGroupModules = (groupId) =>
  api.post('/Group/GetGroupModulesList', { groupId });

export const getDashboard = (groupId) =>
  api.post('/Group/GetNewDashboard', { groupId });

export const getNotificationCount = (groupId, memberProfileId) =>
  api.post('/Group/GetNotificationCount', { groupId, memberProfileId });