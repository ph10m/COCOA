var Navbar = ReactBootstrap.Navbar;
var NavItem = ReactBootstrap.NavItem;
var Nav = ReactBootstrap.Nav;
var NavDropdown = ReactBootstrap.NavDropdown;
var MenuItem = ReactBootstrap.MenuItem;
var Image = ReactBootstrap.Image;

class CocoaHeader extends React.Component {
    constructor(props) {
        super(props);
    }
    render() {
        return (<Navbar inverse fluid>
                    <Navbar.Header>
                        <a href='/Home' className='logo-link'>
                        <Image src='/../images/logo.png' className='header-logo' />
                        </a>

                      <Navbar.Brand>
                      </Navbar.Brand>
                      <Navbar.Toggle />
                    </Navbar.Header>
                    <Navbar.Collapse>
                      <Nav>
                        <NavItem eventKey={1} href='/Home'>Home</NavItem>
                        <NavItem eventKey={2} href='/Home'>Tasks</NavItem>
                        <NavDropdown eventKey={3} title="Courses" id="basic-nav-dropdown">
                          <MenuItem eventKey={3.1}>TDT4140</MenuItem>
                          <MenuItem eventKey={3.2}>TDT4145</MenuItem>
                          <MenuItem eventKey={3.3}>TDT4170</MenuItem>
                        </NavDropdown>
                      </Nav>
                      <Nav pullRight>
                        <NavItem eventKey={1} href='/Login'>Log in</NavItem>
                        <NavItem eventKey={2} href='/Register'>Create new user</NavItem>
                        <a href='/Home' className='logo-link'>
                        <Image src='/../images/userIcon.png' className='header-logo' />
                        </a>
                      </Nav>
                    </Navbar.Collapse>
                </Navbar>
                );
    }
}