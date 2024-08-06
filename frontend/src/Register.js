import React from 'react';
import './App.css'
import Button from 'react-bootstrap/Button';


function Register() {


    return (
        <div className='App'>
            <div className='Login-body'>
            <div className='wrapper'>
            <div className='form-box login'>
            <form action=''>
                <h1 className='App-title'>Register</h1>
                <div className='input-box'>
                    <input type='email' placeholder='Email' required />
                </div>
                <div className='input-box'>
                    <input type='text' placeholder='Username' required />
                </div>
                <div className='input-box'>
                    <input type='password' placeholder='Password' required />
                </div>
                <div className='input-box'>
                    <input type='password' placeholder='Repeat Password' required />
                </div>
                <div className='Remember'>
                    <label>
                        <input type='checkbox' />I agree to the terms & conditions
                    </label>
                </div>
                <Button variant='warning' type='submit'>Register</Button>
                <div className='Register'>
                    <p>
                        Already have an account? <a href='/login'>Login</a>
                    </p>
                </div>
            </form>
            </div>
            </div>
            </div>
        </div>
    )
}

export default Register