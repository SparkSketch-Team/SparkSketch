import React, { useState } from 'react';
import axios from 'axios';
import NavBar from './NavBar';
import './Profile.css';

function Edit() {
    const [email, setEmail] = useState('');
    const [username, setUsername] = useState('');
    const [bio, setBio] = useState('');
    const [selectedFile, setSelectedFile] = useState(null);

    const handleSave = async () => {
        const token = localStorage.getItem('token');

        try {
            const formData = new FormData();

            // Append user information
            formData.append('firstName', null); // or ''
            formData.append('lastName', null);  // or ''
            formData.append('username', username);
            formData.append('emailAddress', email);
            formData.append('bio', bio);

            // Append profile picture if selected
            if (selectedFile) {
                formData.append('profilePicture', selectedFile); // 'profilePicture' matches the IFormFile param name
            }

            const response = await axios.post(
                `${process.env.REACT_APP_API_URL}api/User/EditUser`,
                formData,
                {
                    headers: {
                        Authorization: `Bearer ${token}`
                    },
                }
            );

            console.log('Profile updated successfully:', response.data);
            // Optionally handle success (e.g., navigate to another page, show a success message)
        } catch (error) {
            console.error('There was an error updating the profile:', error);
            // Optionally handle error (e.g., show an error message)
        }
    };

    const handleImageChange = (e) => {
        const file = e.target.files[0];
        if (file) {
            setSelectedFile(file);
        }
    };

    return (
        <div className='App'> 
            <NavBar />
            <header className='title'>Edit Profile</header>
            <hr></hr>
            <div className='container'>
                <p className='box-edit'>Change Picture
                    <br/>
                    <input type='file' onChange={handleImageChange} />
                    <br/>
                    {selectedFile && (
                        <img src={URL.createObjectURL(selectedFile)} alt='Profile Preview' width={100} height={100} />
                    )}
                    <br/>
                    Email
                    <br/>
                    <input 
                        type='text' 
                        value={email}
                        onChange={(e) => setEmail(e.target.value)}
                    />
                    <br/>
                    <br/>
                    Username
                    <br/>
                    <input 
                        type='text' 
                        value={username}
                        onChange={(e) => setUsername(e.target.value)}
                    />
                    <br/>
                    <br/>
                    Bio
                    <br/>
                    <textarea 
                        type='text' 
                        className='input3'
                        value={bio}
                        onChange={(e) => setBio(e.target.value)}
                    />
                    <br/>
                    <br/>
                    <button type='submit' onClick={handleSave}>Save</button>
                </p>
            </div>
        </div>
    );
}

export default Edit;
