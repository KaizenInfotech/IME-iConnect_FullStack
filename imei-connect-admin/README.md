# IMEI Connect Admin Panel — React Frontend

React 18 + Vite frontend for the IMEI Connect Admin Panel.

## Quick Start

```bash
npm install
npm run dev
```

App runs at: **http://localhost:5173**

## Environment Variables

Set the API URL in `.env.local`:

```
VITE_API_URL=http://localhost:5000/api
```

## Tech Stack

- React 18 with Vite
- React Router v6
- Axios for HTTP requests
- Tailwind CSS for styling

## Project Structure

```
src/
├── api/           ← API service files (one per module)
├── components/
│   ├── layout/    ← Sidebar, Navbar, Layout wrapper
│   └── shared/    ← DataTable, Modal, ConfirmDialog, etc.
├── context/       ← AuthContext for JWT auth state
├── pages/         ← Page components (one per route)
├── routes/        ← AppRouter with all route definitions
├── main.jsx       ← App entry point
└── index.css      ← Tailwind directives + global styles
```
