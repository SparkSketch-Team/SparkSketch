import React, { useState } from 'react';
import Alert from 'react-bootstrap/Alert';
import axios from 'axios';
import DrawingCanvas from './DrawingCanvas';

const FileUpload = () => {
  const [selectedFile, setSelectedFile] = useState(null);
  const [imagePreview, setImagePreview] = useState(null);
  const [showSnackbar, setShowSnackbar] = useState(false);
  const [errorMessage, setErrorMessage] = useState('');
  const [showDrawingCanvas, setShowDrawingCanvas] = useState(false);


  const onFileChange = event => {
    const file = event.target.files[0];
    setSelectedFile(file);

    if (file && file.type.startsWith('image/')) {
      const reader = new FileReader();
      reader.onloadend = () => {
        setImagePreview(reader.result); 
      };
      reader.readAsDataURL(file);
    } else {
      setImagePreview(null);
    }
  };

  const onFileUpload = () => {
    const formData = new FormData();
    formData.append('file', selectedFile);

    console.log('Selected file:', selectedFile);
    console.log('FormData:', formData);

    axios.post(process.env.REACT_APP_API_URL + 'api/ImageUpload/upload', formData,
      {
        headers: {
          Authorization: `Bearer ${localStorage.getItem('token')}` // Include the JWT token in the request headers
        }
      }
    )
      .then(response => {
        console.log('File successfully uploaded:', response.data.results);
        document.location.href = '/link';
      })
      .catch(error => {
        console.error('Error uploading file:', error);
        setErrorMessage('Error uploading file: ' + error.message);
        setShowSnackbar(true);
      });
  };

  return (
    <div>
      <input type="file" onChange={onFileChange} className='file' />
      <br></br>
      {imagePreview && ( // Render the image preview
        <div>
          <img src={imagePreview} alt="Preview" style={{ maxWidth: '100%', maxHeight: '300px' }} />
        </div>
      )}

      <div style={{ display: 'flex', justifyContent: 'center', gap: '10px' }}>
        <button className='App-upload' onClick={onFileUpload}>Upload</button>
        <button className='App-upload' onClick={() => setShowDrawingCanvas(true)}>Open Canvas</button>
      </div>
      {showDrawingCanvas && <DrawingCanvas onClose={() => setShowDrawingCanvas(false)} />}


      {showSnackbar && (
        <Alert variant="danger" onClose={() => setShowSnackbar(false)} dismissible>
          {errorMessage}
        </Alert>
      )}
    </div>
  );
};

export default FileUpload;
