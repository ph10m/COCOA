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

        this.state = { email: '', password: '' };
    }



    sendLoginRequest() {
        var xhr = new XMLHttpRequest();
        var email = this.state.email;
        var password = this.state.password;

        xhr.open('get', "/user/signin?email=" + encodeURIComponent(email) + "&password=" + encodeURIComponent(password) + "&persistent=true", true);
        xhr.onload = function () {
            if (xhr.status == 200) {
                console.log("Signed in with " + email + ".");
            }
            else {
                console.log("Failed to sign in, wrong email or password.");
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
                <CocoaHeader />
                  <div className='container'>
                  <form>
                    <FieldGroup id="formControlsUsername"
                                type="username"
                                label="Username"
                                placeholder='Username'
                                bsSize='lg'
                                onChange={this.emailChanged.bind(this)}/>
                    <FieldGroup id="formControlPassword"
                                label="Password"
                                type="password" 
                                placeholder='Password'
                                onChange={this.passwordChanged.bind(this)}/>
                    <Button onClick={this.sendLoginRequest.bind(this)}>
                        Submit
                    </Button>
                </form>
                </div>
            </div>
            
        );
    }


}
