import React, { useEffect, useState } from 'react';
import axios from 'axios';
import Post from './Post';
import './ImageFeed.css';
//import CommentModal from './CommentModal';

const ImageFeed = ({ searchTerm }) => {
    const [sketches, setSketches] = useState([]);
    const [isLoading, setIsLoading] = useState(true);
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
                            [postId]: response.data.liked
                        }));
                    }
                })
                .catch(error => {
                    console.error("There was an error fetching like status!", error);
                });
        };
    }, [sketches]);

    return (
        <div className="image-feed">
            {isLoading ? (
                <p>Loading...</p>
            ) : sketches.length === 0 ? (
                <p>No Sketches Found</p>
            ) : (
                sketches.map((sketch) => (
                    <Post key={sketch.postId} sketch={sketch} liked={likedPosts[sketch.postId]} />
                ))
            )}
        </div>
    );
};

export default ImageFeed;
