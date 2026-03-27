import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../../context/AuthContext';

export default function Navbar({ onToggleSidebar }) {
  const { user, logout } = useAuth();
  const navigate = useNavigate();
  const [dropdownOpen, setDropdownOpen] = useState(false);

  const handleLogout = () => {
    logout();
    navigate('/login');
  };

  const userName = user?.firstName || user?.mobileNo || 'User';

  return (
    <header
      style={{
        backgroundColor: '#1a297d',
        color: '#fff',
        minHeight: '90px',
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'space-between',
        padding: '0 15px',
        fontSize: '14px',
      }}
    >
      {/* Left: hamburger */}
      <div style={{ display: 'flex', alignItems: 'center', gap: '10px', flex: '0 0 auto' }}>
        <button
          onClick={onToggleSidebar}
          className="lg:hidden"
          style={{ background: 'none', border: 'none', color: '#fff', padding: '4px', cursor: 'pointer' }}
        >
          <svg width="24" height="24" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M4 6h16M4 12h16M4 18h16" />
          </svg>
        </button>
      </div>

      {/* Center: logo */}
      <div style={{ flex: '1 1 auto', display: 'flex', justifyContent: 'center' }}>
        <img src="/ime_logo.png" alt="IME Logo" style={{ maxWidth: '600px', height: '75px', objectFit: 'contain' }} />
      </div>

      {/* Right: Welcome user dropdown */}
      <div style={{ position: 'relative' }}>
        <button
          onClick={() => setDropdownOpen(!dropdownOpen)}
          style={{
            display: 'flex', alignItems: 'center', gap: '8px',
            background: 'none', border: 'none', cursor: 'pointer',
            color: '#fff', fontSize: '13px', padding: '5px 10px',
          }}
        >
          {/* User icon circle */}
          <span style={{
            display: 'flex', alignItems: 'center', justifyContent: 'center',
            backgroundColor: '#fff', color: '#5997ff', width: '30px', height: '30px',
            borderRadius: '50%', fontSize: '16px', flexShrink: 0,
          }}>
            <svg width="16" height="16" fill="currentColor" viewBox="0 0 20 20">
              <path fillRule="evenodd" d="M10 9a3 3 0 100-6 3 3 0 000 6zm-7 9a7 7 0 1114 0H3z" clipRule="evenodd" />
            </svg>
          </span>
          <span>Welcome {userName} &gt;</span>
        </button>

        {dropdownOpen && (
          <>
            <div style={{ position: 'fixed', inset: 0, zIndex: 40 }} onClick={() => setDropdownOpen(false)} />
            <div style={{
              position: 'absolute', right: 0, zIndex: 50, marginTop: '4px', width: '190px',
              backgroundColor: '#fff', borderRadius: '10px', overflow: 'hidden',
              boxShadow: '0 6px 12px rgba(0,0,0,.175)',
            }}>
              <button
                onClick={() => { setDropdownOpen(false); navigate('/settings'); }}
                style={{
                  width: '100%', textAlign: 'left', padding: '12px 20px', fontSize: '13px',
                  display: 'flex', alignItems: 'center', gap: '8px',
                  color: '#333', background: 'none', border: 'none', cursor: 'pointer',
                }}
                onMouseEnter={(e) => { e.currentTarget.style.backgroundColor = '#ffca28'; e.currentTarget.style.color = '#fff'; }}
                onMouseLeave={(e) => { e.currentTarget.style.backgroundColor = ''; e.currentTarget.style.color = '#333'; }}
              >
                <svg width="14" height="14" fill="none" stroke="#1a297d" viewBox="0 0 24 24" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
                  <rect x="3" y="11" width="18" height="11" rx="2" ry="2" /><path d="M7 11V7a5 5 0 0110 0v4" />
                </svg>
                Change Password
              </button>
              <div style={{ height: '1px', backgroundColor: '#e5e5e5' }} />
              <button
                onClick={handleLogout}
                style={{
                  width: '100%', textAlign: 'left', padding: '12px 20px', fontSize: '13px',
                  display: 'flex', alignItems: 'center', gap: '8px',
                  color: '#333', background: 'none', border: 'none', cursor: 'pointer',
                }}
                onMouseEnter={(e) => { e.currentTarget.style.backgroundColor = '#ffca28'; e.currentTarget.style.color = '#fff'; }}
                onMouseLeave={(e) => { e.currentTarget.style.backgroundColor = ''; e.currentTarget.style.color = '#333'; }}
              >
                <svg width="14" height="14" fill="none" stroke="#1a297d" viewBox="0 0 24 24" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
                  <path d="M9 21H5a2 2 0 01-2-2V5a2 2 0 012-2h4" /><polyline points="16 17 21 12 16 7" /><line x1="21" y1="12" x2="9" y2="12" />
                </svg>
                Logout
              </button>
            </div>
          </>
        )}
      </div>
    </header>
  );
}
