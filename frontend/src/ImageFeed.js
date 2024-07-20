// src/components/ImageFeed.js
import React, { useEffect, useState } from 'react';
import axios from 'axios';

const ImageFeed = () => {
    const [images, setImages] = useState([]);

    useEffect(() => {
        axios.get(REACT_API_URL + '/api/images')
            .then(response => {
                setImages(response.data);
            })
            .catch(error => {
                console.error("There was an error fetching the images!", error);
            });
    }, []);

    return (
        <div>
            {images.map((url, index) => (
                <img key={index} src={url} alt={`image-${index}`} />
            ))}
        </div>
    );
};

export default ImageFeed;
