var Button = ReactBootstrap.Button;
var FormGroup = ReactBootstrap.FormGroup;
var ControlLabel = ReactBootstrap.ControlLabel;
var FormControl = ReactBootstrap.FormControl;
var HelpBlock = ReactBootstrap.HelpBlock;

class EnrollmentPage extends React.Component {
    constructor(props) {
        super(props);

        this.state = { searchString: '', result: [] };
    }

    handleSearchStringChange(e) {
        this.setState({ searchString: e.target.value });
    }

    searchInCourses() {
        console.log("Searching with " + this.state.searchString);
        var xhr = new XMLHttpRequest();
        var searchString = this.state.searchString;
        xhr.open('get', "/course/coursesearch?searchString=" + searchString, true);
        xhr.onload = function () {
            if (xhr.status == 200) {
                this.setState({ result: JSON.parse(xhr.response) });
            }
        }.bind(this);
        xhr.send();
    }

    render() {
        viewRef = this;
        return (
            <div>
                <form>
                    <h1>Course enrollment</h1>
                    <FormGroup controllerId="formSearch">
                        <ControlLabel>Search:</ControlLabel>
                        <FormControl type="text"
                            value={this.state.searchString}
                            placeholder="Enter search string"
                            onChange={this.handleSearchStringChange.bind(this)} />
                        <HelpBlock>We will search for courses containing this string</HelpBlock>
                        <Button onClick={this.searchInCourses.bind(this)}
                            disabled={!this.state.searchString.length > 0}>
                            Search
                        </Button>
                    </FormGroup>
                </form>
            <div>
            {this.state.result.map(function (el) {
                return (
                    <CourseMetaComponent name={el.name} 
                        description={el.description}
                        id={el.id}>
                    </CourseMetaComponent>
                    );
                })  
                }
            </div>
        </div>
        );
    }
}
