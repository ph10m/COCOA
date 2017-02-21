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
                                bsSize='lg'/>
                    <FieldGroup id="formControlPassword"
                                label="Password"
                                type="password" 
                                placeholder='Password'/>
                    <Button type="submit">
                        Submit
                    </Button>
                </form>
                </div>
            </div>
            
        );
    }


}
