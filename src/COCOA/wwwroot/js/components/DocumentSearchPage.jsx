var Button = ReactBootstrap.Button;
var FormGroup = ReactBootstrap.FormGroup;
var ControlLabel = ReactBootstrap.ControlLabel;
var FormControl = ReactBootstrap.FormControl;
var HelpBlock = ReactBootstrap.HelpBlock;
var CenterButton = '/standalone/centerButton.jsx';

class DocumentSearchPage extends React.Component {
    constructor(props) {
        super(props);

        this.state = { searchString: '', courseId: '', result: [] };
    }

    handleSearchStringChange(e) {
        this.setState({ searchString: e.target.value });
    }

    handleCourseIdChange(event) {
        this.setState({ courseId: event.target.options[event.target.selectedIndex].value });
    }

    searchInDocuments() {
        console.log("Searching with " + this.state.searchString);
        var xhr = new XMLHttpRequest();
        var searchString = this.state.searchString;
        xhr.open('get', "/course/documentsearch?courseId=" + this.state.courseId + "&searchString=" + searchString, true);
        xhr.onload = function () {
            if (xhr.status == 200) {
                this.setState({ result: JSON.parse(xhr.response) });
            }
        }.bind(this);
        xhr.send();
    }

    openDocument(event) {
        id = event.target.id;
        var xhr = new XMLHttpRequest();
        xhr.open('get', "/course/getdocumentdata?documentid=" + id);
        xhr.send();
    }

    searchButton() {
        return (
            <div className="centeredButton">
            <Button
                    onClick={this.searchInDocuments.bind(this)}
                    disabled={!(this.state.courseId.length > 0 &&
                    this.state.searchString.length > 0)}>
                Search
            </Button>
            </div>
        );
    }

    render() {

        viewRef = this;
        return (

        <div className="content">
              <h1>Document search</h1>
                <form onSubmit={this.searchInDocuments.bind(this)}>
                <ControlLabel className="docSearchField">Search:</ControlLabel>
                    <FormControl type="text"
                                 value={this.state.searchString}
                                 placeholder="Enter search string"
                                 onChange={this.handleSearchStringChange.bind(this)} />
                <FormGroup controlId="formControlsSelect">

                <ControlLabel className="docSearchField">Course:</ControlLabel>
                    <FormControl componentClass="select"
                                placeholder="Course"
                                onChange={this.handleCourseIdChange.bind(this)}>
                        <option value="" label=""/>
                        {this.props.courses.map(course => {
                            return (<option value={course.courseId} label={course.courseName} />)
                        })}
                    </FormControl>
                </FormGroup>
                <HelpBlock>We will search for this string in the course material</HelpBlock>
                    {this.searchButton()}
                </form>
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
        );
    }


}
