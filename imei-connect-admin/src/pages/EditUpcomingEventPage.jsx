import { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import LoadingSpinner from '../components/shared/LoadingSpinner';
import { getUpcomingEvents, updateEvent } from '../api/eventService';

const inputStyle = {
  width: '100%', height: '34px', border: '1px solid #ccc',
  borderRadius: '2px', padding: '4px 10px', fontSize: '13px', outline: 'none',
};

const labelStyle = {
  display: 'block', fontWeight: '600', fontSize: '12px', color: '#333',
  marginBottom: '4px', marginTop: '10px',
};

function toDatetimeLocal(dateStr) {
  if (!dateStr) return '';
  const d = new Date(dateStr);
  if (isNaN(d)) return dateStr;
  return `${d.getFullYear()}-${String(d.getMonth() + 1).padStart(2, '0')}-${String(d.getDate()).padStart(2, '0')}T${String(d.getHours()).padStart(2, '0')}:${String(d.getMinutes()).padStart(2, '0')}`;
}

export default function EditUpcomingEventPage() {
  const { id } = useParams();
  const navigate = useNavigate();
  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);
  const [error, setError] = useState('');
  const [imagePreview, setImagePreview] = useState(null);
  const [currentRegLink, setCurrentRegLink] = useState('');

  const [form, setForm] = useState({
    EventTitle: '',
    EventDesc: '',
    EventDate: '',
    PublishDate: '',
    ExpiryDate: '',
    EventVenue: '',
    AboutEvent: '',
    RegLink: '',
    EnableRSVP: false,
    AddQuestion: false,
    RepeatNotification: false,
    EventImage: null,
  });

  useEffect(() => {
    (async () => {
      try {
        const res = await getUpcomingEvents('31185', '2', new Date().toISOString().slice(0, 10), 'E');
        const events = res.data?.TBEventListTypeResult?.Result?.Events || [];
        const item = events.find(e => String(e.MemberID) === String(id));
        if (item) {
          setCurrentRegLink(item.RegLink || '');
          setForm({
            EventTitle: item.title || '',
            EventDesc: item.Description || '',
            EventDate: toDatetimeLocal(item.eventDate || ''),
            PublishDate: '',
            ExpiryDate: '',
            EventVenue: item.EventVenue || '',
            AboutEvent: '',
            RegLink: item.RegLink || '',
            EnableRSVP: false,
            AddQuestion: false,
            RepeatNotification: false,
            EventImage: null,
          });
          if (item.eventImg) setImagePreview(item.eventImg);
        }
      } catch {}
      finally { setLoading(false); }
    })();
  }, [id]);

  const handleFileChange = (e) => {
    const file = e.target.files[0];
    if (file) {
      setForm({ ...form, EventImage: file });
      const reader = new FileReader();
      reader.onload = (ev) => setImagePreview(ev.target.result);
      reader.readAsDataURL(file);
    }
  };

  const handleSubmit = async () => {
    if (!form.EventTitle.trim()) { alert('Please Enter Title'); return; }
    if (!form.EventDate) { alert('Please Enter Event Date'); return; }
    if (!form.PublishDate) { alert('Please Enter Publish Date'); return; }
    setSaving(true);
    setError('');
    try {
      await updateEvent(id, {
        EventTitle: form.EventTitle,
        EventDesc: form.EventDesc,
        EventDate: form.EventDate,
        PublishDate: form.PublishDate,
        ExpiryDate: form.ExpiryDate,
        EventVenue: form.EventVenue,
        RegLink: form.RegLink,
        RsvpEnable: form.EnableRSVP ? '1' : '0',
        QuestionEnable: form.AddQuestion ? '1' : '0',
      });
      navigate('/upcoming-events');
    } catch (err) {
      setError(err.response?.data?.message || err.message || 'Update failed');
    } finally { setSaving(false); }
  };

  if (loading) return <LoadingSpinner className="h-screen" />;

  return (
    <div>
      {/* Title Row */}
      <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '20px' }}>
        <div>
          <span style={{ color: '#1a297d', fontSize: '14px' }}>National Admin - Upcoming Events</span>
          <span style={{ fontSize: '14px', fontWeight: 'bold', color: '#333' }}> - Edit Upcoming Events</span>
        </div>
        <div style={{ display: 'flex', gap: '8px' }}>
          <button onClick={handleSubmit} disabled={saving} style={{ display: 'flex', alignItems: 'center', gap: '4px', backgroundColor: '#1a297d', color: '#fff', border: 'none', padding: '7px 16px', borderRadius: '4px', fontSize: '13px', cursor: saving ? 'not-allowed' : 'pointer', opacity: saving ? 0.6 : 1 }}>
            <svg width="12" height="12" fill="currentColor" viewBox="0 0 20 20"><path d="M3 3h11.586l2.707 2.707A1 1 0 0117.586 6H18v11a2 2 0 01-2 2H4a2 2 0 01-2-2V5a2 2 0 012-2zm2 10v4h10v-4H5zm0-6v4h7V7H5z" /></svg>
            Update
          </button>
          <button onClick={() => navigate('/upcoming-events')} style={{ display: 'flex', alignItems: 'center', gap: '6px', backgroundColor: '#1a297d', color: '#fff', border: 'none', padding: '7px 16px', borderRadius: '4px', fontSize: '13px', cursor: 'pointer' }}>
            <svg width="12" height="12" fill="none" stroke="currentColor" viewBox="0 0 24 24" strokeWidth="2.5" strokeLinecap="round" strokeLinejoin="round"><path d="M19 12H5M12 19l-7-7 7-7" /></svg>
            Back
          </button>
        </div>
      </div>

      {error && <div style={{ backgroundColor: '#fef2f2', color: '#dc2626', padding: '10px 16px', borderRadius: '4px', marginBottom: '12px', fontSize: '13px' }}>{error}</div>}

      {/* Form */}
      <div style={{ backgroundColor: '#fff', borderRadius: '4px', padding: '25px 30px', boxShadow: '0 1px 3px rgba(0,0,0,0.08)' }}>
        <div style={{ display: 'flex', gap: '30px' }}>

          {/* Left side */}
          <div style={{ flex: '1 1 60%' }}>
            {/* Title */}
            <div style={{ marginBottom: '8px' }}>
              <label style={labelStyle}>Title</label>
              <input type="text" value={form.EventTitle} onChange={(e) => setForm({ ...form, EventTitle: e.target.value })} style={inputStyle} />
            </div>

            {/* Description */}
            <div style={{ marginBottom: '8px' }}>
              <label style={labelStyle}>Description</label>
              <textarea value={form.EventDesc} onChange={(e) => setForm({ ...form, EventDesc: e.target.value })} rows={3} style={{ ...inputStyle, height: 'auto', resize: 'vertical' }} />
            </div>

            {/* Event Date + Publish Date */}
            <div style={{ display: 'flex', gap: '20px', marginBottom: '8px' }}>
              <div style={{ flex: 1 }}>
                <label style={labelStyle}>Event Date</label>
                <input type="datetime-local" value={form.EventDate} onChange={(e) => setForm({ ...form, EventDate: e.target.value })} style={inputStyle} />
              </div>
              <div style={{ flex: 1 }}>
                <label style={labelStyle}>Publish Date</label>
                <input type="datetime-local" value={form.PublishDate} onChange={(e) => setForm({ ...form, PublishDate: e.target.value })} style={inputStyle} />
              </div>
            </div>

            {/* Event Venue */}
            <div style={{ marginBottom: '8px' }}>
              <label style={labelStyle}>Event Venue</label>
              <input type="text" value={form.EventVenue} onChange={(e) => setForm({ ...form, EventVenue: e.target.value })} style={inputStyle} />
            </div>

            {/* About event */}
            <div style={{ marginBottom: '8px' }}>
              <label style={labelStyle}>About event</label>
              <input type="text" value={form.AboutEvent} onChange={(e) => setForm({ ...form, AboutEvent: e.target.value })} style={inputStyle} />
            </div>

            {/* Registration Link */}
            <div style={{ marginBottom: '8px' }}>
              <label style={labelStyle}>Registration Link (Please copy and paste your URL link)</label>
              {currentRegLink && (
                <div style={{ fontSize: '12px', color: '#1a297d', marginBottom: '4px' }}>
                  Current: <span style={{ fontWeight: '500' }}>{currentRegLink}</span>
                </div>
              )}
              <input type="text" value={form.RegLink} onChange={(e) => setForm({ ...form, RegLink: e.target.value })} style={inputStyle} />
            </div>

            {/* Checkboxes */}
            <div style={{ marginTop: '12px', display: 'flex', flexDirection: 'column', gap: '8px' }}>
              <label style={{ display: 'flex', alignItems: 'center', gap: '6px', fontSize: '13px', cursor: 'pointer' }}>
                <input type="checkbox" checked={form.EnableRSVP} onChange={(e) => setForm({ ...form, EnableRSVP: e.target.checked })} />
                Enable RSVP
              </label>
              <label style={{ display: 'flex', alignItems: 'center', gap: '6px', fontSize: '13px', cursor: 'pointer' }}>
                <input type="checkbox" checked={form.AddQuestion} onChange={(e) => setForm({ ...form, AddQuestion: e.target.checked })} />
                Add Question
              </label>
              <label style={{ display: 'flex', alignItems: 'center', gap: '6px', fontSize: '13px', cursor: 'pointer' }}>
                <input type="checkbox" checked={form.RepeatNotification} onChange={(e) => setForm({ ...form, RepeatNotification: e.target.checked })} />
                Do you want to Repeat Notification
              </label>
            </div>
          </div>

          {/* Right side - Image + Expiry Date */}
          <div style={{ flex: '0 0 200px', paddingTop: '10px' }}>
            <label style={labelStyle}>Image</label>
            <div style={{
              width: '150px', height: '110px', border: '1px solid #ccc',
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
            <input type="file" accept="image/jpeg,image/jpg,image/png,image/gif,image/bmp" onChange={handleFileChange} style={{ fontSize: '11px', width: '180px' }} />

            {/* Expiry Date */}
            <div style={{ marginTop: '15px' }}>
              <label style={labelStyle}>Expiry Date</label>
              <input type="datetime-local" value={form.ExpiryDate} onChange={(e) => setForm({ ...form, ExpiryDate: e.target.value })} style={inputStyle} />
            </div>
          </div>

        </div>
      </div>
    </div>
  );
}
