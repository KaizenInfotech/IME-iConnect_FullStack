import api from './axiosInstance';

export const getSubGroups = (groupId) =>
  api.post('/Group/GetSubGroupList', { groupId });

export const getSubGroupDetail = (subGroupId) =>
  api.post('/Group/GetSubGroupDetail', { subGroupId });

export const createSubGroup = (data) =>
  api.post('/Group/CreateSubGroup', data);

export const deleteSubGroup = (subGroupId) =>
  api.post('/Group/DeleteSubGroup', { subGroupId });