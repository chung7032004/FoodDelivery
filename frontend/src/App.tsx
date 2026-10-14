// src/App.tsx
import { useAuth } from './contexts/AuthContext';
import { AppRoutes } from './routes/AppRoutes';
import  Spinner  from '@/components/ui/Spinner';
import { NotificationRealtime } from '@/components/notifications/NotificationRealtime';

function App() {
  const { isAuthReady } = useAuth();

  if (!isAuthReady) return <Spinner />; // hoặc splash screen

  return (
    <>
      <NotificationRealtime />
      <AppRoutes />
    </>
  );
}

export default App;