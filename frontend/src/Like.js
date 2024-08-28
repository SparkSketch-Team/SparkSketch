import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { FaHeart } from "react-icons/fa6";

const LikeButton = ({ postId, initialLiked }) => {
    const [liked, setLiked] = useState(initialLiked);

    useEffect(() => {
        setLiked(initialLiked);
    }, [initialLiked]);

    const handleLikeClick = () => {
        const token = localStorage.getItem('token');

        if (liked) {
            // Unlike the post
            axios.delete(`${process.env.REACT_APP_API_URL}api/Sketch/removeLike/${postId}`, {
                headers: {
                    Authorization: `Bearer ${token}`,
                },
            })
            .then(response => {
                if (response.data.success) {
                    setLiked(false);
                }
            })
            .catch(error => {
                console.error("There was an error unliking the post!", error);
            });
        } else {
            // Like the post
            axios.post(`${process.env.REACT_APP_API_URL}api/Sketch/addLike/${postId}`, {}, {
                headers: {
                    Authorization: `Bearer ${token}`,
                },
            })
            .then(response => {
                if (response.data.success) {
                    setLiked(true);
                }
            })
            .catch(error => {
                console.error("There was an error liking the post!", error);
            });
        }
    };

    return (
        <FaHeart 
            className={`like ${liked ? 'liked' : ''}`} 
            type="button" 
            onClick={handleLikeClick} 
        />
    );
};

export default LikeButton;
