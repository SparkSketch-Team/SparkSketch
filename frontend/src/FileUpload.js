import React, { useState } from 'react';
import Button from 'react-bootstrap/Button';

const FileUpload = () => {
  const [selectedFile, setSelectedFile] = useState(null);

  const onFileChange = event => {
    setSelectedFile(event.target.files[0]);
  };

  const onFileUpload = () => {
    const formData = new FormData();
    formData.append('file', selectedFile);

    // Replace with your backend endpoint
    fetch(process.env.REACT_APP_API_URL + 'api/ImageUpload/upload', {
      method: 'POST',
      body: formData,
    })
      .then(response => response.json())
      .then(data => {
        console.log('File successfully uploaded:', data);
        // document.location.href='/link'; User sent to ViewPosts page when an upload is successful
      })
      .catch(error => {
        console.error('Error uploading file:', error);
      });
  };

  return (
    <div>
      <h2>Finished Drawing?</h2>
      <input type="file" onChange={onFileChange} />
      <Button type='button' class='btn btn-lg btn-warning' onClick={onFileUpload}>Upload!</Button>
    </div>
  );
};

export default FileUpload;
