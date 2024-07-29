import React, { useState } from 'react';
import Button from 'react-bootstrap/Button';
import axios from 'axios';

const FileUpload = () => {
  const [selectedFile, setSelectedFile] = useState(null);

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
      // document.location.href='/link'; // Uncomment this line if you want to redirect the user after a successful upload
    })
    .catch(error => {
      console.error('Error uploading file:', error);
    });
  };

  return (
    <div>
      <p className='App-prompt'>Finished Drawing?</p>
      <input type="file" onChange={onFileChange} />
      <Button variant='secondary' onClick={onFileUpload}>Upload!</Button>
    </div>
  );
};

export default FileUpload;
