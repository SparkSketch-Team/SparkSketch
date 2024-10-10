import React, { useEffect, useState } from 'react';
import NavBar from './NavBar';
import './Profile.css';
import { useNavigate } from 'react-router-dom';
import Avatar from './Avatar';
import axios from 'axios';

function Profile() {
    const navigate = useNavigate();
    const [userId, setUserId] = useState(null);

  useEffect(() => {
    const fetchUserId = async () => {
      try {
        const token = localStorage.getItem('token'); // Assuming you're storing the token in localStorage
        const response = await axios.get(`${process.env.REACT_APP_API_URL}api/User/GetSelfUserId`, {
          headers: {
            'Authorization': `Bearer ${token}`,
          }
        });
        // Assuming the response has the structure { "results": "fa81cf90-2862-461b-ad8a-2c6999dbf230", "success": true, "error": null }
        if (response.data && response.data.success) {
          setUserId(response.data.results);
        }
      } catch (error) {
        console.error('Error fetching user data:', error);
      }
    };
  
    fetchUserId();
  }, []);

    return (
        <div className='App'> 
            <NavBar />
            <header className='title'>MY PROFILE</header>
            <hr/>
            <div className='container'>
                <div className='avatar'>{userId && <Avatar userId={userId} />}</div>
                <p className='box'>--- <br></br>Followers<br></br>
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