var Button = ReactBootstrap.Button;
var FormGroup = ReactBootstrap.FormGroup;
var ControlLabel = ReactBootstrap.ControlLabel;
var FormControl = ReactBootstrap.FormControl;
var HelpBlock = ReactBootstrap.HelpBlock;

const timeBetweenChecks = 50; //Milliseconds
const delaySearchAfterTyping = 500; //Milliseconds
const searchSpinDelay = 500; //Milliseconds

class DocumentSearchPage extends React.Component {
    constructor(props) {
        super(props);

        this.state = { searchString: '', courseId: this.props.data.courseId, result: [], cache: {} };

        this.state = { searchString: '', courseId: '', result: [], cache: {}, ongoingSearches: 0, searchSpinStateIndex: 0 };
        this.timer = 0.0;
        this.updated = false;
        this.ongoingSearches = 0; //Duplicated in object and state to ensure read/write errors don't occur
        typeof window !== "undefined" &&
            (this.countdown = window.setInterval(this.searchIfEnoughTimePassed.bind(this), timeBetweenChecks)); //Small workaround to make setInterval work with server-side rendering

        this.searchSpinStates = ["Searching", "Searching.", "Searching..", "Searching..."];
        typeof window !== "undefined" &&
            (this.searchSpinTimer = window.setInterval(this.updateSearchSpinState.bind(this), searchSpinDelay));
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

    searchIfEnoughTimePassed() {
        this.timer = Math.max(0, this.timer - timeBetweenChecks);
        if (this.timer == 0 && this.updated) {
            this.updated = false;
            this.checkCacheAndSearch.bind(this)();
        }
    }

    updateSearchSpinState() {
        this.setState({ searchSpinStateIndex: (this.state.searchSpinStateIndex + 1) % this.searchSpinStates.length });
    }

    handleSearchStringChange(e) {
        this.setState(
            { searchString: e.target.value },
            () => { this.updated = true; this.timer = delaySearchAfterTyping; }
        );
    }

    handleCourseIdChange(event) {
        this.setState(
            { courseId: event.target.options[event.target.selectedIndex].value },
            () => { this.updated = true; this.timer = delaySearchAfterTyping; });

    }

    searchInDocuments() {
        var xhr = new XMLHttpRequest();
        var searchString = this.state.searchString;
        xhr.open('get', "/course/documentsearch?courseId=" + this.state.courseId + "&searchString=" + searchString, true);
        xhr.onload = function () {
            this.ongoingSearches -= 1;
            this.setState({ ongoingSearches: this.ongoingSearches });
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
        this.ongoingSearches += 1;
        this.setState({ ongoingSearches: this.ongoingSearches });
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
                        <option value={this.props.data.courseId} label={this.props.data.courseName} />
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
            {this.state.ongoingSearches > 0 && ( <div><h4>{this.searchSpinStates[this.state.searchSpinStateIndex]}</h4> <br/></div> )}

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
