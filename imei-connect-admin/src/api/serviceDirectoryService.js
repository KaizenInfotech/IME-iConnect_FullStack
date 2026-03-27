import api from './axiosInstance';

export const getEntries = (groupId, memberProfileId) =>
  api.post('/ServiceDirectory/GetServiceCategoriesData', { groupId, memberProfileId });

export const getCategories = (groupId) =>
  api.post('/ServiceDirectory/GetServiceDirectoryCategories', { groupId });

export const createEntry = (data) =>
  api.post('/ServiceDirectory/AddServiceDirectory', data);

export const updateEntry = (data) =>
  api.post('/ServiceDirectory/AddServiceDirectory', data);

export const deleteEntry = (serviceDirId) =>
  api.post('/ServiceDirectory/DeleteServiceDirectory', { serviceDirId });