import React, { useEffect, useState } from 'react';
import axios from 'axios';
import './ImageFeed.css';
import Avatar from './Avatar.js';
import { FaHeart } from "react-icons/fa6";
import { FaRegComments } from "react-icons/fa";
import 'animate.css';
import CommentModal from './CommentModal';

const ImageFeed = ({ searchTerm }) => {
    const [sketches, setSketches] = useState([]);
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [selectedImageUrl, setSelectedImageUrl] = useState('');
    const [isLoading, setIsLoading] = useState(true);
    const [isCommentModalOpen, setIsCommentModalOpen] = useState(false);
    const [selectedPostId, setSelectedPostId] = useState(null);
    const [likedPosts, setLikedPosts] = useState({});

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

    useEffect(() => {
        // Fetch the like status for all sketches
        sketches.forEach = (postId) => {
            const token = localStorage.getItem('token');
            axios.get(`${process.env.REACT_APP_API_URL}api/Sketch/getLikes/${postId}`, {},
                {headers: {
                    Authorization: `Bearer ${token}`
                }
        })
                .then(response => {
                    if (response.data.success) {
                        setLikedPosts(prevLikedPosts => ({
                            ...prevLikedPosts,
                            [postId]: response.data.liked  // Assuming response.data.liked is a boolean
                        }));
                    }
                })
                .catch(error => {
                    console.error("There was an error fetching like status!", error);
                });
        };
    }, [sketches]);

    const handleImageClick = (url) => {
        setSelectedImageUrl(url);
        setIsModalOpen(true);
    };

    const closeModal = () => {
        setIsModalOpen(false);
        setSelectedImageUrl('');
    };

    
    const handleLikeClick = (postId) => {
        const liked = likedPosts[postId];
        const token = localStorage.getItem('token');

        if (liked) {
            // Unlike the post
            axios.delete(`${process.env.REACT_APP_API_URL}api/Sketch/removeLike/${postId}`, {},
                {headers: {
                    Authorization: `Bearer ${token}`
                }
            })
                .then(response => {
                    if (response.data.success) {
                        setLikedPosts(prevLikedPosts => ({
                            ...prevLikedPosts,
                            [postId]: false
                        }));
                    }
                })
                .catch(error => {
                    console.error("There was an error unliking the post!", error);
                });
            } else {
                const token = localStorage.getItem('token');
                // Like the post
                axios.post(`${process.env.REACT_APP_API_URL}api/Sketch/addLike/${postId}`, {},
                    {headers: {
                        Authorization: `Bearer ${token}`
                    }
                })
                    .then(response => {
                        if (response.data.success) {
                            setLikedPosts(prevLikedPosts => ({
                                ...prevLikedPosts,
                                [postId]: true
                            }));
                        }
                    })
                    .catch(error => {
                        console.error("There was an error liking the post!", error);
                    });
            }
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
