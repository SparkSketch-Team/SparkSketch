import React, { useEffect, useState } from 'react';
import axios from 'axios';
//import Post from './Post';
import './ImageFeed.css';
import Avatar from './Avatar.js';
import { FaRegComments } from "react-icons/fa";
import 'animate.css';
import CommentModal from './CommentModal';
import LikeButton from './Like.js';
import { toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';

const ImageFeed = ({ searchTerm }) => {
    const [sketches, setSketches] = useState([]);
    const [isLoading, setIsLoading] = useState(true);
    const [likedPosts, setLikedPosts] = useState({});
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [selectedImageUrl, setSelectedImageUrl] = useState('');
    const [isCommentModalOpen, setIsCommentModalOpen] = useState(false);
    const [selectedPostId, setSelectedPostId] = useState(null);
    const [isProfileModalOpen, setIsProfileModalOpen] = useState(false);
    const [selectedUser, setSelectedUser] = useState(null);

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
        const fetchLikesForAllPosts = async () => {
            try {
                const token = localStorage.getItem('token');
                const likesData = {};

                for (let sketch of sketches) {
                    const response = await axios.get(`${process.env.REACT_APP_API_URL}api/Sketch/getLikes/${sketch.postId}`, {
                        headers: {
                            Authorization: `Bearer ${token}`,
                        }
                    });
                    
                    if (response.data.success && response.data.results.length > 0) {
                    
                        likesData[sketch.postId] = true;
                    } else {
    
                        likesData[sketch.postId] = false;
                    }
                }
    
                setLikedPosts(likesData); 
            } catch (error) {
                console.error("Error fetching likes for posts:", error);
            }
        };
    
        if (sketches.length > 0) {
            fetchLikesForAllPosts();
        }
    }, [sketches]);
    

    
    const handleLikeClick = async (postId, isLiked) => {
        const token = localStorage.getItem('token');
        if (isLiked) {
            try {
                await axios.delete(`${process.env.REACT_APP_API_URL}api/Sketch/removeLike/${postId}`, {
                    headers: {
                        Authorization: `Bearer ${token}`,
                    }
                });
                setLikedPosts(prevState => ({
                    ...prevState,
                    [postId]: false
                }));
            } catch (error) {
                console.error("Error unliking the post:", error);
            }
        } else {
            try {
                await axios.post(`${process.env.REACT_APP_API_URL}api/Sketch/addLike/${postId}`, {}, {
                    headers: {
                        Authorization: `Bearer ${token}`,
                    }
                });
                setLikedPosts(prevState => ({
                    ...prevState,
                    [postId]: true
                }));
            } catch (error) {
                console.error("Error liking the post:", error);
            }
        }
    };

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
    };
    
    const closeProfileModal = () => {
        setIsProfileModalOpen(false);
        if (selectedUser != (null)) {
            setSelectedUser(null);
        }
    };

    const handleProfileClick = async (userId) => {
        try {
            const response = await axios.get(
                `${process.env.REACT_APP_API_URL}api/User/GetUserById`, 
                {
                    headers: {
                        'Authorization': `Bearer ${localStorage.getItem('token')}`
                    },
                    params: {
                        userId: userId  // Send userId as a query parameter
                    }
                }
            );
            if (response.data.success) {
                setSelectedUser(response.data.results); // Set the selected user's data
                setIsProfileModalOpen(true);
                console.log(response.data.results);
            } else {
                console.error("Error fetching user data:", response.data.error);
            }
        } catch (error) {
            console.error("There was an error fetching the user data!", error);
        }
    };

    const addFriend = async () => {
        try {
            const response = await fetch(`${process.env.REACT_APP_API_URL}api/Friend/Add`, {
                method: 'POST',
                headers: {
                    'Authorization': `Bearer ${localStorage.getItem('token')}`,
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({ // Ensure the body is stringified to JSON
                    followedUserId: selectedUser.userId.toString(),
                }),
            });
            if (response.ok) {
                toast.success(`${selectedUser.username} has been added as a friend!`, {
                    position: "top-left"
                });
            } else {
                toast.error('Failed to add friend. Please try again.', {
                    position: "top-left"
                });
            }
        } catch (error) {
            console.error('Error adding friend:', error);
            alert('An error occurred. Please try again.');
        }
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
                        <LikeButton 
                            postId={sketch.postId} 
                            liked={likedPosts[sketch.postId] || false} // Pass down the liked state
                            onLikeClick={handleLikeClick} // Pass down the like handler
                        />
                        <FaRegComments className='comment' type='button' onClick={() => handleCommentClick(sketch.postId)}/>
                        <button className='profile' onClick={() => handleProfileClick(sketch.artistID)}><Avatar className='avatar' userId={sketch.artistID}/></button>
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
            {isProfileModalOpen && selectedUser && (
                <div className="modal">
                    <span className="close" onClick={closeProfileModal}>&times;</span>
                    <div className="modal-content">
                    <div className='container1'>
                        <div className='modalpfp'><Avatar userId={selectedUser.userId}/> {selectedUser.username}</div>
                        <p className='box1'>{selectedUser.followers} <br/>Followers</p><p className='box1'>{selectedUser.sketches} <br/>Sketches<br/>
                        </p><button className='add' onClick={addFriend}>+ Add Friend</button>
                        <br />
                    </div>
                        <div>Bio: {selectedUser.bio}</div>
                    </div>
                </div>
            )}
        </div>
    );
};

export default ImageFeed;
