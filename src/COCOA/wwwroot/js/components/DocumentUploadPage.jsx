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
            <FormGroup controlId={this.props.id} 
                       validationState = {this.props.validationState}>
                <ControlLabel>{this.props.label}</ControlLabel>
                <FormControl {...this.props} />
            </FormGroup>
        );
    }
}

class DocumentUploadPage extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            file: undefined,
            materialName: '',
            courseId: '',
            validMaterialName: true
        };
    }

    fileChanged(event) {

        var name = event.target.files[0].name;
        if (name.lastIndexOf('.') > 0) {
            name = name.substring(0, name.lastIndexOf('.'));
        }
        this.setState({ file: event.target.files[0], materialName: name });
    }

    sendFile(event) {
        var reader = new FileReader();
        var materialName = this.state.materialName;
        var courseId = this.state.courseId;
        var description = this.state.description;
        reader.onload = function () {
            var array = this.result;
            var bytes = new Uint8Array(array);
            var xhr = new XMLHttpRequest();

            xhr.open('post', '/course/upload?name=' +
                encodeURIComponent(materialName)
                + '&courseId=' + encodeURIComponent(courseId)
                + '&description=' + encodeURIComponent(description)
                , true);
            xhr.onload = function () {
                console.log("Got status " + xhr.status + " and response '" + xhr.response + "'");
            }
            xhr.send(bytes);
        }
        reader.readAsArrayBuffer(this.state.file);
    }

    isValidMaterialName(name) {
        return name.length >= 3;
    }

    courseChanged(event) {
        this.setState({courseId: event.target.options[event.target.selectedIndex].value});
    }

    materialNameChanged(event) {
        var valid = 'error';
        if (this.isValidMaterialName(event.target.value)) {
            valid = 'success';
        }
        this.setState({ materialName: event.target.value, validMaterialName: valid });
    }

    descriptionChanged(event) {
        this.setState({ description: event.target.value });
    }

    render() {
            return (
                <div className="content">
                <div className='container'>
                  <h1>Upload document</h1>
                  <form>
                    <FieldGroup id='formControlsFile'
                                type='file'
                                label='Material file'
                                bsSize='lg'
                                accept='application/pdf'
                                onChange={this.fileChanged.bind(this)
                        } />

                      <FormGroup controlId="formControlsSelect">
                      <ControlLabel>Course</ControlLabel>
                      <FormControl componentClass="select" 
                                   placeholder="Course"
                                   onChange={this.courseChanged.bind(this)}>
                          <option value='' label=''/>
                         {this.props.courses.map(course => {
                             return (<option value={course.courseId} label={course.courseName}/>)
                            })
                         }
                      </FormControl>
                      </FormGroup>

                    <FieldGroup id="formControlName"
                                label="Material name"
                                type="text"
                                placeholder='Name'
                                validationState={this.state.validMaterialName}
                                value={this.state.materialName}
                                onChange={this.materialNameChanged.bind(this)
                        } />
                      <FieldGroup id="formControlDescription"
                                  label="Description (optional)"
                                  componentClass="textarea"
                                  placeholder='Description'
                                  value={this.state.description}
                                  onChange={this.descriptionChanged.bind(this)} />
                    <Button onClick={this.sendFile.bind(this)}
                            disabled={!(this.state.courseId.length > 0
                                        && this.isValidMaterialName(this.state.materialName)
                                        && this.state.materialName.length > 0
                                        && this.state.file !== undefined)}>
                        Upload
                    </Button>
                  </form>
                </div>
           </div>
            );
        }
    }