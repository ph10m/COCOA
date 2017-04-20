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

class TaskPage extends React.Component {
    constructor(props) {
        super(props);
    }

    render() {
        return (
              <div>
                  <div>
                    <h1>Tasks</h1>
                  <form>
                    <FieldGroup id="formControlsUsername"
                        type="username"
                        label="Email"
                        placeholder='Email'
                        bsSize='lg'
                        />
                </form>
                </div>
            </div>
            
        );
    }


}
