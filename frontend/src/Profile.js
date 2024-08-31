import React from 'react';
import NavBar from './NavBar';
import './Profile.css';
import { useNavigate } from 'react-router-dom';
import Avatar from './Avatar';


function Profile() {
    const navigate = useNavigate();

    return (
        <div className='App'> 
            <NavBar />
            <header className='title'>MY PROFILE</header>
            <hr/>
            <div className='container'>
                <div className='avatar'><Avatar/></div>
                <p className='box'>--- <br></br>Followers<br></br>
                <button className='button' type='button' onClick={() => navigate('/edit')} >Edit Profile</button>
                </p>
                <p className='box'>--- <br></br>Sketches<br></br>
                <button className='button' type='button'>Settings</button>
                </p>
                <br />
            </div>
            <div className='bio'>Bio:</div>
        </div>
    )
}

export default Profile