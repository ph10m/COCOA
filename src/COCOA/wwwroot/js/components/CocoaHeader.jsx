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
        const enrolled = this.props.enrolledCourses.map(course => {
            console.log(course.courseName + ": " + course.courseId);
            return (
                <MenuItem eventKey={"3." + course.courseId} href={"/course/index/" + course.courseId}>{course.courseName}</MenuItem>
                );
        });

        return (
            <Navbar inverse>
                <Navbar.Header>
                    <a href='/home' className='logo-link'>
                        <Image src='/../images/logo.png' className='header-logo' />
                    </a>
                    <Navbar.Brand>
                    </Navbar.Brand>
                    <Navbar.Toggle />
                </Navbar.Header>
                <Navbar.Collapse>
                    <Nav>
                    <NavItem eventKey={"1"} href='/home'>Home</NavItem>
                    <NavItem eventKey={"2"} href='/home'>Tasks</NavItem>
                    <NavDropdown eventKey={"3"} title="Courses" id="basic-nav-dropdown">
                        {enrolled}
                        <MenuItem divider />
                    </NavDropdown>
                    {this.props.signedIn && (<NavItem eventKey={"4"} href='/course/materialsearch'>Document search</NavItem>)}
                    </Nav>
                    <Nav pullRight>
                    {!this.props.signedIn && (<NavItem eventKey={1} href='/user/signin'>Log in</NavItem>)}
                    {!this.props.signedIn && (<NavItem eventKey={2} href='/user/register'>Create new user</NavItem>)}
                    {this.props.signedIn && (<NavItem eventKey={3}>{this.props.userName}</NavItem>)}
                    {this.props.signedIn && (<NavItem eventKey={3} href='/user/signout'>Log out</NavItem>)}
                    {this.props.signedIn && (<a href='/user' className='logo-link'>
                            <Image src='/../images/userIcon.png' className='header-logo' />
                        </a>)}
                    </Nav>
                </Navbar.Collapse>
            </Navbar>
        );
    }
}