import React, { useState } from 'react';
import Button from 'react-bootstrap/Button';
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
    formData.append('file', selectedFile);

    console.log('Selected file:', selectedFile);
    console.log('FormData:', formData);

    axios.post(process.env.REACT_APP_API_URL + 'api/ImageUpload/upload', formData)
    .then(response => {
      console.log('File successfully uploaded:', response.data.results);
      document.location.href='/link'; // Uncomment this line if you want to redirect the user after a successful upload
    })
    .catch(error => {
      console.error('Error uploading file:', error);
      setErrorMessage('Error uploading file: ' + error.message);
      setShowSnackbar(true);
    });
  };

  return (
    <div>
      <p className='App-prompt'>Finished Drawing?</p>
      <input type="file" onChange={onFileChange} />
      <Button variant='secondary' onClick={onFileUpload}>Upload!</Button>

      {showSnackbar && (
        <Alert variant="danger" onClose={() => setShowSnackbar(false)} dismissible>
          {errorMessage}
        </Alert>
      )}
    </div>
  );
};

export default FileUpload;
