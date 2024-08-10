import React from 'react';
import './Login.css';

function Email () {
    return (
        <div className='App'> 
        <div className='Login-body'>
            <div className='wrapper'>
                <div className='form-box login'>
                    <form action=''>
                        <h1 className='App-title'>Email Verification</h1>
                        <div className='input-box'>
                            <input
                                type='text'
                                placeholder='Enter Username'
                                required
                            />
                        </div>
                        <button type='submit'>Continue</button>
                    </form>
                </div>
            </div>
        </div>
    </div>
)}

export default Email