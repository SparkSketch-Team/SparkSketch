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
                    <br/>
                    Email
                    <br/>
                    <br/>
                    Username
                    <br/>
                    <br/>
                    Bio
                    <br/>
                    <br/>
                    <button type='submit'>Save</button></p>
                <p className='box-edit'><button>Choose Image</button>
                    <br/>
                    <br/>
                    <input type='text'/>
                    <br/>
                    <br/>
                    <input type='text'/>
                    <br/>
                    <br/>
                    <textarea type='text' className='input3'/></p>
            </div>
        </div>
    )
}

export default Edit