// src/App.js
import React, { useEffect } from 'react';
import PromptFetcher from './PromptFetcher';
import NavBar from './NavBar';
import './App.css'
import 'bootstrap/dist/css/bootstrap.min.css';
import FileUpload from './FileUpload';
import PromptPage from './PromptPage';



function App() {


  useEffect(() => {
    const apiUrl = process.env.REACT_APP_MOCK_JWT_TOKEN;
    if (apiUrl) {
      localStorage.setItem('token', apiUrl);
    }
  }, []);
  const monthNames = ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'];
  const current = new Date();
  let months = (current.getMonth());
  let month = monthNames[months];
  const date = `${current.getDate()} ${current.getFullYear()}`;

  return (
    <body className="App">
      <nav>
        <NavBar />
      </nav>
      <header className='App-title1'>
        {month} {date}
      </header>
      <div className="App-body">
        <p>
          <PromptFetcher />
        </p>

        <FileUpload />

      </div>

    </body>
  );
}

export default App;

