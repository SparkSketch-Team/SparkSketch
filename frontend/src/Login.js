import React from 'react';
import './App.css'
import Navbar from './NavBar'
import Button from 'react-bootstrap/Button';


function Login() {

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
                    <a href='#'>Forgot Password?</a>
                </div>
                <Button variant='warning' type='submit'>Login</Button>
                <div className='Register'>
                    <p>Don't have an account? 
                        <a href='#'>Register</a>
                    </p>
                </div>
            </form>
        </div>
)}

export default Login