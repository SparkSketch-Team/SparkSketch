import React, { useState, useEffect } from 'react';
import axios from 'axios';
import './App.css'

const PromptFetcher = () => {
  const [data, setData] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [rating, setRating] = useState(null);
  const [showCommentBox, setShowCommentBox] = useState(false);
  const [comment, setComment] = useState('');
  const [commentSubmitted, setCommentSubmitted] = useState(false);
  const [showCommentButton, setShowCommentButton] = useState(false);
  const [commentButtonTriggered, setCommentButtonTriggered] = useState(false);
  const [ratingId, setRatingId] = useState(null);

console.log("API URL: "  + process.env.REACT_APP_API_URL);


useEffect(() => {
  axios.get(process.env.REACT_APP_API_URL + 'api/Ai/getPrompt')
    .then(response => {
      setData(response.data.results);
      setLoading(false);
    })
    .catch(error => {
      console.log('Error:', error);
      setError(error);
      setLoading(false);
    });
}, []);

const handleRating = (selectedRating) => {
  setRating(selectedRating);

  const ratingPayload = {
    Id: ratingId, // Null if new rating
    PromptId: data.id, 
    RatingValue: selectedRating === 'up' ? "Good" : selectedRating === 'neutral' ? "Neutral" : "Bad",
    Comment: comment || "" // Ensure comment is not undefined
  };

  axios.post(process.env.REACT_APP_API_URL + 'api/Ai/UpdatePromptRating', ratingPayload, {
    headers: { "Content-Type": "application/json" } // Ensure JSON format
  })
    .then(response => {
      if (response.data.success) {
        console.log("Rating submitted:", response.data);
        
        if (!ratingId) {
          setRatingId(response.data.results.id); // Store the new rating ID
        }
      }
    })
    .catch(error => {
      console.error("Error submitting rating:", error.response);
    });

  if (!commentButtonTriggered) {
    setShowCommentButton(true);
    setCommentButtonTriggered(true);
  }
};


const handleCommentClick = () => {
  setShowCommentBox(true);
  setShowCommentButton(false); // Remove the Leave a Comment button once clicked
};

const handleCommentSubmit = () => {
  setCommentSubmitted(true);
  setShowCommentBox(false);
  setShowCommentButton(true);

  console.log(`User submitted comment: ${comment}`);

  // Update the rating with the comment
  axios.post(process.env.REACT_APP_API_URL + 'api/Ai/UpdatePromptRating', {
    Id: ratingId,  // Ensure we use the stored rating ID
    PromptId: data.id,
    RatingValue: rating, // Keep the previous rating value
    Comment: comment
  })
    .then(response => {
      if (response.data.success) {
        console.log("Comment added:", response.data);
      }
    })
    .catch(error => {
      console.error("Error updating comment:", error);
    });
};


  if (loading) return <p>Loading...</p>;
  if (error) return <p>Error: {error.message}</p>;

  return (
    <div className='prompt-container'>
      <div className='prompt'>
        <div><strong></strong> {data.promptText}</div>
      </div>

      {/* Rating Buttons */}
      <div className='rating-container'>
        <button
          className={`rating-btn ${rating === 'up' ? 'selected' : ''}`}
          onClick={() => handleRating('up')}
        >
          👍
        </button>
        <button
          className={`rating-btn ${rating === 'neutral' ? 'selected' : ''}`}
          onClick={() => handleRating('neutral')}
        >
          😐
        </button>
        <button
          className={`rating-btn ${rating === 'down' ? 'selected' : ''}`}
          onClick={() => handleRating('down')}
        >
          👎
        </button>
      </div>

      {/* Leave a Comment Button (Appears only once) */}
      {showCommentButton && (
        <div className='comment-section'>
          <button
            className={`comment-btn ${commentSubmitted ? 'disabled' : ''}`}
            onClick={commentSubmitted ? null : handleCommentClick}
            disabled={commentSubmitted}
          >
            {commentSubmitted ? 'Thanks for your feedback' : 'Leave a Comment'}
          </button>
        </div>
      )}

      {/* Comment Box (Appears after clicking Leave a Comment) */}
      {showCommentBox && (
        <div className='comment-box'>
          <textarea
            placeholder="Enter your comment..."
            value={comment}
            onChange={(e) => setComment(e.target.value)}
          />
          <button onClick={handleCommentSubmit}>Submit</button>
        </div>
      )}
    </div>
  );
};

export default PromptFetcher;
