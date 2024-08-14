// src/App.js
import React, {useEffect} from 'react';
import PromptFetcher from './PromptFetcher';
import NavBar from './NavBar';
import './App.css'
import 'bootstrap/dist/css/bootstrap.min.css';
import FileUpload from './FileUpload';


function App() {

  useEffect(() => {
    const apiUrl = process.env.REACT_APP_MOCK_JWT_TOKEN;
    if (apiUrl) {
      localStorage.setItem('token', apiUrl);
    }
  }, []);

  return (
    <body className="App">
      <nav>
      <NavBar  />
      </nav>
      <header className='App-title'>
        SPARKSKETCH
      </header>
        <div className="App-body">
          <p className='App-prompt'>
            Today's Prompt:
          </p>
          <p>
          <PromptFetcher />
          </p>
        <div>
        <FileUpload />
        </div>
        </div>
        
    </body>
  );
}

export default App;

