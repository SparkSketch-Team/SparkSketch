import React from 'react';
import Container from 'react-bootstrap/Container';
import Nav from 'react-bootstrap/Nav';
import Navbar from 'react-bootstrap/Navbar';
import Avatar from './Avatar.js';
import './Navbar.css';

const logout = () => {
  localStorage.removeItem('token');
  window.location.href = '/login'; 
};


function NavBar() {
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
                  <Avatar/>
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
