// src/App.js
import React from 'react';
import PromptFetcher from './PromptFetcher';
import NavBar from './NavBar';
import './App.css'
import 'bootstrap/dist/css/bootstrap.min.css';
import FileUpload from './FileUpload';


function App() {
  return (
    <body className="App">
      <nav>
      <NavBar  />
      </nav>
        <header className="App-body">
          <h1>
            Today's Prompt:
          </h1>
          <p>
          <PromptFetcher />
          </p>
        <div>
        <FileUpload />
        </div>
        </header>
        
    </body>
  );
}

export default App;

