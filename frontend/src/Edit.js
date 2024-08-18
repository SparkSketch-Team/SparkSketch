import React from 'react';
import NavBar from './NavBar';
import './Profile.css';



function Edit() {
    return (
        <div className='App'> 
            <NavBar />
            <header className='title'>Edit Profile</header>
            <hr></hr>
            <div className='container'>
                <p className='box-edit'>Change Picture
                    <br/>
                    <button>Choose Image</button>
                    <br/>
                    <br/>
                    Email
                    <br/>
                    <input type='text'/>
                    <br/>
                    <br/>
                    Username
                    <br/>
                    <input type='text'/>
                    <br/>
                    <br/>
                    Bio
                    <br/>
                    <textarea type='text' className='input3'/>
                    <br/>
                    <br/>
                    <button type='submit'>Save</button></p>
            </div>
        </div>
    )
}

export default Edit