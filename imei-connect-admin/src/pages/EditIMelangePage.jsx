import { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import LoadingSpinner from '../components/shared/LoadingSpinner';
import { getMerItems, updateMerItem } from '../api/merService';

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

function toDatetimeLocal(dateStr) {
  if (!dateStr) return '';
  const d = new Date(dateStr);
  if (isNaN(d)) return dateStr;
  return `${d.getFullYear()}-${String(d.getMonth() + 1).padStart(2, '0')}-${String(d.getDate()).padStart(2, '0')}T${String(d.getHours()).padStart(2, '0')}:${String(d.getMinutes()).padStart(2, '0')}`;
}

export default function EditIMelangePage() {
  const { id } = useParams();
  const navigate = useNavigate();
  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);
  const [error, setError] = useState('');
  const [existingFileName, setExistingFileName] = useState('');

  const [form, setForm] = useState({
    Title: '',
    PublishDate: '',
    SaveType: 'link',
    Link: '',
    File: null,
    FilePath: '',
  });

  useEffect(() => {
    (async () => {
      try {
        const res = await getMerItems('2026', '2');
        const allItems = res.data?.TBMERListResult?.MERListResult || [];
        const item = allItems.find(m => String(m.MER_ID) === String(id));
        if (item) {
          const hasFile = !!item.File_Path;
          setForm({
            Title: item.Title || '',
            PublishDate: toDatetimeLocal(item.publish_date || ''),
            SaveType: hasFile ? 'file' : 'link',
            Link: item.Link || '',
            File: null,
            FilePath: item.File_Path || '',
          });
          if (hasFile) {
            setExistingFileName((item.File_Path || '').split('/').pop() || '');
          }
        }
      } catch {}
      finally { setLoading(false); }
    })();
  }, [id]);

  const handleSubmit = async () => {
    if (!form.Title || form.Title === '-Select-') { alert('Please Select Title'); return; }
    if (!form.PublishDate.trim()) { alert('Please Enter Publish Date & Time'); return; }
    if (form.SaveType === 'link' && !form.Link.trim()) { alert('Please Enter Link'); return; }

    setSaving(true);
    setError('');
    try {
      await updateMerItem(id, {
        Title: form.Title,
        PublishDate: form.PublishDate,
        Link: form.SaveType === 'link' ? form.Link : '',
        FilePath: form.SaveType === 'file' ? (form.FilePath || form.File?.name || '') : '',
        TransType: '2',
      });
      navigate('/imelange');
    } catch (err) {
      setError(err.response?.data?.message || err.message || 'Update failed');
    } finally { setSaving(false); }
  };

  const handleRemoveFile = () => {
    setExistingFileName('');
    setForm({ ...form, FilePath: '', File: null });
  };

  const handleDownloadFile = () => {
    if (form.FilePath) window.open(form.FilePath, '_blank');
  };

  if (loading) return <LoadingSpinner className="h-screen" />;

  return (
    <div>
      {/* Title Row */}
      <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '20px' }}>
        <div>
          <span style={{ color: '#1a297d', fontSize: '14px' }}>National Admin -</span>
          <span style={{ fontSize: '14px', fontWeight: 'bold', color: '#333' }}> - Edit iMelange</span>
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
            Update
          </button>
          <button
            onClick={() => navigate('/imelange')}
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

        {/* Row 1: Title + Publish Date & Time */}
        <div style={{ display: 'flex', gap: '20px', marginBottom: '15px' }}>
          <div style={{ flex: '0 0 180px' }}>
            <label style={labelStyle}>Title</label>
            <select value={form.Title} onChange={(e) => setForm({ ...form, Title: e.target.value })} style={{ ...inputStyle, backgroundColor: '#fff' }}>
              {titleOptions.map(t => <option key={t} value={t === '-Select-' ? '' : t}>{t}</option>)}
            </select>
          </div>
          <div style={{ flex: '0 0 300px' }}>
            <label style={labelStyle}>Publish Date & Time</label>
            <input type="datetime-local" value={form.PublishDate} onChange={(e) => setForm({ ...form, PublishDate: e.target.value })} style={inputStyle} />
          </div>
        </div>

        {/* Row 2: Radio buttons */}
        <div style={{ marginBottom: '15px', display: 'flex', alignItems: 'center', gap: '25px' }}>
          <label style={{ display: 'flex', alignItems: 'center', gap: '5px', fontSize: '13px', cursor: 'pointer' }}>
            <input type="radio" name="saveType" checked={form.SaveType === 'link'} onChange={() => setForm({ ...form, SaveType: 'link', File: null })} />
            Save Link
          </label>
          <label style={{ display: 'flex', alignItems: 'center', gap: '5px', fontSize: '13px', cursor: 'pointer' }}>
            <input type="radio" name="saveType" checked={form.SaveType === 'file'} onChange={() => setForm({ ...form, SaveType: 'file', Link: '' })} />
            Attach File (only .Pdf file)
          </label>
        </div>

        {/* Row 3: Conditional */}
        {form.SaveType === 'link' && (
          <div style={{ marginBottom: '15px' }}>
            <label style={labelStyle}>Link :</label>
            <input type="text" value={form.Link} onChange={(e) => setForm({ ...form, Link: e.target.value })} style={{ ...inputStyle, maxWidth: '350px' }} />
          </div>
        )}

        {form.SaveType === 'file' && (
          <div style={{ marginBottom: '15px' }}>
            <div style={{ display: 'flex', alignItems: 'center', gap: '8px', flexWrap: 'wrap' }}>
              <label style={{
                display: 'inline-flex', alignItems: 'center', gap: '4px',
                backgroundColor: '#6c757d', color: '#fff', border: 'none',
                padding: '5px 12px', borderRadius: '4px', fontSize: '12px', cursor: 'pointer',
              }}>
                Upload
                <input type="file" accept=".pdf" onChange={(e) => {
                  const file = e.target.files[0];
                  if (file) { setForm({ ...form, File: file, FilePath: file.name }); setExistingFileName(file.name); }
                }} style={{ display: 'none' }} />
              </label>
              <button onClick={handleRemoveFile} style={{
                display: 'inline-flex', alignItems: 'center', gap: '4px',
                backgroundColor: '#28a745', color: '#fff', border: 'none',
                padding: '5px 12px', borderRadius: '4px', fontSize: '12px', cursor: 'pointer',
              }}>Remove</button>
              {existingFileName && (
                <>
                  <button onClick={handleDownloadFile} title="Download" style={{
                    width: '28px', height: '28px', borderRadius: '50%',
                    backgroundColor: '#2196F3', color: '#fff', border: 'none', cursor: 'pointer',
                    display: 'inline-flex', alignItems: 'center', justifyContent: 'center',
                  }}>
                    <svg width="14" height="14" fill="none" stroke="currentColor" viewBox="0 0 24 24" strokeWidth="2.5" strokeLinecap="round" strokeLinejoin="round">
                      <path d="M4 16v1a3 3 0 003 3h10a3 3 0 003-3v-1m-4-4l-4 4m0 0l-4-4m4 4V4" />
                    </svg>
                  </button>
                  <span style={{ fontSize: '13px', color: '#333' }}>{existingFileName}</span>
                </>
              )}
            </div>
          </div>
        )}
      </div>
    </div>
  );
}
