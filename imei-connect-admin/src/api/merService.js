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

// Uploads a MER / iMelange PDF and returns the saved file's public URL,
// which must be stored as FilePath (a bare filename does not resolve).
export const uploadMerFile = (file, title, financeYear, transType) => {
  const formData = new FormData();
  formData.append('file', file);
  if (title) formData.append('title', title);
  if (financeYear) formData.append('financeYear', financeYear);
  if (transType) formData.append('transType', transType);
  return api.post('/Gallery/UploadMER', formData, {
    headers: { 'Content-Type': 'multipart/form-data' },
  });
};

export const createMerItem = (data) =>
  api.post('/Gallery/AddMER', data);

export const updateMerItem = (id, data) =>
  api.post('/Gallery/UpdateMER', { ...data, MER_ID: String(id) });

export const deleteMerItem = (MER_ID) =>
  api.post('/Gallery/DeleteMER', { MER_ID: String(MER_ID) });