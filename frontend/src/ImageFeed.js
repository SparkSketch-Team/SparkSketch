import React, { useEffect, useState } from 'react';
import axios from 'axios';
import Post from './Post';
import './ImageFeed.css';
import Avatar from './Avatar.js';
import { FaRegComments } from "react-icons/fa";
import 'animate.css';
import CommentModal from './CommentModal';
import LikeButton from './Like.js';

const ImageFeed = ({ searchTerm }) => {
    const [sketches, setSketches] = useState([]);
    const [isLoading, setIsLoading] = useState(true);
    const [likedPosts, setLikedPosts] = useState({});
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [selectedImageUrl, setSelectedImageUrl] = useState('');
    const [isCommentModalOpen, setIsCommentModalOpen] = useState(false);
    const [selectedPostId, setSelectedPostId] = useState(null);
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
        sketches.forEach((sketch) => {
            const postId = sketch.postId;
            const token = localStorage.getItem('token');
            axios.get(`${process.env.REACT_APP_API_URL}api/Sketch/getLikes/${postId}`, {
                headers: {
                    Authorization: `Bearer ${token}`
                }
            })
            .then(response => {
                if (response.data.success) {
                    setLikedPosts(prevLikedPosts => ({
                        ...prevLikedPosts,
                        [postId]: response.data.liked
                    }));
                }
            })
            .catch(error => {
                console.error("There was an error fetching like status!", error);
            });
        });
    }, [sketches]);

    const handleImageClick = (url) => {
        setSelectedImageUrl(url);
        setIsModalOpen(true);
    };

    const closeModal = () => {
        setIsModalOpen(false);
        setSelectedImageUrl('');
    };

    const handleCommentClick = (postId) => {
        setSelectedPostId(postId);
        setIsCommentModalOpen(true);
    };

    const closeCommentModal = () => {
        setIsCommentModalOpen(false);
        setSelectedPostId(null);
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
                        <LikeButton postId={sketch.postId} liked={likedPosts[sketch.postId]} />
                        <FaRegComments className='comment' type='button' onClick={() => handleCommentClick(sketch.postId)}/>
                        <button className='profile' onClick={() => handleProfileClick(sketch.user)}><Avatar className='avatar'/></button>
                    </div>
                ))
            )}
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
            {isProfileModalOpen && (
                <div className="modal">
                    <span className="close" onClick={closeProfileModal}>&times;</span>
                    <div className="modal-content">
                    <div className='container1'>
                        <div className='modalpfp'><Avatar/> Username</div>
                        <p className='box1'>--- <br></br>Followers</p><p className='box1'>--- <br></br>Sketches<br></br>
                        </p><button className='add'>+ Add Friend</button>
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
