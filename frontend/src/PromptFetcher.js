import React, { useState, useEffect } from 'react';
import axios from 'axios';
import './App.css'

const PromptFetcher = () => {
  const [data, setData] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
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



  if (loading) return <p>Loading...</p>;
  if (error) return <p>Error: {error.message}</p>;

  return (
    <div className='prompt'>
      <div><strong>Theme:</strong> {data.theme}</div>
      <div><strong>Prompt:</strong> {data.promptText}</div>
    </div>
  );
};

export default PromptFetcher;
