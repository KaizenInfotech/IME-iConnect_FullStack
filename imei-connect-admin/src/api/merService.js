import api from './axiosInstance';

// MER(I) - TransType "1", iMelange - TransType "2"
export const getMerYears = (Type = '1') =>
  api.post('/Gallery/GetYear', { Type });

export const getMerItems = (FinanceYear, TransType = '1') =>
  api.post('/Gallery/GetMER_List', { FinanceYear, TransType });

export const getMelangeYears = () =>
  api.post('/Gallery/GetYear', { Type: '2' });

export const getMelangeItems = (FinanceYear) =>
  api.post('/Gallery/GetMER_List', { FinanceYear, TransType: '2' });

export const createMerItem = (data) =>
  api.post('/Gallery/AddMER', data);

export const updateMerItem = (data) =>
  api.post('/Gallery/UpdateMER', data);

export const deleteMerItem = (MER_ID) =>
  api.post('/Gallery/DeleteMER', { MER_ID });