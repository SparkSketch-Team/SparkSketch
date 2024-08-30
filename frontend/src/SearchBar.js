import React from 'react';
//import './SearchBar.css';

const SearchBar = ({ searchTerm, onSearchChange }) => {
    return (
        <input 
            placeholder='Search Artists' 
            className='search' 
            value={searchTerm}
            onChange={onSearchChange}
        />
    );
};

export default SearchBar;
