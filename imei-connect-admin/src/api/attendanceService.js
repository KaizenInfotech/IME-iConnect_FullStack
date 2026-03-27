import api from './axiosInstance';

export const getAttendanceRecords = (GroupId) =>
  api.post('/Attendance/GetAttendanceListNew', { GroupId });

export const getAttendanceDetails = (AttendanceID) =>
  api.post('/Attendance/getAttendanceDetails', { AttendanceID, type: 0, createdBy: 0 });

export const getAttendanceMembers = (AttendanceID) =>
  api.post('/Attendance/getAttendanceMemberDetails', { AttendanceID, type: 1 });

export const getAttendanceVisitors = (AttendanceID) =>
  api.post('/Attendance/getAttendanceVisitorsDetails', { AttendanceID, type: 2 });

export const createAttendance = (data) =>
  api.post('/Attendance/AttendanceAddEdit', data);

export const deleteAttendance = (AttendanceID, createdBy) =>
  api.post('/Attendance/AttendanceDelete', { AttendanceID, createdBy });

export const getAttendanceRecord = (AttendanceID) =>
  api.post('/Attendance/getAttendanceDetails', { AttendanceID, type: 0, createdBy: 0 });

export const updateAttendance = (data) =>
  api.post('/Attendance/AttendanceAddEdit', data);