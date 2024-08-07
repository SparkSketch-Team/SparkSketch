import React, { useState } from 'react';
import './App.css';
import Navbar from './NavBar';
import Button from 'react-bootstrap/Button';
import { FaUser } from "react-icons/fa";
import { FaLock } from "react-icons/fa";

function Login() {
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const [errorMessage, setErrorMessage] = useState('');

    // Handler functions for buttons
    const handleForgotPassword = (event) => {
        event.preventDefault();
        // Logic to handle forgot password
        console.log('Forgot Password clicked');
    };


    const handleLogin = async (event) => {
        event.preventDefault();
        setErrorMessage(''); // Reset error message

        // API call to backend for login
        try {
            const response = await fetch(`${process.env.REACT_APP_API_URL}api/User/login`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({ username, password }),
            });

            if (response.ok) {
                const result = await response.json();
                if (result.success) {
                    console.log('Login successful');
                    // Redirect user or update UI as needed
                } else {
                    setErrorMessage('Invalid username or password');
                }
            } else {
                setErrorMessage('Failed to connect to the server');
            }
        } catch (error) {
            setErrorMessage('An error occurred during login');
            console.error('Error:', error);
        }
    };

    return (
        <div className='App'>
            <Navbar />
            <div className='Login-body'>
            <div className='wrapper'>
            <div className='form-box login'>
            <form onSubmit={handleLogin}>
                <h1 className='App-title'>Login</h1>
                <div className='input-box'>
                    <input
                        type='text'
                        placeholder='Username'
                        value={username}
                        onChange={(e) => setUsername(e.target.value)}
                        required
                    />
                    <FaUser className='Icon'/>
                </div>
                <div className='input-box'>
                    <input
                        type='password'
                        placeholder='Password'
                        value={password}
                        onChange={(e) => setPassword(e.target.value)}
                        required
                    />
                    <FaLock className='Icon'/>
                </div>
                <div className='Remember'>
                    <label>
                        <input type='checkbox' />
                        Remember me
                    </label>
                    <button
                        type='button'
                        onClick={handleForgotPassword}
                        style={{
                            background: 'none',
                            border: 'none',
                            color: 'blue',
                            cursor: 'pointer',
                            textDecoration: 'underline',
                            padding: 0,
                        }}
                    >
                        Forgot Password?
                    </button>
                </div>
                <Button variant='warning' type='submit'>
                    Login
                </Button>
                {errorMessage && <p className='error'>{errorMessage}</p>}
                <div className='Register'>
                    <p>
                        Don't have an account? <a href='/register'>Register</a>
                    </p>
                </div>
            </form>
            </div>
            </div>
            </div>
        </div>
    );
}

export default Login;
