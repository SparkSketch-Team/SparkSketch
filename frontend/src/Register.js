import React, { useState } from 'react';
import './Login.css';
import './Register.css';
import Button from 'react-bootstrap/Button';
import { toast, ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';

function Register() {
    const [formData, setFormData] = useState({
        email: '',
        username: '',
        password: '',
        confirmPassword: '',
        agreedToTerms: false,
    });


    const handleChange = (e) => {
        const { name, value, type, checked } = e.target;
        setFormData({
            ...formData,
            [name]: type === 'checkbox' ? checked : value,
        });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();

        // Basic validation
        if (!formData.agreedToTerms) {
            toast.error('You must agree to the terms & conditions.', {
                position: "top-center"
            });
            return;
        }

        if (formData.password !== formData.confirmPassword) {
            toast.error('Passwords do not match.', {
                position: 'top-center'
            });
            return;
        }

        try {
            const response = await fetch(`${process.env.REACT_APP_API_URL}api/User/AddUser`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    firstName: 'FirstName', // You may want to collect this from the form
                    lastName: 'LastName',   // You may want to collect this from the form
                    emailAddress: formData.email,
                    userName: formData.username,
                    password: formData.password,
                    isActive: true,
                    permissionLevel: 'User',
                }),
            });

            if (response.ok) {
                const data = await response.json();

                if (!data.success) {
                    toast.error(`${data.results}`, {
                        position: 'top-center'
                    });
                    return;
                }

                localStorage.setItem('token', data.results);

                // Reset form and show success message
                setFormData({
                email: '',
                username: '',
                password: '',
                confirmPassword: '',
                agreedToTerms: false,
                });
                toast.success("Registration Successful!", {
                    position: 'top-center'
                });

                // Redirect to prompting page after successful registration
                setTimeout(() => {
                    window.location.href = '/home'; // Change to your prompting page route
                }, 2000);


            } else {
                toast.error('Registration Failed.', {
                    position: 'top-center'
                });
            }
        } catch (err) {
            setFormData({
                password: '',
                confirmPassword: '',
                });
        }
    };

    return (
        <div className='App'>
            <div className='Login-body'>
                <div className='wrapper_register'>
                    <div className='form-box login'>
                        <form onSubmit={handleSubmit}>
                            <h1 className='App-title'>Register</h1>
                            <div className='input-box'>
                                <input
                                    type='email'
                                    name='email'
                                    placeholder='Email'
                                    value={formData.email}
                                    onChange={handleChange}
                                    required
                                />
                            </div>
                            <div className='input-box'>
                                <input
                                    type='text'
                                    name='username'
                                    placeholder='Username'
                                    value={formData.username}
                                    onChange={handleChange}
                                    required
                                />
                            </div>
                            <div className='input-box'>
                                <input
                                    type='password'
                                    name='password'
                                    placeholder='Password'
                                    value={formData.password}
                                    onChange={handleChange}
                                    required
                                />
                            </div>
                            <div className='input-box'>
                                <input
                                    type='password'
                                    name='confirmPassword'
                                    placeholder='Confirm Password'
                                    value={formData.confirmPassword}
                                    onChange={handleChange}
                                    required
                                />
                            </div>
                            <div className='Remember'>
                                <label>
                                    <input
                                        className='check2'
                                        type='checkbox'
                                        name='agreedToTerms'
                                        checked={formData.agreedToTerms}
                                        onChange={handleChange}
                                    />
                                    I agree to the terms & conditions
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
            <ToastContainer />
        </div>
    );
}

export default Register;
