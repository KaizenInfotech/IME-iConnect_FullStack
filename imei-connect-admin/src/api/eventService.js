import api from './axiosInstance';

export const getEvents = (groupId, memberProfileId, flag = '0') =>
  api.post('/Event/GetEventList', { groupId, memberProfileId, flag });

export const getEvent = (eventId, groupId) =>
  api.post('/Event/GetEventDetails', { eventId, groupId });

export const createEvent = (data) =>
  api.post('/Event/AddEventNew', data);

export const updateEvent = (data) =>
  api.post('/Event/AddEventNew', data);

export const deleteEvent = (eventId) =>
  api.post('/Event/DeleteEvent', { eventId });

export const answerEvent = (data) =>
  api.post('/Event/AnsweringEvent', data);

export const getUpcomingEvents = (GroupID, groupCategory, SelectedDate, Type) =>
  api.post('/Celebrations/GetMonthEventListTypeWise_National', {
    GroupID, groupCategory, SelectedDate, Type,
  });