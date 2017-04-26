var Button = ReactBootstrap.Button;
var FormGroup = ReactBootstrap.FormGroup;
var ControlLabel = ReactBootstrap.ControlLabel;
var FormControl = ReactBootstrap.FormControl;
var HelpBlock = ReactBootstrap.HelpBlock;

class DocumentSearchPage extends React.Component {
    constructor(props) {
        super(props);

        this.state = { searchString: '', courseId: this.props.data.courseId, result: [], cache: {} };

        this.searchInDocuments = this.searchInDocuments.bind(this);
    }

    checkCacheAndSearch() {
        // Search if string has a non-zero length and course is selected AND the result isn't already cached
        if(this.state.searchString.length > 0 &&
            this.state.courseId.length > 0){
            if(this.state.cache[this.state.courseId + ';' + this.state.searchString]){
                this.setState({result: this.state.cache[this.state.courseId + ';' + this.state.searchString]})
            } else {
                this.searchInDocuments.bind(this)();
            }
        } else {
            this.setState({ result: []})
        }
    }

    handleSearchStringChange(e) {
        this.setState({ searchString: e.target.value });
        this.searchInDocuments();
    }

    handleCourseIdChange(event) {
        this.setState(
            { courseId: event.target.options[event.target.selectedIndex].value }
        );

        this.searchInDocuments();
    }

    searchInDocuments() {
        var xhr = new XMLHttpRequest();
        var searchString = this.state.searchString;
        xhr.open('get', "/course/documentsearch?courseId=" + this.state.courseId + "&searchString=" + searchString, true);
        xhr.onload = function () {
            if (xhr.status == 200) {
                var result = JSON.parse(xhr.response);
                var newCache = this.state.cache;
                newCache[this.state.courseId + ';' + searchString] = result;
                this.setState({
                    result: result,
                    cache: newCache
                });
            }
        }.bind(this);
        xhr.send();
    }

    openDocument(event) {
        id = event.target.id;
        var xhr = new XMLHttpRequest();
        xhr.open('get', "/course/getdocumentdata?documentid=" + id + "#page=2");
        xhr.send();
    }

    ignoreEnter(event) {
        event.preventDefault();
    }

    render() {

        viewRef = this;
        return (

        <div>
          <form onSubmit={this.ignoreEnter.bind(this)}>
              <h1>Document search</h1>
                <FormGroup controlId="formControlsSelect">
                      <ControlLabel>Course</ControlLabel>
                      <FormControl componentClass="select"
                                   placeholder="Course"
                                   onChange={this.handleCourseIdChange.bind(this)}>
                          {this.props.data.enrolledCourses.map(course => {
                          return (<option value={course.courseId} label={course.courseName } />);
                          })}
                          {this.props.data.assignedCourses.map(course => {
                              return (<option value={course.courseId} label={course.courseName } />);
                          })}
                      </FormControl>
                </FormGroup>
              <FormGroup controllerId="formSearch">
                <ControlLabel>
                    Search:
                </ControlLabel>
                <FormControl type="text"
                             value={this.state.searchString}
                             placeholder="Enter search string"
                             onChange={this.handleSearchStringChange.bind(this)} />
              </FormGroup>
          </form>

          <div>
            {this.state.result.map(function (el) {
                return (
                    <MaterialPDFMetaComponent name={el.name} 
                                              description={el.description}
                                              id={el.id}
                                              download={viewRef.openDocument}>
                    </MaterialPDFMetaComponent>
                    );
                })  
            }
          </div>
        </div>

);
    }


}
