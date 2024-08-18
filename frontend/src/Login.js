import React, { useState } from 'react';
import './Login.css';
import Button from 'react-bootstrap/Button';
import { FaUser } from "react-icons/fa";
import { FaLock } from "react-icons/fa";
import { useNavigate } from 'react-router-dom';


function Login() {
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const [errorMessage, setErrorMessage] = useState('');
    const navigate = useNavigate();

    // Handler functions for buttons


    const handleLogin = async (event) => {
        event.preventDefault();
        setErrorMessage(''); // Reset error message
        console.log('Login clicked');
    
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
                console.log(result);
                if (result.results) {
                    // Store the JWT token
                    localStorage.setItem('token', result.results);
                    console.log('Login successful');
                    // Redirect user or update UI as needed
                    // Example: window.location.href = '/link';
                    navigate('/home')
                } else {
                    console.log("Invalid username or password");
                    setErrorMessage('Invalid username or password');
                }
            } else {
                console.log('Failed to connect to the server');
                setErrorMessage('Failed to connect to the server');
            }
        } catch (error) {
            setErrorMessage('An error occurred during login');
            console.error('Error:', error);
        }
    };
    

    return (
        <div className='App'>
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
                        <input className='check' type='checkbox' />
                        Remember me
                    </label>
                    <p>
                        <a href='forgot_password'>Forgot Password?</a>
                    </p>
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
