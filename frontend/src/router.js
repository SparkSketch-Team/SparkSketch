import { createBrowserRouter } from 'react-router-dom';
import App from './App';
import ViewPosts from './ViewPosts';
import ErrorPage from './error-page';


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
  ]);

export default router;