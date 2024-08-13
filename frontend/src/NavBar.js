import React from 'react';
import Container from 'react-bootstrap/Container';
import Nav from 'react-bootstrap/Nav';
import Navbar from 'react-bootstrap/Navbar';
import NavDropdown from 'react-bootstrap/NavDropdown';
import Avatar from 'react-avatar';




function BasicExample() {
  return (
    <div>
    <Navbar expand="lg" className="bg-light">
      <Container>
        <Navbar.Brand href="#home">Sparksketch</Navbar.Brand>
        <Navbar.Toggle aria-controls="basic-navbar-nav" />
        <Navbar.Collapse id="basic-navbar-nav">
          <Nav className="me-auto">
            <Nav.Link href="/home">Home</Nav.Link>
            <Nav.Link href="/link">Posts</Nav.Link>
            <NavDropdown title="Dropdown" id="basic-nav-dropdown">
              <NavDropdown.Item href="/login">Login</NavDropdown.Item>
              <NavDropdown.Item href="#action/3.2">
                Another action
              </NavDropdown.Item>
              <NavDropdown.Item href="#action/3.3">Something</NavDropdown.Item>
              <NavDropdown.Divider />
              <NavDropdown.Item href="#action/3.4">
                Separated link
              </NavDropdown.Item>
            </NavDropdown>
          </Nav>
          <Nav className='ms-auto'>
          <Nav.Link href='/profile'>
          <Avatar name='Aids Shoe' size='35' round/>
          </Nav.Link>
          </Nav>
        </Navbar.Collapse>
      </Container>
    </Navbar>
    </div>
  );
}

export default BasicExample;
