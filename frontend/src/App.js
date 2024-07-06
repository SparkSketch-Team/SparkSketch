// src/App.js
import React from 'react';
import DataFetcher from './PromptFetcher';

function App() {
  return (
    <div className="App">
      <header className="App-header">
        <h1>Welcome to My React App</h1>
        <DataFetcher />
      </header>
    </div>
  );
}

export default App;
