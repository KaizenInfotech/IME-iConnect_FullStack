import { useState } from 'react';

export default function DataTable({
  columns,
  data,
  total,
  pageNo = 1,
  pageSize = 10,
  onPageChange,
  onSearch,
  onRowClick,
  searchPlaceholder = 'Search...',
  emptyMessage = 'No records found.',
  loading = false,
  actions,
}) {
  const [searchTerm, setSearchTerm] = useState('');
  const totalPages = Math.ceil((total || data.length) / pageSize);

  const handleSearch = (e) => {
    e.preventDefault();
    if (onSearch) onSearch(searchTerm);
  };

  return (
    <div className="card p-0 overflow-hidden">
      {onSearch && (
        <div className="p-4 border-b border-gray-100">
          <form onSubmit={handleSearch} className="flex gap-2">
            <input
              type="text"
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
              placeholder={searchPlaceholder}
              className="form-input max-w-xs"
            />
            <button type="submit" className="btn-primary text-sm">
              Search
            </button>
          </form>
        </div>
      )}

      <div className="overflow-x-auto">
        <table className="w-full text-sm">
          <thead>
            <tr className="table-header">
              {columns.map((col, i) => (
                <th key={i} className="px-4 py-3 text-left font-normal whitespace-nowrap">
                  {col.header || col.label}
                </th>
              ))}
              {actions && (
                <th className="px-4 py-3 text-center font-normal">Actions</th>
              )}
            </tr>
          </thead>
          <tbody>
            {loading ? (
              <tr>
                <td colSpan={columns.length + (actions ? 1 : 0)} className="px-4 py-8 text-center text-gray-500">
                  <div className="flex items-center justify-center gap-2">
                    <div className="h-5 w-5 animate-spin rounded-full border-2 border-gray-200 border-t-primary" />
                    Loading...
                  </div>
                </td>
              </tr>
            ) : data.length === 0 ? (
              <tr>
                <td colSpan={columns.length + (actions ? 1 : 0)} className="px-4 py-8 text-center text-gray-500">
                  {emptyMessage}
                </td>
              </tr>
            ) : (
              data.map((row, rowIndex) => (
                <tr
                  key={row.id || row.Id || rowIndex}
                  onClick={() => onRowClick && onRowClick(row)}
                  className={`border-b border-gray-100 hover:bg-gray-50 transition-colors ${
                    onRowClick ? 'cursor-pointer' : ''
                  } ${rowIndex % 2 === 0 ? 'bg-white' : 'bg-gray-50/50'}`}
                >
                  {columns.map((col, colIndex) => (
                    <td key={colIndex} className="px-4 py-3">
                      {col.render ? col.render(row) : row[col.accessor || col.key]}
                    </td>
                  ))}
                  {actions && (
                    <td className="px-4 py-3 text-center">
                      <div className="flex items-center justify-center gap-2">
                        {actions(row)}
                      </div>
                    </td>
                  )}
                </tr>
              ))
            )}
          </tbody>
        </table>
      </div>

      {totalPages > 1 && onPageChange && (
        <div className="flex items-center justify-between px-4 py-3 border-t border-gray-100">
          <span className="text-sm text-gray-600">
            Showing {((pageNo - 1) * pageSize) + 1} to {Math.min(pageNo * pageSize, total || data.length)} of {total || data.length}
          </span>
          <div className="flex gap-1">
            <button
              disabled={pageNo <= 1}
              onClick={() => onPageChange(pageNo - 1)}
              className="px-3 py-1 text-sm rounded border border-gray-300 disabled:opacity-50 hover:bg-gray-50"
            >
              Prev
            </button>
            {Array.from({ length: Math.min(totalPages, 5) }, (_, i) => {
              const start = Math.max(1, Math.min(pageNo - 2, totalPages - 4));
              const page = start + i;
              if (page > totalPages) return null;
              return (
                <button
                  key={page}
                  onClick={() => onPageChange(page)}
                  className={`px-3 py-1 text-sm rounded border ${
                    page === pageNo
                      ? 'bg-primary text-white border-primary'
                      : 'border-gray-300 hover:bg-gray-50'
                  }`}
                >
                  {page}
                </button>
              );
            })}
            <button
              disabled={pageNo >= totalPages}
              onClick={() => onPageChange(pageNo + 1)}
              className="px-3 py-1 text-sm rounded border border-gray-300 disabled:opacity-50 hover:bg-gray-50"
            >
              Next
            </button>
          </div>
        </div>
      )}
    </div>
  );
}
