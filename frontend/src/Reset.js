import React from 'react';
import './Login.css';

function Reset () {
    return (
        <div className='App'> 
        <div className='Login-body'>
            <div className='wrapper'>
                <div className='form-box login'>
                    <form action=''>
                        <h1 className='App-title'>Reset Password</h1>
                        <div className='input-box'>
                            <input
                                type='text'
                                placeholder='New Password'
                                required
                            />
                        </div>
                        <div className='input-box'>
                            <input
                                type='text'
                                placeholder='Confirm Password'
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

export default Reset