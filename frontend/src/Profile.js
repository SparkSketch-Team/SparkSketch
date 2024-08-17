import React from 'react';
import NavBar from './NavBar';
import './Profile.css';
import Avatar from 'react-avatar';
import { useNavigate } from 'react-router-dom';


function Profile() {
    const navigate = useNavigate();

    return (
        <div className='App'> 
            <NavBar />
            <header className='title'>MY PROFILE</header>
            <div className='container'>
                <div className='avatar'><Avatar name='Aids Shoe' round size='120'/></div>
                <p className='box'>--- <br></br>Followers<br></br>
                <button className='button' type='button' onClick={() => navigate('/edit')} >Edit Profile</button>
                </p>
                <p className='box'>--- <br></br>Sketches<br></br>
                <button className='button' type='button'>Settings</button>
                </p>
            </div>
        </div>
    )
}

export default Profile