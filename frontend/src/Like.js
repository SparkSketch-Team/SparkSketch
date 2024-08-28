import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { FaHeart } from "react-icons/fa6";
import 'animate.css';
import './ImageFeed.css';

const LikeButton = ({ postId }) => {
    const [liked, setLiked] = useState(false);
    const [loading, setLoading] = useState(true); // To handle initial fetch

    useEffect(() => {
        // Fetch the initial like status
        const token = localStorage.getItem('token');
        axios.get(`${process.env.REACT_APP_API_URL}api/Sketch/getLikes/${postId}`, {},
            {headers: {
                Authorization: `Bearer ${token}`,
            },
        })
        .then(response => {
            if (response.data.success) {
                setLiked(response.data.liked); // Assuming response.data.liked is a boolean
            }
        })
        .catch(error => {
            console.error("There was an error fetching the like status!", error);
        })
        .finally(() => {
            setLoading(false);
        });
    }, [postId]);

    const handleLikeClick = () => {
        const token = localStorage.getItem('token');

        if (liked) {
            // Unlike the post
            axios.delete(`${process.env.REACT_APP_API_URL}api/Sketch/removeLike/${postId}`, {},
                {headers: {
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

    if (loading) return null; // Optionally render nothing while loading

    return (
        <FaHeart 
            className={`like ${liked ? 'liked' : ''}`} 
            type="button" 
            onClick={handleLikeClick} 
        />
    );
};

export default LikeButton;
