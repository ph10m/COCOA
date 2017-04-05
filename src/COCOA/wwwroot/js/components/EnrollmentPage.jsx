var Button = ReactBootstrap.Button;
var FormGroup = ReactBootstrap.FormGroup;
var ControlLabel = ReactBootstrap.ControlLabel;
var FormControl = ReactBootstrap.FormControl;
var HelpBlock = ReactBootstrap.HelpBlock;

class EnrollmentPage extends React.Component {
    constructor(props) {
        super(props);

        this.state = { searchString: '', result: [] };

        this.searchInCourses = this.searchInCourses.bind(this);
    }

    handleSearchStringChange(e) {
        this.setState({ searchString: e.target.value });
        this.searchInCourses(e.target.value);
    }

    searchInCourses(searchString) {
        var xhr = new XMLHttpRequest();
        xhr.open('get', "/course/coursesearch?searchString=" + searchString, true);
        xhr.onload = function () {
            if (xhr.status == 200) {
                this.setState({ result: JSON.parse(xhr.response) });
            }
        }.bind(this);
        xhr.send();
    }

    render() {
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
