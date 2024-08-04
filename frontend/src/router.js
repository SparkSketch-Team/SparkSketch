import { createBrowserRouter } from 'react-router-dom';
import App from './App';
import ViewPosts from './ViewPosts';
import ErrorPage from './error-page';
import Login from './Login';


export const router = createBrowserRouter([
    {
      path: "/",
      element: <App />,
      errorElement: <ErrorPage />,
    },
    {
      path: "/link",
      element: <ViewPosts />
    },
    {
      path: "/home",
      element: <App />
    },
    {
      path: "/Login",
      element: <Login />
    }
  ]);

export default router;