// src/components/ImageFeed.js
import React, { useEffect, useState } from 'react';
import axios from 'axios';
import './ImageFeed.css';
import { FcLike } from "react-icons/fc";
import { FaRegComments } from "react-icons/fa";


const ImageFeed = () => {
     const [images, setImages] = useState([]);
    useEffect(() => {
        axios.get(process.env.REACT_APP_API_URL + 'api/ImageUpload/list')
            .then(response => {
                if (response.data.success) {
                    setImages(response.data.results);
                } else {
                    console.error("Error fetching images:", response.data.error);
                }
            })
            .catch(error => {
                console.error("There was an error fetching the images!", error);
            });
    }, []);

    return (
        <div className="image-feed">
            {images.map((url, index) => (
                <div className='interaction'>
                    <button className='buttonimg'> 
                        <img key={index} className="image-item" src={url} alt={`images-${index}`} id='img'/>
                    </button>
                    <FcLike className='like' type='button'/>
                    <FaRegComments className='comment' type='button'/>
                </div>
            ))}
        </div>
    );
};

export default ImageFeed;
