import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import { changePassword } from '../api/authService';

const inputStyle = {
  width: '100%', height: '38px', border: '1px solid #ccc',
  borderRadius: '4px', padding: '6px 12px', fontSize: '13px', outline: 'none',
};

const labelStyle = {
  display: 'block', fontWeight: '500', fontSize: '13px', color: '#333',
  marginBottom: '6px',
};

export default function ChangePasswordPage() {
  const navigate = useNavigate();
  const { user } = useAuth();
  const [oldPassword, setOldPassword] = useState('');
  const [newPassword, setNewPassword] = useState('');
  const [confirmPassword, setConfirmPassword] = useState('');
  const [saving, setSaving] = useState(false);

  const handleSubmit = async () => {
    if (!oldPassword.trim()) { alert('Please enter old password'); return; }
    if (!newPassword.trim()) { alert('Please enter new password'); return; }
    if (!confirmPassword.trim()) { alert('Please enter confirm password'); return; }
    if (newPassword !== confirmPassword) { alert('New password and confirm password do not match'); return; }
    if (newPassword.length < 4) { alert('New password must be at least 4 characters'); return; }

    setSaving(true);
    try {
      const mobileNo = user?.mobile || user?.mobileNo || '';
      const res = await changePassword(mobileNo, oldPassword, newPassword);
      if (res.data?.status === '0') {
        alert('Password changed successfully');
        setOldPassword('');
        setNewPassword('');
        setConfirmPassword('');
        navigate('/dashboard');
      } else {
        alert(res.data?.message || 'Failed to change password');
      }
    } catch (err) {
      alert(err.response?.data?.message || err.message || 'Failed to change password');
    } finally { setSaving(false); }
  };

  return (
    <div>
      {/* Title Row */}
      <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '20px' }}>
        <div style={{ fontSize: '14px', fontWeight: 'bold', color: '#333' }}>Change Password</div>
        <div style={{ display: 'flex', gap: '8px' }}>
          <button onClick={handleSubmit} disabled={saving} style={{ display: 'flex', alignItems: 'center', gap: '4px', backgroundColor: '#1a297d', color: '#fff', border: 'none', padding: '7px 16px', borderRadius: '4px', fontSize: '13px', cursor: saving ? 'not-allowed' : 'pointer', opacity: saving ? 0.6 : 1 }}>
            <svg width="12" height="12" fill="none" stroke="currentColor" viewBox="0 0 24 24" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
              <rect x="3" y="11" width="18" height="11" rx="2" ry="2" /><path d="M7 11V7a5 5 0 0110 0v4" />
            </svg>
            Change Password
          </button>
          <button onClick={() => navigate('/dashboard')} style={{ display: 'flex', alignItems: 'center', gap: '6px', backgroundColor: '#1a297d', color: '#fff', border: 'none', padding: '7px 16px', borderRadius: '4px', fontSize: '13px', cursor: 'pointer' }}>
            <svg width="12" height="12" fill="none" stroke="currentColor" viewBox="0 0 24 24" strokeWidth="2.5" strokeLinecap="round" strokeLinejoin="round"><path d="M19 12H5M12 19l-7-7 7-7" /></svg>
            Back
          </button>
        </div>
      </div>

      {/* Form */}
      <div style={{ backgroundColor: '#fff', borderRadius: '4px', padding: '25px 30px', boxShadow: '0 1px 3px rgba(0,0,0,0.08)' }}>
        <div style={{ display: 'flex', gap: '30px' }}>
          <div style={{ flex: 1 }}>
            <label style={labelStyle}>Old password :</label>
            <input type="password" value={oldPassword} onChange={(e) => setOldPassword(e.target.value)} style={inputStyle} />
          </div>
          <div style={{ flex: 1 }}>
            <label style={labelStyle}>New password :</label>
            <input type="password" value={newPassword} onChange={(e) => setNewPassword(e.target.value)} style={inputStyle} />
          </div>
          <div style={{ flex: 1 }}>
            <label style={labelStyle}>Confirm password :</label>
            <input type="password" value={confirmPassword} onChange={(e) => setConfirmPassword(e.target.value)} style={inputStyle} />
          </div>
        </div>
      </div>
    </div>
  );
}