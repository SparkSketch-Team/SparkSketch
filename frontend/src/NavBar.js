import React, { useEffect, useState } from 'react';
import Container from 'react-bootstrap/Container';
import Nav from 'react-bootstrap/Nav';
import Navbar from 'react-bootstrap/Navbar';
import Avatar from './Avatar.js';
import './Navbar.css';
import axios from 'axios'; // Assuming you're using axios for API calls

const logout = () => {
  localStorage.removeItem('token');
  window.location.href = '/login'; 
};

function NavBar() {
  const [userId, setUserId] = useState(null);

  useEffect(() => {
    const fetchUserId = async () => {
      try {
        const token = localStorage.getItem('token'); // Assuming you're storing the token in localStorage
        const response = await axios.get(`${process.env.REACT_APP_API_URL}api/User/GetSelfUserId`, {
          headers: {
            'Authorization': `Bearer ${token}`,
          }
        });
        // Assuming the response has the structure { "results": "fa81cf90-2862-461b-ad8a-2c6999dbf230", "success": true, "error": null }
        if (response.data && response.data.success) {
          setUserId(response.data.results);
        }
      } catch (error) {
        console.error('Error fetching user data:', error);
      }
    };
  
    fetchUserId();
  }, []);
  

  return (
    <div>
      <Navbar expand="lg" dark bgColor='dark' className='navbar navbar-custom'>
        <Container>
          <Navbar.Brand className='navbar navbar-brand' href="/home"><sup>[beta]</sup> SparkSketch </Navbar.Brand>
          <Navbar.Toggle aria-controls="basic-navbar-nav" />
          <Navbar.Collapse id="basic-navbar-nav">
            <Nav className="me-auto">
              <Nav.Link href="/link" className='navbar navbar-text'>Explore</Nav.Link>
            </Nav>
            <Nav className='ms-auto'>
              <Nav.Link href='/profile' className='pic'>
                {userId && <Avatar userId={userId} className='avatar' />}
              </Nav.Link>
              <Nav.Link className='navbar navbar-text' onClick={logout}>Log Out</Nav.Link>
            </Nav>
          </Navbar.Collapse>
        </Container>
      </Navbar>
    </div>
  );
}

export default NavBar;
