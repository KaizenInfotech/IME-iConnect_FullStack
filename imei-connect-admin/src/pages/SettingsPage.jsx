import { useState } from 'react';
import PageHeader from '../components/shared/PageHeader';
import { useAuth } from '../context/AuthContext';

export default function SettingsPage() {
  const { user, logout } = useAuth();
  const [saved, setSaved] = useState(false);

  const handleSave = (e) => {
    e.preventDefault();
    setSaved(true);
    setTimeout(() => setSaved(false), 3000);
  };

  return (
    <div>
      <PageHeader title="Settings" />

      <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
        <div className="bg-white rounded-xl shadow-sm p-6">
          <h3 className="text-lg font-semibold text-gray-800 mb-4">Profile Information</h3>
          <form onSubmit={handleSave} className="space-y-4">
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">User ID</label>
              <input disabled value={user?.userId || '-'} className="w-full border border-gray-200 rounded-lg px-3 py-2 bg-gray-50 text-gray-500" />
            </div>
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">Role</label>
              <input disabled value={user?.role || '-'} className="w-full border border-gray-200 rounded-lg px-3 py-2 bg-gray-50 text-gray-500" />
            </div>
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">Group ID</label>
              <input disabled value={user?.groupId || '-'} className="w-full border border-gray-200 rounded-lg px-3 py-2 bg-gray-50 text-gray-500" />
            </div>
            {saved && <div className="bg-green-50 text-green-600 px-4 py-3 rounded-lg text-sm">Settings saved successfully</div>}
          </form>
        </div>

        <div className="bg-white rounded-xl shadow-sm p-6">
          <h3 className="text-lg font-semibold text-gray-800 mb-4">Application Settings</h3>
          <div className="space-y-4">
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">API Endpoint</label>
              <input disabled value={import.meta.env.VITE_API_URL || 'http://localhost:5000/api'} className="w-full border border-gray-200 rounded-lg px-3 py-2 bg-gray-50 text-gray-500" />
            </div>
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">Frontend Version</label>
              <input disabled value="1.0.0 (Local Development)" className="w-full border border-gray-200 rounded-lg px-3 py-2 bg-gray-50 text-gray-500" />
            </div>
          </div>
        </div>

        <div className="bg-white rounded-xl shadow-sm p-6">
          <h3 className="text-lg font-semibold text-gray-800 mb-4">Session</h3>
          <p className="text-sm text-gray-600 mb-4">Sign out of your current session. You will need to log in again.</p>
          <button onClick={logout} className="px-4 py-2 bg-red-600 text-white rounded-lg hover:bg-red-700 transition-colors">Sign Out</button>
        </div>
      </div>
    </div>
  );
}
