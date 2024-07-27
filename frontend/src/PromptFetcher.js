import React, { useState, useEffect } from 'react';
import axios from 'axios';

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
    <div>
      <h1>Data from API</h1>
      <p><strong>Theme:</strong> {data.theme}</p>
      <p><strong>Prompt:</strong> {data.promptText}</p>
    </div>
  );
};

export default PromptFetcher;
