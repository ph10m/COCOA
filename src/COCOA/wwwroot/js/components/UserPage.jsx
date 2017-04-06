var Button = ReactBootstrap.Button;
var FormGroup = ReactBootstrap.FormGroup;
var ControlLabel = ReactBootstrap.ControlLabel;
var FormControl = ReactBootstrap.FormControl;

class FieldGroup extends React.Component {
    constructor(props) {
        super(props);
    }
 
    render() {
        return (
            <FormGroup controlId={this.props.id}>
                <ControlLabel>{this.props.label}</ControlLabel>
                <FormControl {...this.props} />
            </FormGroup>
        );
    }
}

class UserPage extends React.Component {
    constructor(props) {
        super(props);
        this.state = { email: '', name: '', password: '', passwordConfirmed: '' };
    }

    updatePassword(e) {
        e.preventDefault();
        const oldPassword = this.state.oldPassword;
        const password = this.state.password;
        const passwordConfirmed = this.state.passwordConfirmed;
        console.log("Attempting to change pw");
        if (password === passwordConfirmed) {
            console.log("UPDATING PASSWORD!");
            //xhr.open('get', "/user/register?email=" + encodeURIComponent(email) + "&name=" + encodeURIComponent(name) + "&password=" + encodeURIComponent(password) + "&persistent=true", true);
            //xhr.onload = function () {
            //    if (xhr.status == 200) {
            //        console.log("Signed in with " + email + ".");
            //    }
            //    else {
            //        console.log("Failed to sign in, wrong email or password.");
            //    }
            //}.bind(this);
            //xhr.send();
        }
        else {
            console.log("Passwords are not equal.");
        }
    }

    oldPasswordChanged(event) {
        this.setState({ oldPassword: event.target.value });
    }

    passwordChanged(event) {
        this.setState({ password: event.target.value });
    }

    passwordConfirmedChanged(event) {
        this.setState({ passwordConfirmed: event.target.value });
    }

    render() {
        return (
 
            <div className="content">
                <h1>Change password</h1>
                <form onSubmit={this.updatePassword}>
                    <FieldGroup id="formControlsOldPassword"
                        type="password"
                        label="Old password"
                        placeholder='Old password'
                        onChange={this.oldPasswordChanged.bind(this)} />
                    <FieldGroup id="formControlPassword"
                        label="Password"
                        type="password" 
                        placeholder='Password'
                        onChange={this.passwordChanged.bind(this)}/>
                    <FieldGroup id="formConfirmPassword"
                        label="Password"
                        type="password"
                        placeholder='Confirm password'
                        onChange={this.passwordConfirmedChanged.bind(this)} />
                    <Button xs type="submit">
                        Change password
                    </Button>
                </form>
                <div className='set1024'>
                    <h1>Set 1024 user</h1>
                </div>
            </div>
        );
    }
}

var altUserPage = React.createClass({
    handleSubmit: function (e) {
        e.preventDefault();
        console.log("changing pw! xD");
    },
    render: function() {
        return(
            <form classname="PasswordForm" onSubmit={this.handleSubmit}>
                <input type="password"
                       placeholder="Old password"/>
                <input type="password"
                       placeholder="New password"/>
                <input type="password"
                       placeholder="Repeat new password"/>
                <input type="submit" value="post" />
            </form>
        );
    }
});
