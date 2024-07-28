import React, {useEffect } from 'react';
import NavBar from './NavBar';
import axios from 'axios';

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
            <h1>Test</h1>
        </div>
    );
 }
export default ViewPosts;