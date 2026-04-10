import api from './axiosInstance';

export const getEvents = (grpId, groupProfileID, flag = '0') =>
  api.post('/Event/GetEventList', { grpId, groupProfileID, flag });

export const getEvent = (eventId, groupId) =>
  api.post('/Event/GetEventDetails', { eventID: String(eventId), groupProfileID: String(groupId || '0') });

export const createEvent = (data) =>
  api.post('/Event/AddEvent_New', data);

export const updateEvent = (data) =>
  api.post('/Event/AddEvent_New', data);

export const deleteEvent = (eventId) =>
  api.post('/Event/DeleteEvent', { eventId });

export const answerEvent = (data) =>
  api.post('/Event/AnsweringEvent', data);

export const getEventExtras = (eventID) =>
  api.post('/Event/GetEventExtras', { eventID: String(eventID) });

export const saveEventExtras = (data) =>
  api.post('/Event/SaveEventExtras', data);

// Upload an agenda or minutes-of-meeting file for an event.
// docType must be "agenda" or "minutes". Response: { status, url, fileName }.
export const uploadEventDoc = (file, eventID, docType) => {
  const formData = new FormData();
  formData.append('file', file);
  formData.append('eventID', String(eventID ?? ''));
  formData.append('docType', docType);
  return api.post('/Event/UploadEventDoc', formData, {
    headers: { 'Content-Type': 'multipart/form-data' },
  });
};

export const getUpcomingEvents = (GroupID, groupCategory, SelectedDate, Type) =>
  api.post('/Celebrations/GetMonthEventListTypeWise_National', {
    GroupID, groupCategory, SelectedDate, Type,
  });