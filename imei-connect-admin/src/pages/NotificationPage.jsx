import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import api from '../api/axiosInstance';

export default function NotificationPage() {
  const navigate = useNavigate();
  const [sending, setSending] = useState(false);
  const [error, setError] = useState('');

  const [form, setForm] = useState({
    Title: '',
    Description: '',
    Android: true,
    iOS: true,
  });

  const handleSend = async () => {
    if (!form.Title.trim()) { alert('Please Enter Title'); return; }
    if (!form.Description.trim()) { alert('Please Enter Description'); return; }
    if (!form.Android && !form.iOS) { alert('Please Select Device'); return; }
    setSending(true);
    setError('');
    try {
      await api.post('/Notification/SendPushNotification', {
        title: form.Title,
        body: form.Description,
        sendToAndroid: form.Android,
        sendToiOS: form.iOS,
      });
      alert('Notification sent successfully');
      setForm({ Title: '', Description: '', Android: true, iOS: true });
    } catch (err) {
      setError(err.response?.data?.message || 'Failed to send notification');
    } finally { setSending(false); }
  };

  return (
    <div>
      {/* Title Row */}
      <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '15px' }}>
        <div>
          <span style={{ color: '#1a297d', fontSize: '14px' }}>National Admin</span>
          <span style={{ fontSize: '14px', fontWeight: 'bold', color: '#333' }}> - Notification</span>
        </div>
        <button onClick={() => navigate('/dashboard')} style={{ display: 'flex', alignItems: 'center', gap: '6px', backgroundColor: '#1a297d', color: '#fff', border: 'none', padding: '6px 14px', borderRadius: '4px', fontSize: '13px', cursor: 'pointer' }}>
          <svg width="12" height="12" fill="none" stroke="currentColor" viewBox="0 0 24 24" strokeWidth="2.5" strokeLinecap="round" strokeLinejoin="round"><path d="M19 12H5M12 19l-7-7 7-7" /></svg>
          Back
        </button>
      </div>

      {error && (
        <div style={{ backgroundColor: '#fef2f2', color: '#dc2626', padding: '10px 16px', borderRadius: '4px', marginBottom: '12px', fontSize: '13px' }}>
          {error}
        </div>
      )}

      {/* Push Notification Form */}
      <div style={{ backgroundColor: '#fff', borderRadius: '8px', padding: '25px', boxShadow: '0 3px 5px 0px rgba(0,0,0,0.06)', maxWidth: '600px' }}>
        <h4 style={{ fontSize: '14px', fontWeight: 'bold', textTransform: 'uppercase', borderBottom: '1px solid #bbb', paddingBottom: '10px', marginBottom: '20px', color: '#333' }}>
          Send Push Notification
        </h4>

        {/* Title */}
        <div style={{ marginBottom: '15px' }}>
          <label style={{ display: 'block', fontSize: '12px', fontWeight: '600', color: '#333', marginBottom: '5px' }}>Title</label>
          <input
            type="text"
            value={form.Title}
            onChange={(e) => setForm({ ...form, Title: e.target.value })}
            placeholder="Enter Title"
            style={{ width: '100%', height: '36px', border: '1px solid #ccc', borderRadius: '4px', padding: '4px 10px', fontSize: '13px', outline: 'none' }}
          />
        </div>

        {/* Description */}
        <div style={{ marginBottom: '15px' }}>
          <label style={{ display: 'block', fontSize: '12px', fontWeight: '600', color: '#333', marginBottom: '5px' }}>Description</label>
          <textarea
            value={form.Description}
            onChange={(e) => setForm({ ...form, Description: e.target.value })}
            placeholder="Enter Description"
            rows={4}
            style={{ width: '100%', border: '1px solid #ccc', borderRadius: '4px', padding: '8px 10px', fontSize: '13px', outline: 'none', resize: 'vertical' }}
          />
        </div>

        {/* Device Selection */}
        <div style={{ marginBottom: '20px' }}>
          <label style={{ display: 'block', fontSize: '12px', fontWeight: '600', color: '#333', marginBottom: '8px' }}>Send To</label>
          <div style={{ display: 'flex', gap: '20px' }}>
            <label style={{ display: 'flex', alignItems: 'center', gap: '6px', fontSize: '13px', cursor: 'pointer' }}>
              <input
                type="checkbox"
                checked={form.Android}
                onChange={(e) => setForm({ ...form, Android: e.target.checked })}
                style={{ cursor: 'pointer' }}
              />
              Android
            </label>
            <label style={{ display: 'flex', alignItems: 'center', gap: '6px', fontSize: '13px', cursor: 'pointer' }}>
              <input
                type="checkbox"
                checked={form.iOS}
                onChange={(e) => setForm({ ...form, iOS: e.target.checked })}
                style={{ cursor: 'pointer' }}
              />
              iOS
            </label>
          </div>
        </div>

        {/* Send Button */}
        <button
          onClick={handleSend}
          disabled={sending}
          style={{
            backgroundColor: '#1a297d', color: '#fff', border: 'none',
            padding: '8px 30px', borderRadius: '4px', fontSize: '13px',
            fontWeight: '600', cursor: 'pointer', opacity: sending ? 0.6 : 1,
          }}
        >
          {sending ? 'Sending...' : 'Send Notification'}
        </button>
      </div>
    </div>
  );
}