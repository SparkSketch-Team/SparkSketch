// src/App.js
import React from 'react';
import PromptFetcher from './PromptFetcher';
import NavBar from './NavBar';
import './App.css'
import 'bootstrap/dist/css/bootstrap.min.css';
import FileUpload from './FileUpload';

function App() {
  return (
    <div className="App">
      <NavBar  />
        <header className="App-header">
          <h1>Sparksketch Prototype</h1>
          <PromptFetcher />
          <div>
        <FileUpload />
        </div>
        </header>
        
    </div>
  );
}

export default App;
