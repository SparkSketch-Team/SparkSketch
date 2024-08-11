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
                        <p className='Register'>A four digit code has been sent to your email address</p>
                        <div className='input-box'>
                            <input
                                type='text'
                                placeholder='Enter four digit code'
                                required
                            />
                        </div>
                        <button type='submit'>Continue</button>
                    </form>
                    <p className='Register'>Didn't receive an email? <button className='link' >Click here to resend</button></p>
                </div>
            </div>
        </div>
    </div>
)}

export default Email