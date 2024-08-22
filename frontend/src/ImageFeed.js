import React, { useEffect, useState } from 'react';
import axios from 'axios';
import './ImageFeed.css';
import Avatar from './Avatar.js';
import { FcLike } from "react-icons/fc";
import { FaRegComments } from "react-icons/fa";

const ImageFeed = () => {
    const [sketches, setSketches] = useState([]);
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [selectedImageUrl, setSelectedImageUrl] = useState('');

    useEffect(() => {
        axios.get(process.env.REACT_APP_API_URL + 'api/ImageUpload/sketches')
            .then(response => {
                if (response.data.success) {
                    setSketches(response.data.results);
                } else {
                    console.error("Error fetching sketches:", response.data.error);
                }
            })
            .catch(error => {
                console.error("There was an error fetching the sketches!", error);
            });
    }, []);

    const handleImageClick = (url) => {
        setSelectedImageUrl(url);
        setIsModalOpen(true);
    };

    const closeModal = () => {
        setIsModalOpen(false);
        setSelectedImageUrl('');
    };

    return (
        <div className="image-feed">
            {sketches.map((sketch) => (
                <div key={sketch.postId} className='interaction'>
                    <button className='buttonimg' onClick={() => handleImageClick(sketch.mediaUrl)}> 
                        <img className="image-item" src={sketch.mediaUrl} alt={`sketch-${sketch.postId}`} id='img'/>
                    </button>
                    <FcLike className='like' type='button'/>
                    <FaRegComments className='comment' type='button'/>
                    <button className='profile'><Avatar className='avatar'/></button>
                </div>
            ))}

            {isModalOpen && (
                <div className="modal" onClick={closeModal}>
                    <span className="close">&times;</span>
                    <img className="modal-content" src={selectedImageUrl} alt="Expanded Sketch"/>
                    <div className="caption">Sketch</div>
                </div>
            )}
        </div>
    );
};

export default ImageFeed;
