import api from './axiosInstance';

export const getAnnouncements = (groupId, memberProfileId, searchText = '', moduleId = '3') =>
  api.post('/Announcement/GetAnnouncementList', { groupId, memberProfileId, searchText, moduleId });

export const getAnnouncementDetails = (announID) =>
  api.post('/Announcement/GetAnnouncementDetails', { announID });

export const createAnnouncement = (data) =>
  api.post('/Announcement/AddAnnouncement', data);

export const updateAnnouncement = (id, data) =>
  api.post('/Announcement/AddAnnouncement', { ...data, announID: id });

export const deleteAnnouncement = (announID) =>
  api.post('/Announcement/DeleteAnnouncement', { announID });