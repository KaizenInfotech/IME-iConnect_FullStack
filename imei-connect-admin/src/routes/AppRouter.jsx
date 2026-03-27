import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import Layout from '../components/layout/Layout';
import LoadingSpinner from '../components/shared/LoadingSpinner';

import LoginPage from '../pages/LoginPage';
import DashboardPage from '../pages/DashboardPage';
import GroupsPage from '../pages/GroupsPage';
import GroupDetailPage from '../pages/GroupDetailPage';
import AddGroupPage from '../pages/AddGroupPage';
import EditGroupPage from '../pages/EditGroupPage';
import AddMemberPage from '../pages/AddMemberPage';
import MembersPage from '../pages/MembersPage';
import MemberDetailPage from '../pages/MemberDetailPage';
import EventsPage from '../pages/EventsPage';
import EventDetailPage from '../pages/EventDetailPage';
import AnnouncementsPage from '../pages/AnnouncementsPage';
import AttendancePage from '../pages/AttendancePage';
import AttendanceDetailPage from '../pages/AttendanceDetailPage';
import DocumentsPage from '../pages/DocumentsPage';
import EbulletinsPage from '../pages/EbulletinsPage';
import AlbumsPage from '../pages/AlbumsPage';
import AlbumDetailPage from '../pages/AlbumDetailPage';
import ServiceDirectoryPage from '../pages/ServiceDirectoryPage';
import PastPresidentsPage from '../pages/PastPresidentsPage';
import WebLinksPage from '../pages/WebLinksPage';
import SubGroupsPage from '../pages/SubGroupsPage';
import MerPage from '../pages/MerPage';
import AddMerPage from '../pages/AddMerPage';
import EditMerPage from '../pages/EditMerPage';
import IMelangePage from '../pages/IMelangePage';
import AddIMelangePage from '../pages/AddIMelangePage';
import EditIMelangePage from '../pages/EditIMelangePage';
import GoverningCouncilPage from '../pages/GoverningCouncilPage';
import AddAnnouncementPage from '../pages/AddAnnouncementPage';
import EditAnnouncementPage from '../pages/EditAnnouncementPage';
import UpcomingEventsPage from '../pages/UpcomingEventsPage';
import EditUpcomingEventPage from '../pages/EditUpcomingEventPage';
import AddUpcomingEventPage from '../pages/AddUpcomingEventPage';
import ReportsPage from '../pages/ReportsPage';
import BannersPage from '../pages/BannersPage';
import SettingsPage from '../pages/SettingsPage';
import ExecutiveCommitteePage from '../pages/ExecutiveCommitteePage';
import NotificationPage from '../pages/NotificationPage';

function ProtectedRoute({ children }) {
  const { isAuthenticated, loading } = useAuth();
  if (loading) return <LoadingSpinner className="h-screen" size="lg" />;
  if (!isAuthenticated) return <Navigate to="/login" replace />;
  return children;
}

export default function AppRouter() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/login" element={<LoginPage />} />
        <Route
          path="/"
          element={
            <ProtectedRoute>
              <Layout />
            </ProtectedRoute>
          }
        >
          <Route index element={<Navigate to="/dashboard" replace />} />
          <Route path="dashboard" element={<DashboardPage />} />
          <Route path="groups" element={<GroupsPage />} />
          <Route path="groups/add" element={<AddGroupPage />} />
          <Route path="groups/:id" element={<GroupDetailPage />} />
          <Route path="groups/:id/edit" element={<EditGroupPage />} />
          <Route path="members" element={<MembersPage />} />
          <Route path="members/add" element={<AddMemberPage />} />
          <Route path="members/:id" element={<MemberDetailPage />} />
          <Route path="events" element={<EventsPage />} />
          <Route path="events/:id" element={<EventDetailPage />} />
          <Route path="announcements" element={<AnnouncementsPage />} />
          <Route path="announcements/add" element={<AddAnnouncementPage />} />
          <Route path="announcements/:id/edit" element={<EditAnnouncementPage />} />
          <Route path="upcoming-events" element={<UpcomingEventsPage />} />
          <Route path="events/add" element={<AddUpcomingEventPage />} />
          <Route path="reports" element={<ReportsPage />} />
          <Route path="events/:id/edit" element={<EditUpcomingEventPage />} />
          <Route path="attendance" element={<AttendancePage />} />
          <Route path="attendance/:id" element={<AttendanceDetailPage />} />
          <Route path="documents" element={<DocumentsPage />} />
          <Route path="ebulletins" element={<EbulletinsPage />} />
          <Route path="albums" element={<AlbumsPage />} />
          <Route path="albums/:id" element={<AlbumDetailPage />} />
          <Route path="service-directory" element={<ServiceDirectoryPage />} />
          <Route path="past-presidents" element={<PastPresidentsPage />} />
          <Route path="web-links" element={<WebLinksPage />} />
          <Route path="sub-groups" element={<SubGroupsPage />} />
          <Route path="mer" element={<MerPage />} />
          <Route path="mer/add" element={<AddMerPage />} />
          <Route path="mer/:id/edit" element={<EditMerPage />} />
          <Route path="imelange" element={<IMelangePage />} />
          <Route path="imelange/add" element={<AddIMelangePage />} />
          <Route path="imelange/:id/edit" element={<EditIMelangePage />} />
          <Route path="governing-council" element={<GoverningCouncilPage />} />
          <Route path="executive-committee" element={<ExecutiveCommitteePage />} />
          <Route path="notifications" element={<NotificationPage />} />
          <Route path="banners" element={<BannersPage />} />
          <Route path="settings" element={<SettingsPage />} />
        </Route>
        <Route path="*" element={<Navigate to="/dashboard" replace />} />
      </Routes>
    </BrowserRouter>
  );
}
