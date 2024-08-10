import React from 'react';
import './Login.css';



function Forgot() {
    return (
        <div className='App'> 
            <div className='Login-body'>
                <div className='wrapper'>
                    <div className='form-box login'>
                        <form action=''>
                            <h1 className='App-title'>Forgot Your Password?</h1>
                            <div className='input-box'>
                                <input
                                    type='text'
                                    placeholder='Enter email'
                                    required
                                />
                            </div>
                            <button type='submit'>Continue</button>
                        </form>
                    </div>
                </div>
            </div>
        </div>
    )
}

export default Forgot
