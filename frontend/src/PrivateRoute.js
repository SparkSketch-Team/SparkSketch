import React from 'react';
import { Navigate } from 'react-router-dom';

const PrivateRoute = ({ element }) => {
    const token = localStorage.getItem('token');
    console.log('Token:', token); // Check if the token is being retrieved correctly

    if (!token) {
        console.log('User not authenticated, redirecting to /login');
        return <Navigate to="/login" />;
    }

    console.log('User authenticated, rendering element');
    return element;
};

export default PrivateRoute;
