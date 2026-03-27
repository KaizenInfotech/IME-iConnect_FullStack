import api from './axiosInstance';

export const getGroups = (masterUID) =>
  api.post('/Group/GetAllGroupListSync', {
    masterUID,
    imeiNo: '',
    loginType: '0',
    mobileNo: '',
    countryCode: '1',
    updatedOn: '1970-01-01 00:00:00',
  });

export const getGroup = (groupId, memberProfileId) =>
  api.post('/Group/GetGroupDetail', { groupId, memberProfileId });

export const createGroup = (data) =>
  api.post('/Group/CreateGroup', data);

export const getGroupMembers = (grpID, updatedOn = '1970-01-01 00:00:00') =>
  api.post('/Member/GetMemberListSync', { grpID, updatedOn });

export const getGroupModules = (groupId) =>
  api.post('/Group/GetGroupModulesList', { groupId });

export const getEntityInfo = (grpID) =>
  api.post('/Group/GetEntityInfo', { grpID });

export const getClubList = () =>
  api.post('/FindClub/GetClubList', { keyword: '', country: '', meetingDay: '', district: '', stateProvinceCity: '' });

export const getClubDetails = (grpId) =>
  api.post('/FindClub/GetClubDetails', { grpId });

export const updateGroup = (id, data) =>
  api.post('/Group/CreateGroup', {
    grpId: String(id),
    grpName: data.GrpName,
    grpCategory: data.GrpCategory,
    addrss1: data.Address1,
    city: data.City,
    state: data.State,
    pincode: data.Pincode,
    country: data.Country,
    emailid: data.Email,
    website: data.Website,
    other: data.MeetingDay,
  });

export const deleteGroup = (id) =>
  api.post('/Group/DeleteGroup', { groupId: id });