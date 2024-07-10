// src/App.js
import React from 'react';
import PromptFetcher from './PromptFetcher';
import NavBar from './NavBar';

function App() {
  return (
    <><head>
      <title>Sparksketch</title>
      <link href = "App.css" rel = "stylesheet"></link>
      </head><div className="App">
        <header className="App-header">
          <h1>Welcome to My React App</h1>
          <NavBar />
          <PromptFetcher />
        </header>
      </div></>
  );
}

export default App;
