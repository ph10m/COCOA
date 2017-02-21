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
                      <Navbar.Brand>
                          <div className='logo-div'>
                             <a href='#'><Image src='/../images/logo.png' className='header-logo' responsive /></a>
                          </div>
                      </Navbar.Brand>
                      <Navbar.Toggle />
                    </Navbar.Header>
                    <Navbar.Collapse>
                      <Nav>
                        <NavItem eventKey={1} href="#">A tab</NavItem>
                        <NavItem eventKey={2} href="#">Another tab</NavItem>
                        <NavDropdown eventKey={3} title="Dropdown" id="basic-nav-dropdown">
                          <MenuItem eventKey={3.1}>Action</MenuItem>
                          <MenuItem eventKey={3.2}>Another action</MenuItem>
                          <MenuItem eventKey={3.3}>Something else here</MenuItem>
                          <MenuItem divider />
                          <MenuItem eventKey={3.3}>Separated link</MenuItem>
                        </NavDropdown>
                      </Nav>
                      <Nav pullRight>
                        <NavItem eventKey={1} href="#">Link Right</NavItem>
                        <NavItem eventKey={2} href="#">Link Right</NavItem>
                      </Nav>
                    </Navbar.Collapse>
                </Navbar>
                );
    }
}