import React, {useEffect } from 'react';
import NavBar from './NavBar';
import axios from 'axios';
import ImageFeed from './ImageFeed';
import './App.css'

function ViewPosts() {
    useEffect(() => {
        axios.get(process.env.REACT_APP_API_URL + 'api/ImageUpload/list')
            .then(response => {
                  //  setImages(response.data);
             })
            .catch(error => {
                console.error("There was an error fetching the images!", error);
            });
         }, []);
    return(
        <div className='App'>
            <NavBar />
            <h1 className='App-title'>EXPLORE</h1>
            <ImageFeed />
        </div>
    );
 }
export default ViewPosts;