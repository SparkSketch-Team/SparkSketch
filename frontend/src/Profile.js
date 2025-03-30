import React, { useState, useEffect } from 'react';
import NavBar from './NavBar';
import './Profile.css';
import { useNavigate } from 'react-router-dom';
import Avatar from './Avatar';

function Profile() {
    const navigate = useNavigate();
    const [userData, setUserData] = useState(null);
    const [error, setError] = useState('');

    useEffect(() => {
        const fetchUserData = async () => {
            try {
                const token = localStorage.getItem('token');
                if (!token) {
                    throw new Error('No token found. Please login.');
                }

                const response = await fetch(`${process.env.REACT_APP_API_URL}api/User/GetSelf`, {
                    method: 'GET',
                    headers: {
                        'Authorization': `Bearer ${token}`,
                        'Content-Type': 'application/json',
                    },
                });

                if (!response.ok) {
                    throw new Error('No Response');
                }

                const data = await response.json();
                if (data.success) {
                    setUserData(data.results);
                } else {
                    throw new Error(data.error || 'Failed to fetch user data');
                }
            } catch (err) {
                setError(err.message);
                // If there's an authentication error, redirect to login
                if (err.message === 'No token found. Please login.') {
                    navigate('/login');
                }
            }
        };

        fetchUserData();
    }, [navigate]);

    if (error) {
        return <div className="error-message">{error}</div>;
    }

    if (!userData) {
        return <div>Loading...</div>;
    }

    return (
        <div className='App'> 
            <NavBar />
            <header className='title'>MY PROFILE</header>
            <hr/>
            <div className='container'>
                <div className='avatar'><Avatar/></div>
                <p className='box'>
                    {userData.followers || 0} <br></br>Followers<br></br>
                    <button className='button' type='button' onClick={() => navigate('/edit')} >Edit Profile</button>
                </p>
                <p className='box'>
                    {userData.sketches || 0} <br></br>Sketches<br></br>
                    <button className='button' type='button'>Settings</button>
                </p>
                <br />
            </div>
            <div className='bio'>Bio: {userData.bio || 'No bio available'}</div>
            <div className='user-info'>
                <p>Username: {userData.userName}</p>
                <p>Email: {userData.emailAddress}</p>
                <p>Name: {userData.firstName} {userData.lastName}</p>
            </div>
        </div>
    );
}

export default Profile;