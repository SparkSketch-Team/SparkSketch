import React from 'react';
import './App.css'
import Navbar from './NavBar'
import Button from 'react-bootstrap/Button';


function Login() {

    // Handler functions for buttons
    const handleForgotPassword = (event) => {
        event.preventDefault();
        // Logic to handle forgot password
        console.log('Forgot Password clicked');
    };

    const handleRegister = (event) => {
        event.preventDefault();
        // Logic to handle register
        console.log('Register clicked');
    };

    return (
        <div className='App'>
            <Navbar />
            <form action=''>
                <h1 className='App-title'>Login</h1>
                <div className='input-box'>
                    <input type='text' placeholder='Username' required />
                </div>
                <div className='input-box'>
                    <input type='password' placeholder='Password' required />
                </div>
                <div className='Remember'>
                    <label><input type='checkbox' />Remember me</label>
                    <button
                        type='button'
                        onClick={handleForgotPassword}
                        style={{ background: 'none', border: 'none', color: 'blue', cursor: 'pointer', textDecoration: 'underline', padding: 0 }}
                    >
                        Forgot Password?
                    </button>
                </div>
                <Button variant='warning' type='submit'>Login</Button>
                <div className='Register'>
                    <p>Don't have an account? 
                    <button
                            type='button'
                            onClick={handleRegister}
                            style={{ background: 'none', border: 'none', color: 'blue', cursor: 'pointer', textDecoration: 'underline', padding: 0 }}
                        >
                            Register
                        </button>
                    </p>
                </div>
            </form>
        </div>
)}

export default Login