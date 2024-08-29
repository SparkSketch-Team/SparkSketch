import React, { useState } from 'react';
import NavBar from './NavBar';
import ImageFeed from './ImageFeed';
import './App.css'

function ViewPosts() {
    const [searchTerm, setSearchTerm] = useState('');

    const handleSearchChange = (event) => {
        setSearchTerm(event.target.value);
    };

    return(
        <div className='App'>
            <NavBar />
            <h1 className='App-title'>EXPLORE</h1>
            <input 
                placeholder='Search Artists' 
                className='search' 
                value={searchTerm}
                onChange={handleSearchChange}
            />
            <ImageFeed searchTerm={searchTerm} />
        </div>
    );
 }
export default ViewPosts;
