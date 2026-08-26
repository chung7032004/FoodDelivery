// src/App.tsx
import { useAuth } from './contexts/AuthContext';
import { AppRoutes } from './routes/AppRoutes';
import  Spinner  from '@/components/ui/Spinner';

function App() {
  const { isAuthReady } = useAuth();

  if (!isAuthReady) return <Spinner />; // hoặc splash screen

  return <AppRoutes />;
}

export default App;