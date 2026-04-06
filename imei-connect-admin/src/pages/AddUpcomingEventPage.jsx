import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { createEvent } from '../api/eventService';

const inputStyle = {
  width: '100%', height: '34px', border: '1px solid #ccc',
  borderRadius: '2px', padding: '4px 10px', fontSize: '13px', outline: 'none',
};

const labelStyle = {
  display: 'block', fontWeight: '600', fontSize: '12px', color: '#333',
  marginBottom: '4px', marginTop: '10px',
};

export default function AddUpcomingEventPage() {
  const navigate = useNavigate();
  const [saving, setSaving] = useState(false);
  const [error, setError] = useState('');
  const [imagePreview, setImagePreview] = useState(null);

  const [form, setForm] = useState({
    EventTitle: '',
    EventDesc: '',
    EventDate: '',
    PublishDate: '',
    ExpiryDate: '',
    EventVenue: '',
    RegLink: '',
    EnableRSVP: false,
    AddQuestion: false,
    QuestionType: '2',
    QuestionText: '',
    Option1: '',
    Option2: '',
    RepeatNotification: false,
    EventImage: null,
  });
  const [repeatDates, setRepeatDates] = useState([]);
  const [repeatDate, setRepeatDate] = useState('');

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
    if (!form.EventVenue?.trim()) { alert('Please Enter Event Venue'); return; }
    if (!form.PublishDate) { alert('Please Enter Publish Date'); return; }
    if (!form.ExpiryDate) { alert('Please Enter Expiry Date'); return; }
    if (new Date(form.ExpiryDate) <= new Date(form.PublishDate)) { alert('Expiry Date must be greater than Publish Date'); return; }
    setSaving(true);
    setError('');
    try {
      await createEvent({
        grpID: '31185',
        userID: '13010',
        evntTitle: form.EventTitle,
        evntDesc: form.EventDesc,
        evntDate: form.EventDate,
        publishDate: form.PublishDate,
        expiryDate: form.ExpiryDate,
        eventVenue: form.EventVenue,
        reglink: form.RegLink,
        rsvpEnable: form.EnableRSVP ? '1' : '0',
        questionEnable: form.AddQuestion ? form.QuestionType : '0',
        questionType: form.QuestionType,
        questionText: form.QuestionText,
        option1: form.Option1,
        option2: form.Option2,
        RepeatDateTime: repeatDates.join(','),
        eventImageID: imagePreview || '',
      });
      alert('Event added successfully');
      navigate('/upcoming-events');
    } catch (err) {
      setError(err.response?.data?.message || err.message || 'Save failed');
    } finally { setSaving(false); }
  };

  return (
    <div>
      {/* Title Row */}
      <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '20px' }}>
        <div>
          <span style={{ color: '#1a297d', fontSize: '14px' }}>National Admin - Upcoming Events</span>
          <span style={{ fontSize: '14px', fontWeight: 'bold', color: '#333' }}> - Add Upcoming Events</span>
        </div>
        <div style={{ display: 'flex', gap: '8px' }}>
          <button onClick={handleSubmit} disabled={saving} style={{ display: 'flex', alignItems: 'center', gap: '4px', backgroundColor: '#1a297d', color: '#fff', border: 'none', padding: '7px 16px', borderRadius: '4px', fontSize: '13px', cursor: saving ? 'not-allowed' : 'pointer', opacity: saving ? 0.6 : 1 }}>
            <svg width="12" height="12" fill="currentColor" viewBox="0 0 20 20"><path d="M3 3h11.586l2.707 2.707A1 1 0 0117.586 6H18v11a2 2 0 01-2 2H4a2 2 0 01-2-2V5a2 2 0 012-2zm2 10v4h10v-4H5zm0-6v4h7V7H5z" /></svg>
            Save
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
          <div style={{ flex: '1 1 65%' }}>
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

            {/* Event Date + Publish Date + Expiry Date */}
            <div style={{ display: 'flex', gap: '20px', marginBottom: '8px' }}>
              <div style={{ flex: 1 }}>
                <label style={labelStyle}>Event Date</label>
                <input type="datetime-local" value={form.EventDate} onChange={(e) => setForm({ ...form, EventDate: e.target.value })} style={inputStyle} />
              </div>
              <div style={{ flex: 1 }}>
                <label style={labelStyle}>Publish Date</label>
                <input type="datetime-local" value={form.PublishDate} onChange={(e) => setForm({ ...form, PublishDate: e.target.value })} style={inputStyle} />
              </div>
              <div style={{ flex: 1 }}>
                <label style={labelStyle}>Expiry Date</label>
                <input type="datetime-local" value={form.ExpiryDate} onChange={(e) => setForm({ ...form, ExpiryDate: e.target.value })} style={inputStyle} />
              </div>
            </div>

            {/* Event Venue */}
            <div style={{ marginBottom: '8px' }}>
              <label style={labelStyle}>Event Venue</label>
              <input type="text" value={form.EventVenue} onChange={(e) => setForm({ ...form, EventVenue: e.target.value })} style={inputStyle} />
            </div>

            {/* Registration Link */}
            <div style={{ marginBottom: '8px' }}>
              <label style={labelStyle}>Registration Link</label>
              <input type="text" value={form.RegLink} onChange={(e) => setForm({ ...form, RegLink: e.target.value })} style={inputStyle} />
            </div>

            {/* Checkboxes row + Repeat Notification panel */}
            <div style={{ display: 'flex', marginTop: '12px', gap: '30px', alignItems: 'flex-start' }}>
              {/* Left: Checkboxes */}
              <div>
                <div style={{ display: 'flex', gap: '15px', marginBottom: '6px' }}>
                  <label style={{ display: 'flex', alignItems: 'center', gap: '5px', fontSize: '13px', cursor: 'pointer' }}>
                    <input type="checkbox" checked={form.EnableRSVP} onChange={(e) => setForm({ ...form, EnableRSVP: e.target.checked })} />
                    Enable RSVP
                  </label>
                  <label style={{ display: 'flex', alignItems: 'center', gap: '5px', fontSize: '13px', cursor: 'pointer' }}>
                    <input type="checkbox" checked={form.AddQuestion} onChange={(e) => setForm({ ...form, AddQuestion: e.target.checked })} />
                    Add Question
                  </label>
                </div>
                <label style={{ display: 'flex', alignItems: 'center', gap: '5px', fontSize: '13px', cursor: 'pointer' }}>
                  <input type="checkbox" checked={form.RepeatNotification} onChange={(e) => setForm({ ...form, RepeatNotification: e.target.checked })} />
                  Do you want to Repeat Notification
                </label>
              </div>

              {/* Right: Repeat Notification panel */}
              {form.RepeatNotification && (
                <div style={{ flex: 1, backgroundColor: '#f0f0f0', borderRadius: '4px', overflow: 'hidden' }}>
                  <div style={{ backgroundColor: '#1a297d', color: '#fff', padding: '8px 15px', fontSize: '13px', fontWeight: '600' }}>Repeat Notification :</div>
                  <div style={{ padding: '12px 15px' }}>
                    <div style={{ fontSize: '13px', fontWeight: '600', marginBottom: '6px' }}>Date & Time :</div>
                    <div style={{ display: 'flex', gap: '15px', alignItems: 'center' }}>
                      <input type="datetime-local" value={repeatDate} onChange={(e) => setRepeatDate(e.target.value)} style={{ ...inputStyle, width: 'auto' }} />
                      <button type="button" onClick={() => { if (repeatDate) { setRepeatDates([...repeatDates, repeatDate]); setRepeatDate(''); } }} style={{ backgroundColor: '#1a297d', color: '#fff', border: 'none', padding: '7px 16px', borderRadius: '4px', fontSize: '12px', cursor: 'pointer', whiteSpace: 'nowrap' }}>+ Add To List</button>
                    </div>
                    {repeatDates.length > 0 && (
                      <table style={{ width: '100%', borderCollapse: 'collapse', fontSize: '12px', marginTop: '10px' }}>
                        <thead><tr style={{ backgroundColor: '#1a297d', color: '#fff' }}><th style={{ padding: '6px 10px', textAlign: 'left', fontWeight: 'normal' }}>Date & Time</th><th style={{ padding: '6px 8px', textAlign: 'center', fontWeight: 'normal', width: '50px' }}>Remove</th></tr></thead>
                        <tbody>{repeatDates.map((rd, i) => (<tr key={i} style={{ borderBottom: '1px solid #eee' }}><td style={{ padding: '6px 10px' }}>{new Date(rd).toLocaleString()}</td><td style={{ padding: '6px 8px', textAlign: 'center' }}><button onClick={() => setRepeatDates(repeatDates.filter((_, idx) => idx !== i))} style={{ background: 'none', border: 'none', color: '#f44336', cursor: 'pointer', fontSize: '16px' }}>&times;</button></td></tr>))}</tbody>
                      </table>
                    )}
                  </div>
                </div>
              )}
            </div>
          </div>

          {/* Right side - Image + Expiry Date */}
          <div style={{ flex: '0 0 200px', paddingTop: '10px' }}>
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
            <input type="file" accept="image/jpeg,image/jpg,image/png,image/gif,image/bmp" onChange={handleFileChange} style={{ fontSize: '11px', width: '180px' }} />

          </div>

        </div>
      </div>

      {/* Event Questionaries */}
      {form.AddQuestion && (
        <div style={{ backgroundColor: '#fff', borderRadius: '4px', boxShadow: '0 1px 3px rgba(0,0,0,0.08)', marginTop: '15px', overflow: 'hidden' }}>
          <div style={{ backgroundColor: '#e0e0e0', padding: '10px 20px', fontSize: '13px', fontWeight: '600', color: '#333' }}>Event Questionaries</div>
          <div style={{ padding: '20px 25px' }}>
            <div style={{ display: 'flex', gap: '20px', marginBottom: '15px' }}>
              <label style={{ display: 'flex', alignItems: 'center', gap: '5px', fontSize: '13px', cursor: 'pointer' }}>
                <input type="radio" name="questionTypeAdd" value="2" checked={form.QuestionType === '2'} onChange={() => setForm({ ...form, QuestionType: '2' })} />
                Objective Question
              </label>
              <label style={{ display: 'flex', alignItems: 'center', gap: '5px', fontSize: '13px', cursor: 'pointer' }}>
                <input type="radio" name="questionTypeAdd" value="1" checked={form.QuestionType === '1'} onChange={() => setForm({ ...form, QuestionType: '1' })} />
                Normal Question
              </label>
            </div>
            <div style={{ marginBottom: '12px' }}>
              <label style={labelStyle}>Question Text</label>
              <textarea value={form.QuestionText} onChange={(e) => setForm({ ...form, QuestionText: e.target.value })} rows={3} style={{ ...inputStyle, height: 'auto', resize: 'vertical' }} />
            </div>
            {form.QuestionType === '2' && (
              <>
                <div style={{ marginBottom: '12px' }}>
                  <label style={labelStyle}>First Option</label>
                  <input type="text" value={form.Option1} onChange={(e) => setForm({ ...form, Option1: e.target.value })} style={inputStyle} />
                </div>
                <div style={{ marginBottom: '12px' }}>
                  <label style={labelStyle}>Second Option</label>
                  <input type="text" value={form.Option2} onChange={(e) => setForm({ ...form, Option2: e.target.value })} style={inputStyle} />
                </div>
              </>
            )}
          </div>
        </div>
      )}
    </div>
  );
}
