import React from "react";
import './Avatar.css';
import placeholder from '../src/Blankpfp.jpg'


function Avatar() {
    return (
        <img className='pfp' src={placeholder} alt='pfp'></img>
    )
}

export default Avatar