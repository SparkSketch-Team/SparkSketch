import React from 'react';
import NavBar from './NavBar';
import './Profile.css';
import Avatar from 'react-avatar';



function Profile() {
    return (
        <div className='App'> 
            <NavBar />
            <header className='App-title'>MY PROFILE</header>
            <div className='container'>
                <Avatar name='Aids Shoe' round size='120'/>
                <p className='box'>--- <br></br>Followers<br></br>
                <button className='button'>Edit Profile</button>
                </p>
                <p className='box'>--- <br></br>Sketches<br></br>
                <button className='button'>Settings</button>
                </p>
            </div>
        </div>
    )
}

export default Profile