import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { createMerItem } from '../api/merService';

const titleOptions = [
  '-Select-', 'January', 'February', 'March', 'April', 'May', 'June',
  'July', 'August', 'September', 'October', 'November', 'December',
];

const inputStyle = {
  width: '100%', height: '34px', border: '1px solid #ccc',
  borderRadius: '2px', padding: '4px 10px', fontSize: '13px', outline: 'none',
};

const labelStyle = {
  display: 'block', fontWeight: '600', fontSize: '12px', color: '#333',
  marginBottom: '4px',
};

export default function AddMerPage() {
  const navigate = useNavigate();
  const [saving, setSaving] = useState(false);
  const [error, setError] = useState('');

  const [form, setForm] = useState({
    Title: '',
    PublishDate: '',
    SaveType: 'link', // 'link' or 'file'
    Link: '',
    File: null,
  });

  const handleSubmit = async () => {
    if (!form.Title || form.Title === '-Select-') { alert('Please Select Title'); return; }
    if (!form.PublishDate.trim()) { alert('Please Enter Publish Date & Time'); return; }
    if (form.SaveType === 'link' && !form.Link.trim()) { alert('Please Enter Link'); return; }
    if (form.SaveType === 'file' && !form.File) { alert('Please Attach File'); return; }

    setSaving(true);
    setError('');
    try {
      await createMerItem({
        Title: form.Title,
        PublishDate: form.PublishDate,
        Link: form.SaveType === 'link' ? form.Link : '',
        FilePath: form.SaveType === 'file' ? form.File?.name : '',
        TransType: '1',
        FinanceYear: String(new Date().getFullYear()),
      });
      navigate('/mer');
    } catch (err) {
      setError(err.response?.data?.message || err.message || 'Save failed');
    } finally { setSaving(false); }
  };

  return (
    <div>
      {/* Title Row */}
      <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '20px' }}>
        <div>
          <span style={{ color: '#1a297d', fontSize: '14px' }}>National Admin - MER (I)</span>
          <span style={{ fontSize: '14px', fontWeight: 'bold', color: '#333' }}> - Add MER (I)</span>
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
            onClick={() => navigate('/mer')}
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
        <div style={{ backgroundColor: '#fef2f2', color: '#dc2626', padding: '10px 16px', borderRadius: '4px', marginBottom: '12px', fontSize: '13px' }}>
          {error}
        </div>
      )}

      {/* Form */}
      <div style={{ backgroundColor: '#fff', borderRadius: '4px', padding: '25px 30px', boxShadow: '0 1px 3px rgba(0,0,0,0.08)' }}>

        {/* Row 1: Title + Publish Date & Time */}
        <div style={{ display: 'flex', gap: '20px', marginBottom: '15px' }}>
          <div style={{ flex: '0 0 180px' }}>
            <label style={labelStyle}>Title</label>
            <select
              value={form.Title}
              onChange={(e) => setForm({ ...form, Title: e.target.value })}
              style={{ ...inputStyle, backgroundColor: '#fff' }}
            >
              {titleOptions.map(t => <option key={t} value={t === '-Select-' ? '' : t}>{t}</option>)}
            </select>
          </div>
          <div style={{ flex: '0 0 300px' }}>
            <label style={labelStyle}>Publish Date & Time</label>
            <input
              type="datetime-local"
              value={form.PublishDate}
              onChange={(e) => setForm({ ...form, PublishDate: e.target.value })}
              style={inputStyle}
            />
          </div>
        </div>

        {/* Row 2: Radio buttons */}
        <div style={{ marginBottom: '15px', display: 'flex', alignItems: 'center', gap: '25px' }}>
          <label style={{ display: 'flex', alignItems: 'center', gap: '5px', fontSize: '13px', cursor: 'pointer' }}>
            <input
              type="radio"
              name="saveType"
              checked={form.SaveType === 'link'}
              onChange={() => setForm({ ...form, SaveType: 'link', File: null })}
            />
            Save Link
          </label>
          <label style={{ display: 'flex', alignItems: 'center', gap: '5px', fontSize: '13px', cursor: 'pointer' }}>
            <input
              type="radio"
              name="saveType"
              checked={form.SaveType === 'file'}
              onChange={() => setForm({ ...form, SaveType: 'file', Link: '' })}
            />
            Attach File (only .Pdf file)
          </label>
        </div>

        {/* Row 3: Conditional — Link input or File upload */}
        {form.SaveType === 'link' && (
          <div style={{ marginBottom: '15px' }}>
            <label style={labelStyle}>Link :</label>
            <input
              type="text"
              value={form.Link}
              onChange={(e) => setForm({ ...form, Link: e.target.value })}
              placeholder=""
              style={{ ...inputStyle, maxWidth: '350px' }}
            />
          </div>
        )}

        {form.SaveType === 'file' && (
          <div style={{ marginBottom: '15px' }}>
            <label style={labelStyle}>Attach File :</label>
            <input
              type="file"
              accept=".pdf"
              onChange={(e) => setForm({ ...form, File: e.target.files[0] || null })}
              style={{ fontSize: '13px' }}
            />
          </div>
        )}
      </div>
    </div>
  );
}
