import React, { useEffect, useState } from 'react';
import axios from 'axios';
import './ImageFeed.css';
import Avatar from './Avatar.js';
import { FcLike } from "react-icons/fc";
import { FaRegComments } from "react-icons/fa";

const ImageFeed = ({ searchTerm }) => {
    const [sketches, setSketches] = useState([]);
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [selectedImageUrl, setSelectedImageUrl] = useState('');
    const [isLoading, setIsLoading] = useState(true);
    const [isProfileModalOpen, setIsProfileModalOpen] = useState(false);

    useEffect(() => {
        const fetchSketches = async () => {
            try {
                const response = await axios.get(process.env.REACT_APP_API_URL + 'api/Sketch/sketches', {
                    params: { username: searchTerm }
                });
                if (response.data.success) {
                    setSketches(response.data.results);
                } else {
                    console.error("Error fetching sketches:", response.data.error);
                    setSketches([]);
                }
            } catch (error) {
                console.error("There was an error fetching the sketches!", error);
            } finally {
                setIsLoading(false);
            }
        };

        fetchSketches();
    }, [searchTerm]);

    const handleImageClick = (url) => {
        setSelectedImageUrl(url);
        setIsModalOpen(true);
    };

    const closeModal = () => {
        setIsModalOpen(false);
        setSelectedImageUrl('');
    };

    const handleProfileClick = (profile) => {
        setIsProfileModalOpen(true);
    };

    const closeProfileModal = () => {
        setIsProfileModalOpen(false);
    };

    return (
        <div className="image-feed">
            {isLoading ? (
                <p>Loading...</p>
            ) : sketches.length === 0 ? (
                <p>No Sketches Found</p>
            ) : (
                sketches.map((sketch) => (
                    <div key={sketch.postId} className='interaction'>
                        <button className='buttonimg' onClick={() => handleImageClick(sketch.mediaUrl)}> 
                            <img className="image-item" src={sketch.mediaUrl} alt={`sketch-${sketch.postId}`} id='img'/>
                        </button>
                        <FcLike className='like' type='button'/>
                        <FaRegComments className='comment' type='button'/>
                        <button className='profile' onClick={() => handleProfileClick(sketch.user)}><Avatar className='avatar'/></button>
                    </div>
                ))
            )}

            {isModalOpen && (
                <div className="modal" onClick={closeModal}>
                    <span className="close">&times;</span>
                    <img className="modal-content" src={selectedImageUrl} alt="Expanded Sketch"/>
                    <div className="caption">Sketch</div>
                </div>
            )}
            {isProfileModalOpen && (
                <div className="modal">
                    <span className="close" onClick={closeProfileModal}>&times;</span>
                    <div className="modal-content">
                    <div className='container'>
                        <div className='modalpfp'><Avatar/> Username</div>
                        <p className='box1'>--- <br></br>Followers</p><p className='box1'>--- <br></br>Sketches<br></br>
                        </p>
                        <br />
                    </div>
                        <div>Bio:</div>
                    </div>
                </div>
            )}
        </div>
    );
};

export default ImageFeed;
