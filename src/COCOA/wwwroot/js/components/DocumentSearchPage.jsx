var Button = ReactBootstrap.Button;
var FormGroup = ReactBootstrap.FormGroup;
var ControlLabel = ReactBootstrap.ControlLabel;
var FormControl = ReactBootstrap.FormControl;
var HelpBlock = ReactBootstrap.HelpBlock;

class DocumentSearchPage extends React.Component {
    constructor(props) {
        super(props);

        this.state = { searchString: '' };
    }

    handleChange(e) {
        this.setState({ searchString: e.target.value });
    }
    render() {
        return (

        <div>
        <CocoaHeader />
          <form>
              <div className='container'>
            <FormGroup controllerId="formSearch">
                <ControlLabel>
                    Search:
                </ControlLabel>
                <FormControl type="text"
                             value={this.state.searchString}
                             placeholder="Enter search string"
                             onChange={this.handleChange.bind(this)} />
                <HelpBlock>We will search for this string in the course material</HelpBlock>
            </FormGroup>
              </div>
          </form>
        </div>

        );
    }


}
