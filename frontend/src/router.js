import { createBrowserRouter } from 'react-router-dom';
import App from './App';
import ViewPosts from './ViewPosts';
import ErrorPage from './error-page';
import Login from './Login';
import Register from './Register';
import PrivateRoute from './PrivateRoute';
import Forgot from './Forgot';
import Reset from './Reset';
import Profile from './Profile';
import Edit from './Edit';

export const router = createBrowserRouter([
  {
      path: "/",
      element: <App />,
      errorElement: <ErrorPage />,
  },
  {
      path: "/link",
      element: <PrivateRoute element={<ViewPosts />} />,
  },
  {
      path: "/home",
      element: <PrivateRoute element={<App />} />,
  },
  {
      path: "/login",
      element: <Login />,
  },
  {
      path: "/register",
      element: <Register />
    },
    {
      path: "/forgot_password",
      element: <Forgot />
    },
    {
      path: "/reset_password",
      element: <Reset />
    },
    {
      path: "/profile",
      element: <PrivateRoute element={<Profile />} />,
    },
    {
      path: "/edit",
      element: <PrivateRoute element={<Edit />} />,
    }
  ]);

export default router;