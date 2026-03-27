import api from './axiosInstance';

export const getBanners = (groupId) =>
  api.post('/Group/GetNewDashboard', { groupId });

export const createBanner = (data) =>
  api.post('/Group/AddBanner', data);

export const updateBanner = (data) =>
  api.post('/Group/UpdateBanner', data);

export const deleteBanner = (bannerId) =>
  api.post('/Group/DeleteBanner', { bannerId });