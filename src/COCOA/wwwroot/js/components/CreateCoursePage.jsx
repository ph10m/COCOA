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

class CreateCoursePage extends React.Component {
    constructor(props) {
        super(props);

        this.state = { code: '', name: '', description: '', name1024: '', errorText: null };
    }



    createNewCourse() {
        var xhr = new XMLHttpRequest();
        var code = this.state.code;
        var name = this.state.name;
        var description = this.state.description;
        var name1024 = this.state.name1024;
        console.log("Create course with code: " + code + ", " + name + ", " + description + " and " + name1024);
        xhr.open('get', "/course/newcourse?code=" + encodeURIComponent(code) + "&name=" + encodeURIComponent(name) + "&description=" + encodeURIComponent(description) + "&name1024=" + encodeURIComponent(name1024) + "&persistent=true", true);
        xhr.onload = function () {
            if (xhr.status == 200) {
                console.log("Added course " + code + ".");
                this.setState({ errorText: null });
            }
            else {
                console.error(JSON.parse(xhr.response)[0].description);
                this.setState({ errorText: JSON.parse(xhr.response)[0].description });
            }
        }.bind(this);
        xhr.send();
    }

    codeChanged(event) {
        this.setState({ code: event.target.value });
    }

    nameChanged(event) {
        this.setState({ name: event.target.value });
    }

    descriptionChanged(event) {
        this.setState({ description: event.target.value });
    }

    name1024Changed(event) {
        this.setState({ name1024: event.target.value });
    }

    render() {
        return (
            <div>
                <div>
                    <h1>Create new course</h1>
                    <form>
                        <FieldGroup id="formControlsCode"
                                    type="text"
                                    label="Code"
                                    placeholder='Code'
                                    bsSize='lg'
                                    onChange={this.codeChanged.bind(this)} />
                        <FieldGroup id="formControlsName"
                                    type="text"
                                    label="Name"
                                    placeholder='Name'
                                    bsSize='lg'
                                    onChange={this.nameChanged.bind(this)} />
                        <FieldGroup id="formControlsDescription"
                                    type="text"
                                    label="Description"
                                    placeholder='Description'
                                    bsSize='lg'
                                    componentClass='textarea'
                                    onChange={this.descriptionChanged.bind(this)} />
                        <FieldGroup id="formControlsName1024"
                                    type="text"
                                    label="Name on timetable generator"
                                    placeholder='Name'
                                    bsSize='lg'
                                    onChange={this.name1024Changed.bind(this)} />
                        <p>{this.state.errorText}</p>
                        <Button onClick={this.createNewCourse.bind(this)}>
                            Create course
                        </Button>
                    </form>
                </div>
            </div>

        );
    }


}
