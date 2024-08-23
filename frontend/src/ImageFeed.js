import React, { useEffect, useState } from 'react';
import axios from 'axios';
import './ImageFeed.css';
import Avatar from './Avatar.js';
import { FaHeart } from "react-icons/fa6";
import { FaRegComments } from "react-icons/fa";
import 'animate.css';
import CommentModal from './CommentModal';

const ImageFeed = () => {
    const [sketches, setSketches] = useState([]);
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [selectedImageUrl, setSelectedImageUrl] = useState('');
    const [isCommentModalOpen, setIsCommentModalOpen] = useState(false);
    const [selectedPostId, setSelectedPostId] = useState(null);

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

    const [likedPosts, setLikedPosts] = useState({});
    const handleLikeClick = (postId) => {
        setLikedPosts((prevLikedPosts) => ({
          ...prevLikedPosts,
          [postId]: !prevLikedPosts[postId], // Toggle like status
        }));
      };
    
    const handleCommentClick = (postId) => {
        setSelectedPostId(postId);
        setIsCommentModalOpen(true);
    };

    const closeCommentModal = () => {
        setIsCommentModalOpen(false);
        setSelectedPostId(null);
    };

    return (
        <div className="image-feed">
            {sketches.map((sketch) => (
                <div key={sketch.postId} className='interaction'>
                    <button className='buttonimg' onClick={() => handleImageClick(sketch.mediaUrl)}> 
                        <img className="image-item" src={sketch.mediaUrl} alt={`sketch-${sketch.postId}`} id='img'/>
                    </button>
                    <FaHeart className={`like ${likedPosts[sketch.postId] ? 'liked' : ''}`} type='button' 
                    onClick={() => handleLikeClick(sketch.postId)}/>
                    <FaRegComments className='comment' type='button' onClick={() => handleCommentClick(sketch.postId)}/>
                    <button className='profile'><Avatar className='avatar'/></button>
                </div>
            ))}
            <CommentModal
                isOpen={isCommentModalOpen}
                onClose={closeCommentModal}
                postId={selectedPostId}
            />

            {isModalOpen && (
                <div className="modal" onClick={closeModal}>
                    <span className="close">&times;</span>
                    <img className="modal-content" src={selectedImageUrl} alt="Expanded Sketch"/>
                </div>
            )}
        </div>
    );
};

export default ImageFeed;
