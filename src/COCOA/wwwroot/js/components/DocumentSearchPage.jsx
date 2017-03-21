var Button = ReactBootstrap.Button;
var FormGroup = ReactBootstrap.FormGroup;
var ControlLabel = ReactBootstrap.ControlLabel;
var FormControl = ReactBootstrap.FormControl;
var HelpBlock = ReactBootstrap.HelpBlock;

class DocumentSearchPage extends React.Component {
    constructor(props) {
        super(props);

        this.state = { searchString: '', courseId: '', result: [] };
    }

    handleSearchStringChange(e) {
        this.setState({ searchString: e.target.value });
    }

    handleCourseIdChange(e) {
        this.setState({ courseId: e.target.value });
    }

    searchInDocuments() {
        console.log("Searching with " + this.state.searchString);
        var xhr = new XMLHttpRequest();
        var searchString = this.state.searchString;
        xhr.open('get', "/course/documentsearch?courseId=" + this.state.courseId + "&searchString=" + searchString, true);
        xhr.onload = function () {
            if (xhr.status == 200) {
                console.log("Got response: " +
                xhr.response);
                this.setState({ result: JSON.parse(xhr.response) });
            } else {
                console.log("Got status " + xhr.status);
            }
        }.bind(this);
        xhr.send();
    }

    render() {
        return (

        <div>
          <form>
              <div className='container'>
            <FormGroup controllerId="formSearch">
                <ControlLabel>
                    Search:
                </ControlLabel>
                <FormControl type="text"
                             value={this.state.searchString}
                             placeholder="Enter search string"

                             onChange={this.handleSearchStringChange.bind(this)} />
                <FormControl type="text"
                             value={this.state.courseId}
                             placeholder="Course code"
                             onChange={this.handleCourseIdChange.bind(this)} />
                <HelpBlock>We will search for this string in the course material</HelpBlock>
                <Button onClick={this.searchInDocuments.bind(this)}>
                    Search
                </Button>
            </FormGroup>
              </div>
          </form>

          <div>
            {this.state.result.map(function(el) {
                return (
                    <MaterialPDFMetaComponent name={el.name} description={el.description}>
                    </MaterialPDFMetaComponent>
                    );
                })
            }
          </div>
        </div>

);
    }


}
