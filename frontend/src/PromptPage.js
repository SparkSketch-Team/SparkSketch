import React, { useState, useEffect } from 'react';
import axios from 'axios';
import './App.css';

const PromptPage = () => {
  const [data, setData] = useState({});
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [rating, setRating] = useState(null);
  const [showFeedback, setShowFeedback] = useState(false);
  const [feedbackText, setFeedbackText] = useState('');

  console.log("API URL: " + process.env.REACT_APP_API_URL);

  useEffect(() => {
    axios.get(process.env.REACT_APP_API_URL + 'api/Ai/getPrompt')
      .then(response => {
        setData(response.data);
        setLoading(false);
      })
      .catch(error => {
        console.log('Error:', error);
        setError(error);
        setLoading(false);
      });
  }, []);

  const handleRating = (value) => {
    setRating(value);
    setShowFeedback(true);
  };

  const handleFeedbackSubmit = () => {
    const feedbackData = {
      promptId: data.id,
      rating,
      feedbackText
    };

    axios.post(process.env.REACT_APP_API_URL + 'api/Ai/ratePrompt', feedbackData)
      .then(response => {
        console.log('Feedback submitted:', response.data);
        setShowFeedback(false);
        setFeedbackText('');
      })
      .catch(error => {
        console.error('Error submitting feedback:', error);
      });
  };

  if (loading) return <p>Loading...</p>;
  if (error) return <p>Error: {error.message}</p>;

  return (
    <div className="prompt-container">
      <div className="prompt">
        <div><strong>Theme:</strong> {data.theme}</div>
        <div><strong>Prompt:</strong> {data.promptText}</div>
      </div>

      <div className="rating-panel">
        <button className={`thumbs-btn ${rating === 'up' ? 'selected' : ''}`} onClick={() => handleRating('up')}>👍</button>
        <button className={`thumbs-btn ${rating === 'neutral' ? 'selected' : ''}`} onClick={() => handleRating('neutral')}>😐</button>
        <button className={`thumbs-btn ${rating === 'down' ? 'selected' : ''}`} onClick={() => handleRating('down')}>👎</button>
      </div>

      {showFeedback && (
        <div className="feedback-section">
          <textarea 
            placeholder="Optional feedback..." 
            value={feedbackText} 
            onChange={(e) => setFeedbackText(e.target.value)}
          />
          <button onClick={handleFeedbackSubmit}>Submit</button>
        </div>
      )}
    </div>
  );
};

export default PromptPage;
