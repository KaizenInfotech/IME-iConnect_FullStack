import api from './axiosInstance';

export const getAlbumYears = (grpID) =>
  api.post('/Gallery/Fillyearlist', { grpID });

export const getAlbums = (groupId, year = '', moduleId = '8', searchText = '') =>
  api.post('/Gallery/GetAlbumsList_New', {
    groupId, district_id: '2', category_id: '1',
    year, clubid: '', SharType: '0', moduleId, searchText,
  }, { headers: { 'Content-Type': 'application/json' } });

export const getAlbum = (albumId) =>
  api.post('/Gallery/GetAlbumDetails', { albumId });

export const getAlbumPhotos = (albumId, groupId, Financeyear = '') =>
  api.post('/Gallery/GetAlbumPhotoList_New', { albumId, groupId, Financeyear },
    { headers: { 'Content-Type': 'application/json' } });

export const createAlbum = (data) =>
  api.post('/Gallery/AddUpdateAlbum', data);

export const addPhoto = (formData) =>
  api.post('/Gallery/AddUpdateAlbumPhoto', formData, {
    headers: { 'Content-Type': 'multipart/form-data' },
  });

export const deletePhoto = (photoId) =>
  api.post('/Gallery/DeleteAlbumPhoto', { photoId });

export const deleteAlbum = (albumId) =>
  api.post('/Gallery/DeleteAlbum', { albumId });