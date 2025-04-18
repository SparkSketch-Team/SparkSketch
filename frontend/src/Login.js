import React, { useState } from 'react';
import './Login.css';
import Button from 'react-bootstrap/Button';
import { FaUser, FaLock } from 'react-icons/fa';
import { useNavigate } from 'react-router-dom';

function Login() {
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const [errorMessage, setErrorMessage] = useState('');
    const [isLoading, setIsLoading] = useState(false); // State to track loading
    const navigate = useNavigate();

    // Handler functions for buttons
    const handleLogin = async (event) => {
        event.preventDefault();
        setErrorMessage(''); // Reset error message
        setIsLoading(true);  // Set loading to true when request starts
        console.log('Login clicked');

        try {
            const response = await fetch(`${process.env.REACT_APP_API_URL}api/User/login`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({ username, password }),
            });

            setIsLoading(false);  // Set loading to false when request finishes

            if (response.ok) {
                const result = await response.json();
                console.log(result);
                if (result.success === true) {
                    // Store the JWT token
                    localStorage.setItem('token', result.results);
                    console.log('Login successful');
                    navigate('/home');
                } else {
                    console.log('Invalid username or password');
                    setErrorMessage('Invalid username or password');
                }
            } else {
                console.log('Failed to connect to the server');
                setErrorMessage('Failed to connect to the server');
            }
        } catch (error) {
            setIsLoading(false);  // Set loading to false on error
            setErrorMessage('An error occurred during login');
            console.error('Error:', error);
        }
    };

    return (
        <div className='App'>
            <p>
                <a className='create' href='/register'>Create an Account</a>
            </p>
            <div className='App-title'><sup>[beta]</sup> SparkSketch</div>
            <div className='Login-body'>
                <div className='wrapper'>
                    <div className='form-box login'>
                        <form onSubmit={handleLogin}>
                            <div className='input-box'>
                                <input
                                    type='text'
                                    placeholder='Username'
                                    value={username}
                                    onChange={(e) => setUsername(e.target.value)}
                                    required
                                />
                                <FaUser className='Icon' />
                            </div>
                            <div className='input-box'>
                                <input
                                    type='password'
                                    placeholder='Password'
                                    value={password}
                                    onChange={(e) => setPassword(e.target.value)}
                                    required
                                />
                                <FaLock className='Icon' />
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
                            <Button variant='warning' type='submit' disabled={isLoading}>
                                {isLoading ? 'Loading' : 'Login'}
                            </Button>
                            {errorMessage && <p className='error'>{errorMessage}</p>}
                        </form>
                    </div>
                </div>
            </div>
        </div>
    );
}

export default Login;
