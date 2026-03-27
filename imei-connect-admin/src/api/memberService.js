import api from './axiosInstance';

export const getMembers = (grpID, updatedOn = '1970-01-01 00:00:00') =>
  api.post('/Member/GetMemberListSync', { grpID, updatedOn });

export const getMember = (memberProfileId, groupId) =>
  api.get(`/Member/GetMemberDetails?MemProfileId=${memberProfileId}&GrpID=${groupId}`);

export const getMemberDetail = (memberProfID, grpID) =>
  api.post('/Member/GetDirectoryList', { grpID, memberProfID });

export const updateProfile = (data) =>
  api.post('/Member/UpdateProfile', data);

export const updateAddress = (data) =>
  api.post('/Member/UpdateAddressDetails', data);

export const updateFamily = (data) =>
  api.post('/Member/UpdateFamilyDetails', data);

export const uploadProfilePhoto = (formData) =>
  api.post('/Member/UploadProfilePhoto', formData, {
    headers: { 'Content-Type': 'multipart/form-data' },
  });

export const getBodList = (grpId, searchText = '', YearFilter = '') =>
  api.post('/Member/GetBODList', { grpId, searchText, YearFilter });

export const getGoverningCouncil = (searchText = '', YearFilter = '') =>
  api.post('/Member/GetGoverningCouncl', { searchText, YearFilter });

export const updateMember = (data) =>
  api.post('/Member/UpdateMemebr', data);

export const getDirectoryList = (grpID, page = '1', searchText = '') =>
  api.post('/Member/GetDirectoryList', { grpID, page, searchText });

export const createMember = (data) =>
  api.post('/Login/Registration', data);

export const deleteMember = (id) =>
  api.post('/Member/DeleteMember', { memberProfileId: id });