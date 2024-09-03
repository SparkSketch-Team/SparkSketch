import React from 'react';
import Container from 'react-bootstrap/Container';
import Nav from 'react-bootstrap/Nav';
import Navbar from 'react-bootstrap/Navbar';
import Avatar from './Avatar.js';
import './App.css';

const logout = () => {
  localStorage.removeItem('token');
  window.location.href = '/login'; 
};


function NavBar() {
  return (
    <div>
      <Navbar expand="lg" className="bg-light">
        <Container>
            <Navbar.Brand>Sparksketch [beta]</Navbar.Brand>
            <Navbar.Toggle aria-controls="basic-navbar-nav" />
            <Navbar.Collapse id="basic-navbar-nav">
              <Nav className="me-auto">
                <Nav.Link href="/home">Home</Nav.Link>
                <Nav.Link href="/link">Posts</Nav.Link>
              </Nav>
              <Nav className='ms-auto'>
                <Nav.Link href='/profile' className='pic'>
                  <Avatar/>
                </Nav.Link>
                <Nav.Link className='Logout' onClick={logout}>Log Out</Nav.Link>
              </Nav>
            </Navbar.Collapse>
        </Container>
      </Navbar>
    </div>
  );
}

export default NavBar;
