import React, { useState } from 'react';

const FileUpload = () => {
  const [selectedFile, setSelectedFile] = useState(null);

  const onFileChange = event => {
    setSelectedFile(event.target.files[0]);
  };

  const onFileUpload = () => {
    const formData = new FormData();
    formData.append('file', selectedFile);

    // Replace with your backend endpoint
    fetch(process.env.REACT_APP_API_URL + 'api/sketch/uploadImage', {
      method: 'POST',
      body: formData,
    })
      .then(response => response.json())
      .then(data => {
        console.log('File successfully uploaded:', data);
      })
      .catch(error => {
        console.error('Error uploading file:', error);
      });
  };

  return (
    <div>
      <h2>File Upload</h2>
      <input type="file" onChange={onFileChange} />
      <button onClick={onFileUpload}>Upload!</button>
    </div>
  );
};

export default FileUpload;
