import api from './axiosInstance';

export const getDocuments = (grpID, memberProfileID, flag = '0') =>
  api.post('/DocumentSafe/GetDocumentList', { grpID, memberProfileID, flag });

export const createDocument = (formData) =>
  api.post('/DocumentSafe/AddDocument', formData, {
    headers: { 'Content-Type': 'multipart/form-data' },
  });

export const deleteDocument = (DocID) =>
  api.post('/DocumentSafe/DeleteDocument', { DocID });

export const updateDocumentIsRead = (DocID, memberProfileID) =>
  api.post('/DocumentSafe/UpdateDocumentIsRead', { DocID, memberProfileID });