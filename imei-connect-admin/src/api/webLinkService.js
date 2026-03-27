import api from './axiosInstance';

export const getWebLinks = (GroupId) =>
  api.post('/WebLink/GetWebLinksList', { GroupId });

export const createWebLink = (data) =>
  api.post('/WebLink/AddWebLink', data);

export const updateWebLink = (data) =>
  api.post('/WebLink/UpdateWebLink', data);

export const deleteWebLink = (weblinkId) =>
  api.post('/WebLink/DeleteWebLink', { weblinkId });