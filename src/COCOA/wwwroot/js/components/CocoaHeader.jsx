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
        return (<Navbar inverse collapseOnSelect fluid>
                    <Navbar.Header>
                        <a href='#' className='logo-link'>
                        <Image src='/../images/logo.png' className='header-logo' />
                        </a>

                      <Navbar.Brand>
                      </Navbar.Brand>
                      <Navbar.Toggle />
                    </Navbar.Header>
                    <Navbar.Collapse>
                      <Nav>
                        <NavItem eventKey={1} href="/Home">A tab</NavItem>
                        <NavItem eventKey={2} href="/Home">Another tab</NavItem>
                        <NavDropdown eventKey={3} title="Dropdown" id="basic-nav-dropdown">
                          <MenuItem eventKey={3.1}>Action</MenuItem>
                          <MenuItem eventKey={3.2}>Another action</MenuItem>
                          <MenuItem eventKey={3.3}>Something else here</MenuItem>
                          <MenuItem divider />
                          <MenuItem eventKey={3.3}>Separated link</MenuItem>
                        </NavDropdown>
                      </Nav>
                      <Nav pullRight>
                        <NavItem eventKey={1} href="/Login">Log in</NavItem>
                        <NavItem eventKey={2} href="/Register">Create new user</NavItem>
                      </Nav>
                    </Navbar.Collapse>
                </Navbar>
                );
    }
}