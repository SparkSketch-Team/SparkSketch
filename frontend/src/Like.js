import React from 'react';
import { FaHeart } from "react-icons/fa6";
import 'animate.css';
import './ImageFeed.css';

const LikeButton = ({ postId, liked, onLikeClick }) => {
    return (
        <FaHeart 
            className={`like ${liked ? 'liked' : ''}`} 
            type="button" 
            onClick={() => onLikeClick(postId, liked)} // Trigger the like/unlike action
        />
    );
};

export default LikeButton;
