import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { createAnnouncement } from '../api/announcementService';

const inputStyle = {
  width: '100%', height: '34px', border: '1px solid #ccc',
  borderRadius: '2px', padding: '4px 10px', fontSize: '13px', outline: 'none',
};

const labelStyle = {
  display: 'block', fontWeight: '600', fontSize: '12px', color: '#333',
  marginBottom: '4px', marginTop: '10px',
};

export default function AddAnnouncementPage() {
  const navigate = useNavigate();
  const [saving, setSaving] = useState(false);
  const [error, setError] = useState('');
  const [imagePreview, setImagePreview] = useState(null);

  const [form, setForm] = useState({
    AnnounTitle: '',
    AnnounDesc: '',
    PublishDate: '',
    ExpiryDate: '',
    RegLink: '',
    AnnounImg: null,
  });

  const handleFileChange = (e) => {
    const file = e.target.files[0];
    if (file) {
      setForm({ ...form, AnnounImg: file });
      const reader = new FileReader();
      reader.onload = (ev) => setImagePreview(ev.target.result);
      reader.readAsDataURL(file);
    }
  };

  const handleSubmit = async () => {
    if (!form.AnnounTitle.trim()) { alert('Please Enter Title'); return; }
    if (!form.PublishDate) { alert('Please Enter Publish Date & Time'); return; }
    if (!form.ExpiryDate) { alert('Please Enter Expiry Date & Time'); return; }
    setSaving(true);
    setError('');
    try {
      await createAnnouncement(form);
      navigate('/announcements');
    } catch (err) {
      setError(err.response?.data?.message || err.message || 'Save failed');
    } finally { setSaving(false); }
  };

  return (
    <div>
      {/* Title Row */}
      <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '20px' }}>
        <div>
          <span style={{ color: '#1a297d', fontSize: '14px' }}>National Admin - Announcements</span>
          <span style={{ fontSize: '14px', fontWeight: 'bold', color: '#333' }}> - Add Announcements</span>
        </div>
        <div style={{ display: 'flex', gap: '8px' }}>
          <button
            onClick={handleSubmit}
            disabled={saving}
            style={{
              display: 'flex', alignItems: 'center', gap: '4px',
              backgroundColor: '#1a297d', color: '#fff', border: 'none',
              padding: '7px 16px', borderRadius: '4px', fontSize: '13px',
              cursor: saving ? 'not-allowed' : 'pointer', opacity: saving ? 0.6 : 1,
            }}
          >
            <svg width="12" height="12" fill="currentColor" viewBox="0 0 20 20"><path d="M3 3h11.586l2.707 2.707A1 1 0 0117.586 6H18v11a2 2 0 01-2 2H4a2 2 0 01-2-2V5a2 2 0 012-2zm2 10v4h10v-4H5zm0-6v4h7V7H5z" /></svg>
            Save
          </button>
          <button
            onClick={() => navigate('/announcements')}
            style={{
              display: 'flex', alignItems: 'center', gap: '6px',
              backgroundColor: '#1a297d', color: '#fff', border: 'none',
              padding: '7px 16px', borderRadius: '4px', fontSize: '13px',
              cursor: 'pointer',
            }}
          >
            <svg width="12" height="12" fill="none" stroke="currentColor" viewBox="0 0 24 24" strokeWidth="2.5" strokeLinecap="round" strokeLinejoin="round">
              <path d="M19 12H5M12 19l-7-7 7-7" />
            </svg>
            Back
          </button>
        </div>
      </div>

      {error && (
        <div style={{ backgroundColor: '#fef2f2', color: '#dc2626', padding: '10px 16px', borderRadius: '4px', marginBottom: '12px', fontSize: '13px' }}>{error}</div>
      )}

      {/* Form */}
      <div style={{ backgroundColor: '#fff', borderRadius: '4px', padding: '25px 30px', boxShadow: '0 1px 3px rgba(0,0,0,0.08)' }}>
        <div style={{ display: 'flex', gap: '30px' }}>

          {/* Left side */}
          <div style={{ flex: '1 1 60%' }}>
            {/* Title */}
            <div style={{ marginBottom: '10px' }}>
              <label style={labelStyle}>Title</label>
              <input
                type="text"
                value={form.AnnounTitle}
                onChange={(e) => setForm({ ...form, AnnounTitle: e.target.value })}
                style={inputStyle}
              />
            </div>

            {/* Description */}
            <div style={{ marginBottom: '10px' }}>
              <label style={labelStyle}>Description</label>
              <textarea
                value={form.AnnounDesc}
                onChange={(e) => setForm({ ...form, AnnounDesc: e.target.value })}
                rows={4}
                style={{ ...inputStyle, height: 'auto', resize: 'vertical' }}
              />
            </div>

            {/* Publish Date & Expiry Date */}
            <div style={{ display: 'flex', gap: '20px', marginBottom: '10px' }}>
              <div style={{ flex: 1 }}>
                <label style={labelStyle}>Publish Date & Time</label>
                <input
                  type="datetime-local"
                  value={form.PublishDate}
                  onChange={(e) => setForm({ ...form, PublishDate: e.target.value })}
                  style={inputStyle}
                />
              </div>
              <div style={{ flex: 1 }}>
                <label style={labelStyle}>Expiry Date & Time</label>
                <input
                  type="datetime-local"
                  value={form.ExpiryDate}
                  onChange={(e) => setForm({ ...form, ExpiryDate: e.target.value })}
                  style={inputStyle}
                />
              </div>
            </div>

            {/* Registration Link */}
            <div style={{ marginBottom: '10px' }}>
              <label style={labelStyle}>Registration Link</label>
              <input
                type="text"
                value={form.RegLink}
                onChange={(e) => setForm({ ...form, RegLink: e.target.value })}
                style={{ ...inputStyle, maxWidth: '50%' }}
              />
            </div>
          </div>

          {/* Right side - Image */}
          <div style={{ flex: '0 0 180px', paddingTop: '10px' }}>
            <label style={labelStyle}>Image</label>
            <div style={{
              width: '120px', height: '90px', border: '1px solid #ccc',
              display: 'flex', alignItems: 'center', justifyContent: 'center',
              marginBottom: '6px', backgroundColor: '#f5f5f5', borderRadius: '2px',
              overflow: 'hidden',
            }}>
              {imagePreview ? (
                <img src={imagePreview} alt="Preview" style={{ maxWidth: '100%', maxHeight: '100%', objectFit: 'contain' }} />
              ) : (
                <div style={{ textAlign: 'center', color: '#999', fontSize: '11px' }}>
                  <svg width="30" height="30" fill="#ccc" viewBox="0 0 24 24" style={{ display: 'block', margin: '0 auto 4px' }}>
                    <path d="M21 19V5c0-1.1-.9-2-2-2H5c-1.1 0-2 .9-2 2v14c0 1.1.9 2 2 2h14c1.1 0 2-.9 2-2zM8.5 13.5l2.5 3.01L14.5 12l4.5 6H5l3.5-4.5z" />
                  </svg>
                  No Image
                </div>
              )}
            </div>
            <div style={{ fontSize: '11px', fontWeight: 'bold', color: '#333', marginBottom: '4px' }}>ATTACH FILE</div>
            <input
              type="file"
              accept="image/jpeg,image/jpg,image/png,image/gif,image/bmp"
              onChange={handleFileChange}
              style={{ fontSize: '11px', width: '170px' }}
            />
          </div>

        </div>
      </div>
    </div>
  );
}
