import React, { useState } from 'react';
import './App.css';
import Navbar from './NavBar';
import Button from 'react-bootstrap/Button';

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

    const handleRegister = (event) => {
        event.preventDefault();
        // Logic to handle register
        console.log('Register clicked');
    };

    const handleLogin = async (event) => {
        event.preventDefault();
        setErrorMessage(''); // Reset error message

        // API call to backend for login
        try {
            const response = await fetch(`${process.env.REACT_APP_API_URL}/login`, {
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
                </div>
                <div className='input-box'>
                    <input
                        type='password'
                        placeholder='Password'
                        value={password}
                        onChange={(e) => setPassword(e.target.value)}
                        required
                    />
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
                        Don't have an account?
                        <button
                            type='button'
                            onClick={handleRegister}
                            style={{
                                background: 'none',
                                border: 'none',
                                color: 'blue',
                                cursor: 'pointer',
                                textDecoration: 'underline',
                                padding: 0,
                            }}
                        >
                            Register
                        </button>
                    </p>
                </div>
            </form>
        </div>
    );
}

export default Login;
