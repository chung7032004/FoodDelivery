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
        theme='dark'
        position='top-right'
      />
      </QueryClientProvider>
    </BrowserRouter>
);