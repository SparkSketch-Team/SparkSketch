import React from 'react';
import NavBar from './NavBar';
import './App.css';
import Avatar from 'react-avatar';



function Profile() {
    return (
        <div className='App'> 
        <NavBar />
        <div>
            <Avatar name='Aids Shoe' round/>
        </div>
        </div>
    )
}

export default Profile