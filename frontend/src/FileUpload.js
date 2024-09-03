import React, { useState } from 'react';
import Alert from 'react-bootstrap/Alert';
import axios from 'axios';

const FileUpload = () => {
  const [selectedFile, setSelectedFile] = useState(null);
  const [showSnackbar, setShowSnackbar] = useState(false);
  const [errorMessage, setErrorMessage] = useState('');

  const onFileChange = event => {
    setSelectedFile(event.target.files[0]);
  };

  const onFileUpload = () => {
    const formData = new FormData();
    const token = process.env.REACT_APP_MOCK_JWT_TOKEN;
    formData.append('file', selectedFile);

    console.log('Selected file:', selectedFile);
    console.log('FormData:', formData);

    axios.post(process.env.REACT_APP_API_URL + 'api/ImageUpload/upload', formData,
    {
      headers: {
          Authorization: `Bearer ${token}` // Include the JWT token in the request headers
      }
  }
)
    .then(response => {
      console.log('File successfully uploaded:', response.data.results);
      document.location.href='/link'; 
    })
    .catch(error => {
      console.error('Error uploading file:', error);
      setErrorMessage('Error uploading file: ' + error.message);
      setShowSnackbar(true);
    });
  };

  return (
    <div>
      <input type="file" onChange={onFileChange} className='file'/>
      <br></br>
      <button className='App-upload' onClick={onFileUpload}>Upload</button>

      {showSnackbar && (
        <Alert variant="danger" onClose={() => setShowSnackbar(false)} dismissible>
          {errorMessage}
        </Alert>
      )}
    </div>
  );
};

export default FileUpload;
