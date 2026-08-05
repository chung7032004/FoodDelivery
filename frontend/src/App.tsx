// src/App.tsx
import { AppRoutes } from './routes/AppRoutes';
import { useAuthInit } from '@/hooks/useAuthInit';
import  Spinner  from '@/components/ui/Spinner';

function App() {
  const { isLoading } = useAuthInit();

  if (isLoading) return <Spinner />; // hoặc splash screen

  return <AppRoutes />;
}

export default App;