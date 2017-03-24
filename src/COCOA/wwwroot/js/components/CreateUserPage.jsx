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

class CreateUserPage extends React.Component {
    constructor(props) {
        super(props);
        this.state = { email: "", name: "", password: "", passwordConfirmed: "", errorText: null };
    }



    createNewUser() {
        var xhr = new XMLHttpRequest();
        var email = this.state.email;
        var name = this.state.name;
        var password = this.state.password;
        var passwordConfirmed = this.state.passwordConfirmed;
        if (password === passwordConfirmed) {
            xhr.open('get', "/user/registeruser?email=" + encodeURIComponent(email) + "&name=" + encodeURIComponent(name) + "&password=" + encodeURIComponent(password) + "&persistent=true", true);
            xhr.onload = function () {
                if (xhr.status == 200) {
                    console.log("Signed in with " + email + ".");
                    this.setState({ errorText: null });
                }
                else {
                    console.error(JSON.parse(xhr.response)[0].description);
                    this.setState({ errorText: JSON.parse(xhr.response)[0].description });
                }
            }.bind(this);
            xhr.send();
        }
        else {
            this.setState({ errorText: "Passwords are not matching." });
        }
    }

    emailChanged(event) {
        this.setState({ email: event.target.value });
    }

    nameChanged(event) {
        this.setState({ name: event.target.value });
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
                    <h1>Create new user</h1>
                    <form>
                        <FieldGroup id="formControlsUsername"
                                    type="username"
                                    label="Email"
                                    placeholder='Email'
                                    bsSize='lg'
                                    onChange={this.emailChanged.bind(this)} />
                        <FieldGroup id="formControlsName"
                                    type="username"
                                    label="Name"
                                    placeholder='Name'
                                    bsSize='lg'
                                    onChange={this.nameChanged.bind(this)} />
                        <FieldGroup id="formControlPassword"
                            label="Password"
                            type="password" 
                            placeholder='Password'
                            bsSize = 'lg'
                            onChange={this.passwordChanged.bind(this)}/>
                        <FieldGroup id="formConfirmPassword"
                                    label="Password"
                                    type="password"
                                    placeholder='Confirm password'
                                    bsSize='lg'
                                    onChange={this.passwordConfirmedChanged.bind(this)} />
                        <p>{this.state.errorText}</p>
                        <Button onClick={this.createNewUser.bind(this)}>
                            Submit
                        </Button>
                    </form>
                </div>
            </div>
            
        );
    }


}
