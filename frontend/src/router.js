import { createBrowserRouter } from 'react-router-dom';
import App from './App';
import ViewPosts from './ViewPosts';
import ErrorPage from './error-page';
import Login from './Login';
import Register from './Register';
import Forgot from './Forgot';
import Email from './Email';
import Reset from './Reset';

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
    path: "/otp",
    element: <Email />
    },
    {
      path: "/reset_password",
      element: <Reset />
    }
  ]);

export default router;