import { useState } from 'react';
import { Outlet, useLocation } from 'react-router-dom';
import Sidebar from './Sidebar';
import Navbar from './Navbar';

export default function Layout() {
  const [sidebarOpen, setSidebarOpen] = useState(false);
  const location = useLocation();

  // Dashboard page has no sidebar — matches production screenshot
  const isDashboard = location.pathname === '/dashboard' || location.pathname === '/';

  return (
    <div style={{ minHeight: '100vh', backgroundColor: '#f0f0f0' }}>
      <Navbar onToggleSidebar={() => setSidebarOpen(!sidebarOpen)} />
      <div style={{ display: 'flex' }}>
        {!isDashboard && (
          <Sidebar isOpen={sidebarOpen} onClose={() => setSidebarOpen(false)} />
        )}
        <main style={{
          flex: 1,
          padding: '20px',
          minHeight: 'calc(100vh - 50px)',
          overflow: 'auto',
        }}>
          <Outlet />
        </main>
      </div>
    </div>
  );
}
