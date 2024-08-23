import React, { useState, useEffect } from 'react';
import './CommentModal.css';
import axios from 'axios';

const CommentModal = ({ isOpen, onClose, postId }) => {
    const [comments, setComments] = useState([]);
    const [newComment, setNewComment] = useState('');

    useEffect(() => {
        if (isOpen) {
            // Fetch comments for the specific post
            axios.get(`${process.env.REACT_APP_API_URL}api/comments/${postId}`)
                .then(response => {
                    if (response.data.success) {
                        setComments(response.data.comments);
                    } else {
                        console.error("Error fetching comments:", response.data.error);
                    }
                })
                .catch(error => {
                    console.error("There was an error fetching the comments!", error);
                });
        }
    }, [isOpen, postId]);

    const handleAddComment = () => {
        // Post the new comment to the server
        axios.post(`${process.env.REACT_APP_API_URL}api/comments`, {
            postId,
            comment: newComment
        })
        .then(response => {
            if (response.data.success) {
                setComments([...comments, response.data.comment]);
                setNewComment('');
            } else {
                console.error("Error adding comment:", response.data.error);
            }
        })
        .catch(error => {
            console.error("There was an error adding the comment!", error);
        });
    };

    if (!isOpen) return null;

    return (
        <div className="comment-modal-overlay">
            <div className="comment-modal">
                <button className="close-button" onClick={onClose}>X</button>
                <h3>Comments</h3>
                <div className="comments-list">
                    {comments.map((comment, index) => (
                        <div key={index} className="comment-item">
                            {comment.text}
                        </div>
                    ))}
                </div>
                <div className="comment-input">
                    <input
                        type="text"
                        value={newComment}
                        onChange={(e) => setNewComment(e.target.value)}
                        placeholder="Add a comment..."
                    />
                    <button onClick={handleAddComment}>Post</button>
                </div>
            </div>
        </div>
    );
};

export default CommentModal;