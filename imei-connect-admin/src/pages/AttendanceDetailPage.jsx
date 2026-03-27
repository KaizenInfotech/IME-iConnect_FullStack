import { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import LoadingSpinner from '../components/shared/LoadingSpinner';
import { getAttendanceRecord } from '../api/attendanceService';

function formatDateDMY(dateStr) {
  if (!dateStr) return '';
  const d = new Date(dateStr);
  if (isNaN(d)) return dateStr;
  const dd = String(d.getDate()).padStart(2, '0');
  const mm = String(d.getMonth() + 1).padStart(2, '0');
  const yyyy = d.getFullYear();
  const hh = String(d.getHours()).padStart(2, '0');
  const min = String(d.getMinutes()).padStart(2, '0');
  return `${dd}/${mm}/${yyyy} ${hh}:${min}`;
}

export default function AttendanceDetailPage() {
  const { id } = useParams();
  const navigate = useNavigate();
  const { user } = useAuth();
  const groupName = user?.GrpName || user?.groupName || user?.ClubName || 'Group';

  const [record, setRecord] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    (async () => {
      try {
        const res = await getAttendanceRecord(id);
        setRecord(res.data?.data || res.data);
      } catch { setError('Failed to load attendance record'); }
      finally { setLoading(false); }
    })();
  }, [id]);

  if (loading) return <LoadingSpinner className="h-screen" />;
  if (error) return <div className="bg-red-50 text-red-600 px-4 py-3 rounded-lg">{error}</div>;
  if (!record) return <div className="text-gray-500 text-center py-8">Record not found</div>;

  const meetingName = record.AttendanceName || record.attendanceName || '';
  const meetingDate = formatDateDMY(record.AttendanceDate || record.attendanceDate);
  const description = record.AttendanceDesc || record.attendanceDesc || '-';
  const members = record.Members || record.members || [];
  const visitors = record.Visitors || record.visitors || [];

  const presentCount = members.filter(m => (m.Type || m.type) === 'Present').length;
  const absentCount = members.filter(m => (m.Type || m.type) === 'Absent').length;

  return (
    <div>
      {/* Page Title */}
      <div className="flex items-center justify-between mb-4">
        <h1 className="page-title">{groupName} Attendance Detail</h1>
      </div>

      {/* Meeting Info */}
      <div className="card mb-4">
        <div className="well">
          <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
            <div>
              <label className="form-label" style={{ paddingTop: 0 }}>Meeting Name</label>
              <p className="font-medium text-sm">{meetingName}</p>
            </div>
            <div>
              <label className="form-label" style={{ paddingTop: 0 }}>Date</label>
              <p className="font-medium text-sm">{meetingDate}</p>
            </div>
            <div>
              <label className="form-label" style={{ paddingTop: 0 }}>Description</label>
              <p className="font-medium text-sm">{description}</p>
            </div>
          </div>
        </div>
      </div>

      {/* Members Table */}
      <div className="card p-0 overflow-hidden mb-4">
        <div className="px-4 py-3 bg-gray-50 border-b border-gray-200 flex items-center justify-between">
          <h3 className="text-sm font-semibold">Members ({members.length})</h3>
          <div className="flex items-center gap-3 text-xs">
            <span className="text-green-700">Present: {presentCount}</span>
            <span className="text-red-700">Absent: {absentCount}</span>
          </div>
        </div>
        <table className="w-full text-sm">
          <thead>
            <tr className="table-header">
              <th className="px-4 py-3 text-left font-normal" style={{ width: '5%' }}>Sr.No.</th>
              <th className="px-4 py-3 text-left font-normal" style={{ width: '70%' }}>Member Name</th>
              <th className="px-4 py-3 text-center font-normal" style={{ width: '25%' }}>Status</th>
            </tr>
          </thead>
          <tbody>
            {members.length === 0 ? (
              <tr><td colSpan={3} className="px-4 py-8 text-center text-gray-500">No members recorded.</td></tr>
            ) : (
              members.map((m, i) => {
                const status = m.Type || m.type || 'Unknown';
                const isPresent = status === 'Present';
                return (
                  <tr key={i} className={`border-b border-gray-100 ${i % 2 === 0 ? 'bg-white' : 'bg-gray-50/50'}`}>
                    <td className="px-4 py-3">{i + 1}</td>
                    <td className="px-4 py-3">{m.MemberName || m.memberName || `Member #${m.MemberProfileId || m.memberProfileId || ''}`}</td>
                    <td className="px-4 py-3 text-center">
                      <span className={`px-3 py-1 rounded text-xs font-medium ${isPresent ? 'bg-green-100 text-green-700' : 'bg-red-100 text-red-700'}`}>
                        {status}
                      </span>
                    </td>
                  </tr>
                );
              })
            )}
          </tbody>
        </table>
      </div>

      {/* Visitors Table */}
      <div className="card p-0 overflow-hidden mb-4">
        <div className="px-4 py-3 bg-gray-50 border-b border-gray-200">
          <h3 className="text-sm font-semibold">Visitors ({visitors.length})</h3>
        </div>
        <table className="w-full text-sm">
          <thead>
            <tr className="table-header">
              <th className="px-4 py-3 text-left font-normal" style={{ width: '5%' }}>Sr.No.</th>
              <th className="px-4 py-3 text-left font-normal" style={{ width: '70%' }}>Visitor Name</th>
              <th className="px-4 py-3 text-center font-normal" style={{ width: '25%' }}>Type</th>
            </tr>
          </thead>
          <tbody>
            {visitors.length === 0 ? (
              <tr><td colSpan={3} className="px-4 py-8 text-center text-gray-500">No visitors recorded.</td></tr>
            ) : (
              visitors.map((v, i) => (
                <tr key={i} className={`border-b border-gray-100 ${i % 2 === 0 ? 'bg-white' : 'bg-gray-50/50'}`}>
                  <td className="px-4 py-3">{i + 1}</td>
                  <td className="px-4 py-3">{v.VisitorName || v.visitorName || '-'}</td>
                  <td className="px-4 py-3 text-center">
                    <span className="px-3 py-1 rounded text-xs font-medium bg-blue-100 text-blue-700">
                      {v.Type || v.type || 'Visitor'}
                    </span>
                  </td>
                </tr>
              ))
            )}
          </tbody>
        </table>
      </div>

      {/* Back button */}
      <div className="flex justify-end">
        <button onClick={() => navigate('/attendance')} className="btn-outline text-sm">Back</button>
      </div>
    </div>
  );
}
