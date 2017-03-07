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



    updatePassword() {
        var xhr = new XMLHttpRequest();
        var oldPassword = this.state.oldPassword;
        var password = this.state.password;
        var passwordConfirmed = this.state.passwordConfirmed;
        if (password === passwordConfirmed) {
            console.log("Update password");
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
            <div>
                <div className='container'>
                    <h1>Change password</h1>
                    <form>
                        <FieldGroup id="formControlsOldPassword"
                                    type="password"
                                    label="Old password"
                                    placeholder='Old password'
                                    bsSize='lg'
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
                        <Button onClick={this.updatePassword.bind(this)}>
                            Change password
                        </Button>
                    </form>
                </div>
            </div>
            
        );
    }


}
