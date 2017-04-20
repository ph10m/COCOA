var FormControl = ReactBootstrap.FormControl;
var ControlLabel = ReactBootstrap.ControlLabel;
var PageHeader = ReactBootstrap.PageHeader;
var FormGroup = ReactBootstrap.FormGroup;

class TaskPage extends React.Component {
    constructor(props) {
        super(props);
        this.state = { searchString: '', result: [] };
    }

    handleSearchStringChange(event) {
        this.setState({ searchString: event.target.value });
    }

    search(event) {
        event.preventDefault();
        console.log(this.state.searchString);
    }

    render() {
        return (
            <div>
                <form onSubmit={this.search.bind(this)}>
                    <h1>Search</h1>
                    <FormGroup controllerId="formSearch">
                        <ControlLabel>Search:</ControlLabel>
        <FormControl type="text"
        value={this.state.searchString}
        placeholder="Enter search string"
        onChange={this.handleSearchStringChange.bind(this)} />
</FormGroup>
</form>
        </div>
        );
            }
}
