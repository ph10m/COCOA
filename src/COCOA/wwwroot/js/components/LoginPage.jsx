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

class LoginPage extends React.Component {
    constructor(props) {
        super(props);

        this.state = { email: '', password: '', error: false };
    }



    sendLoginRequest() {
        var xhr = new XMLHttpRequest();
        var email = this.state.email;
        var password = this.state.password;
        this.setState({ error: false });

        xhr.open('get', "/user/signinuser?email=" + encodeURIComponent(email) + "&password=" + encodeURIComponent(password) + "&persistent=true", true);
        xhr.onload = function () {
            if (xhr.status == 200) {
                console.log("Signed in with " + email + ".");
                window.location.href = "/";
            }
            else {
                console.log("Failed to sign in, wrong email or password.");
                this.setState({ error: true });
            }
        }.bind(this);
        xhr.send();
    }

    emailChanged(event) {
        this.setState({ email: event.target.value });
    }

    passwordChanged(event) {
        this.setState({ password: event.target.value });
    }

    render() {
        return (
              <div>
                  <div>
                    <h1>Log in</h1>
                  <form>
                    <FieldGroup id="formControlsUsername"
                        type="username"
                        label="Email"
                        placeholder='Email'
                        bsSize='lg'
                        onChange={this.emailChanged.bind(this)}/>
                    <FieldGroup id="formControlPassword"
                        label="Password"
                        type="password" 
                        placeholder='Password'
                        bsSize='lg'
                        onChange={this.passwordChanged.bind(this)}/>
                    {this.state.error && (<p>Wrong password or username!</p>)}
                    <Button onClick={this.sendLoginRequest.bind(this)}>
                        Log in
                    </Button>
                </form>
                </div>
            </div>
            
        );
    }


}
