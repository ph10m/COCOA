﻿var Navbar = ReactBootstrap.Navbar;
var NavItem = ReactBootstrap.NavItem;
var Nav = ReactBootstrap.Nav;
var NavDropdown = ReactBootstrap.NavDropdown;
var MenuItem = ReactBootstrap.MenuItem;
var Image = ReactBootstrap.Image;
var Glyphicon = ReactBootstrap.Glyphicon;

class CocoaHeader extends React.Component {
    constructor(props) {
        super(props);
    }
    render() {
        const enrolled = this.props.enrolledCourses.map(course => {
            return (
                <MenuItem eventKey={"3." + course.courseId} href={"/course/index/" + course.courseId}>{course.courseName}</MenuItem>
            );
        });

        const assigned = this.props.assignedCourses.map(course => {
            return (
                <MenuItem eventKey={"3." + course.courseId} href={"/course/index/" + course.courseId}>{course.courseName}</MenuItem>
            );
        });

        return (
            <Navbar inverse staticTop>
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
                    <NavDropdown eventKey={"3"} title="Courses" id="basic-nav-dropdown">
                        {!this.props.signedIn && (<MenuItem>Log in before viewing courses.</MenuItem>)}
                        {enrolled}
                        {this.props.isTeacher && (<MenuItem divider />)}
                        {this.props.isTeacher && (<MenuItem eventKey={"new"} href={"/course/register"}>Create new course</MenuItem>)}
                        {this.props.isTeacher && (<MenuItem divider />)}
                        {assigned}
                    </NavDropdown>
                             {this.props.signedIn && (<NavItem eventKey={"5"} href='/course/enrollment'>Follow a new course</NavItem>)}
</Nav>
                    <Nav pullRight>
                    {!this.props.signedIn && (<NavItem eventKey={1} href='/user/signin'>Log in</NavItem>)}
                    {!this.props.signedIn && (<NavItem eventKey={2} href='/user/register'>Create new user</NavItem>)}
                    {this.props.signedIn && (<NavItem eventKey={3}><Glyphicon glyph="user" />{" " + this.props.userName}</NavItem>)}
                    {this.props.signedIn && (<NavItem eventKey={3} href='/user/signout'>Log out</NavItem>)}
                    </Nav>
                </Navbar.Collapse>
            </Navbar>
        );
    }
}