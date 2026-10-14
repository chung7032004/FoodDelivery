// src/main.tsx
import { createRoot } from 'react-dom/client';
import { BrowserRouter } from 'react-router-dom';
import App from './App';
import './index.css';
import { QueryClientProvider } from '@tanstack/react-query';
import { queryClient } from './services/queryClient';
import { Toaster } from 'sonner';
import { AuthProvider } from './contexts/AuthContext';

createRoot(document.getElementById('root')!).render(
    <BrowserRouter>
      <QueryClientProvider client={queryClient}>
      <AuthProvider>
      <App />
      </AuthProvider>
      <Toaster
        theme="dark"
        position="top-right"
        offset={16}
        toastOptions={{
          style: {
            border: '1px solid #475569',
            background: '#111827',
            color: '#f8fafc',
            boxShadow: '0 18px 48px rgba(0, 0, 0, 0.42)',
          },
          classNames: {
            title: 'text-slate-50 font-semibold',
            description: 'text-slate-300',
            actionButton: 'bg-orange-700 text-white hover:bg-orange-600',
            cancelButton: 'bg-slate-700 text-slate-100',
          },
        }}
      />
      </QueryClientProvider>
    </BrowserRouter>
);