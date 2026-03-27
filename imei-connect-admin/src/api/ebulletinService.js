import api from './axiosInstance';

export const getEbulletins = (groupId, flag = '0') =>
  api.post('/Ebulletin/GetYearWiseEbulletinList', { groupId, flag });

export const createEbulletin = (data) =>
  api.post('/Ebulletin/AddEbulletin', data);

export const updateEbulletin = (data) =>
  api.post('/Ebulletin/AddEbulletin', data);

export const deleteEbulletin = (ebulletinID) =>
  api.post('/Ebulletin/DeleteEbulletin', { ebulletinID });