import { createBrowserRouter } from 'react-router-dom';
import App from './App';
import ViewPosts from './ViewPosts';
import ErrorPage from './error-page';
import Login from './Login';
import Register from './Register';
import Forgot from './Forgot';
import Reset from './Reset';
import Profile from './Profile';

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
      path: "/login",
      element: <Login />
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
      element: <Profile />
    }
  ]);

export default router;