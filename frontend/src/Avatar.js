import React, { useState, useEffect } from "react";
import './Avatar.css';
import placeholder from '../src/Blankpfp.jpg';

function Avatar() {
  const [profilePictureUrl, setProfilePictureUrl] = useState(null);

  useEffect(() => {
    // Fetch the profile picture from the backend when the component mounts
    const fetchProfilePicture = async () => {
      try {
        const response = await fetch('/api/User/GetSelfProfilePicture', {
          method: 'GET',
          headers: {
            'Authorization': `Bearer ${localStorage.getItem('token')}` // Assuming the token is stored in localStorage
          }
        });
        const data = await response.json();
        
        if (data.success) {
          setProfilePictureUrl(data.results); // Set profile picture URL
        } else {
          console.error(data.error); // Log any errors from the response
        }
      } catch (error) {
        console.error('Error fetching profile picture:', error);
      }
    };

    fetchProfilePicture();
  }, []); // Empty dependency array ensures this runs once after component mounts

  return (
    <img 
      className='pfp' 
      src={profilePictureUrl ? profilePictureUrl : placeholder} 
      alt='Profile'
    />
  );
}

export default Avatar;
