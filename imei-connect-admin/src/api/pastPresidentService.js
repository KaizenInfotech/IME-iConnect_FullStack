import api from './axiosInstance';

export const getPastPresidents = (GroupId, SearchText = '', updateOn = '1970/01/01 00:00:00') =>
  api.post('/PastPresidents/getPastPresidentsList', { GroupId, SearchText, updateOn });

export const createPastPresident = (data) =>
  api.post('/PastPresidents/AddPastPresident', data);

export const updatePastPresident = (data) =>
  api.post('/PastPresidents/UpdatePastPresident', data);

export const deletePastPresident = (PastPresidentId) =>
  api.post('/PastPresidents/DeletePastPresident', { PastPresidentId });