import { useNavigate } from 'react-router-dom';

export default function PageHeader({ title, subtitle, backUrl, children }) {
  const navigate = useNavigate();

  return (
    <div className="flex flex-wrap items-center justify-between gap-4 mb-6">
      <div className="flex items-center gap-3">
        {backUrl && (
          <button
            onClick={() => navigate(backUrl)}
            className="flex items-center gap-1 text-sm text-primary hover:text-primary-dark transition-colors"
          >
            <svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M15 19l-7-7 7-7" />
            </svg>
            Back
          </button>
        )}
        <div>
          <h1 className="page-title">{title}</h1>
          {subtitle && <p className="text-sm text-gray-500 mt-1">{subtitle}</p>}
        </div>
      </div>
      {children && <div className="flex items-center gap-2">{children}</div>}
    </div>
  );
}
