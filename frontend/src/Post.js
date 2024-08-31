import React, { useState } from 'react';
import axios from 'axios';
//import './Post.css';
import Avatar from './Avatar.js';
import { FaHeart } from "react-icons/fa6";
import { FaRegComments } from "react-icons/fa";
import CommentModal from './CommentModal';

const Post = ({ sketch, liked }) => {
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [selectedImageUrl, setSelectedImageUrl] = useState('');
    const [isCommentModalOpen, setIsCommentModalOpen] = useState(false);
    const [likedPosts, setLikedPosts] = useState(liked);

    const handleImageClick = (url) => {
        setSelectedImageUrl(url);
        setIsModalOpen(true);
    };

    const closeModal = () => {
        setIsModalOpen(false);
        setSelectedImageUrl('');
    };

    const handleLikeClick = async (postId) => {
        const token = localStorage.getItem('token');
        try {
            if (likedPosts) {
                await axios.delete(`${process.env.REACT_APP_API_URL}api/Sketch/removeLike/${postId}`, {}, {
                    headers: { Authorization: `Bearer ${token}` }
                });
                setLikedPosts(false);
            } else {
                await axios.post(`${process.env.REACT_APP_API_URL}api/Sketch/addLike/${postId}`, {}, {
                    headers: { Authorization: `Bearer ${token}` }
                });
                setLikedPosts(true);
            }
        } catch (error) {
            console.error("There was an error updating the like status!", error);
        }
    };

    const handleCommentClick = (postId) => {
        setIsCommentModalOpen(true);
    };

    const closeCommentModal = () => {
        setIsCommentModalOpen(false);
    };

    return (
        <div className="interaction">
            <button className="buttonimg" onClick={() => handleImageClick(sketch.mediaUrl)}>
                <img className="image-item" src={sketch.mediaUrl} alt={`sketch-${sketch.postId}`} id="img" />
            </button>
            <FaHeart
                className={`like ${likedPosts ? 'liked' : ''}`}
                type="button"
                onClick={() => handleLikeClick(sketch.postId)}
            />
            <FaRegComments
                className="comment"
                type="button"
                onClick={() => handleCommentClick(sketch.postId)}
            />
            <button className="profile">
                <Avatar className="avatar" />
            </button>
            <CommentModal
                isOpen={isCommentModalOpen}
                onClose={closeCommentModal}
                postId={sketch.postId}
            />
            {isModalOpen && (
                <div className="modal" onClick={closeModal}>
                    <span className="close">&times;</span>
                    <img className="modal-content" src={selectedImageUrl} alt="Expanded Sketch" />
                </div>
            )}
        </div>
    );
};

export default Post;
