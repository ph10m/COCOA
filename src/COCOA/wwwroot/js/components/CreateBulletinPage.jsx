var Button = ReactBootstrap.Button;
var FormGroup = ReactBootstrap.FormGroup;
var ControlLabel = ReactBootstrap.ControlLabel;
var FormControl = ReactBootstrap.FormControl;
var Checkbox = ReactBootstrap.Checkbox;

function createRemarkable() {
    var remarkable = (("undefined" != typeof global) && (global.Remarkable)) ? global.Remarkable : window.Remarkable;
    return new remarkable();
}

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


class CreateBulletinPage extends React.Component {
    constructor(props) {
        super(props);
        this.state = { courseId: this.props.data.courseId, title: '', content: '', href: '', stickey: false, bulletinType: '0', errorText: null };
    }



    createNewBulletin() {
        var xhr = new XMLHttpRequest();
        var courseId = this.state.courseId;
        var title = this.state.title;
        var content = this.state.content;
        var href = this.state.href;
        var bulletinType = this.state.bulletinType;
        var stickey = this.state.stickey;
        console.log("/course/newbulletin?courseid=" + encodeURIComponent(courseId) + "&title=" + encodeURIComponent(title) + "&content=" + encodeURIComponent(content) + "&href=" + encodeURIComponent(href) + "&bulletinType=" + encodeURIComponent(bulletinType) + "&stickey=" + encodeURIComponent(stickey) + "&persistent=true");
        xhr.open('post', "/course/newbulletin?courseid=" + encodeURIComponent(courseId) + "&title=" + encodeURIComponent(title) + "&content=" + encodeURIComponent(content) + "&href=" + encodeURIComponent(href) + "&stickey=" + encodeURIComponent(stickey) + "&persistent=true", true);
        xhr.onload = function () {
            if (xhr.status == 200) {
                console.log("Added bulletin " + title + ".");
                this.setState({ errorText: null });
            }
            else {
                console.error(JSON.parse(xhr.response)[0].description);
                this.setState({ errorText: JSON.parse(xhr.response)[0].description });
            }
        }.bind(this);
        xhr.send();
    }

    courseIdChanged(event) {
        this.setState({ courseId: event.target.options[event.target.selectedIndex].value });
    }

    titleChanged(event) {
        this.setState({ title: event.target.value });
    }

    contentChanged(event) {
        this.setState({ content: event.target.value });
    }

    hrefChanged(event) {
        this.setState({ href: event.target.value });
    }

    stickeyChanged(event) {
        this.setState({ stickey: event.target.checked });
    }

    bulletinTypeChanged(event) {
        this.setState({ bulletinType: event.target.value });
    }

    render() {
        var md = new createRemarkable();

        return (
            <div>
                <div>
                    <h1>Create new bulletin</h1>
                    <form>
                        <ControlLabel>Course</ControlLabel>
                        <FormControl componentClass="select"
                                   placeholder="Course"
                                   onChange={this.courseIdChanged.bind(this)}>
                        <option value={this.props.data.courseId} label={this.props.data.courseName} />
                        {this.props.data.assignedCourses.map(course => {
                            if (course.courseId == this.props.data.courseId) {
                                return null;
                            }

                            return (<option value={course.courseId} label={course.courseName } />);
                        })}
                        </FormControl>
                        <FieldGroup id="formControlsTitle"
                                    type="text"
                                    label="Title"
                                    placeholder='Title'
                                    bsSize='lg'
                                    onChange={this.titleChanged.bind(this)} />
                        <FieldGroup id="formControlsContent"
                                    type="text"
                                    label="Content"
                                    placeholder='Content'
                                    bsSize='lg'
                                    componentClass='textarea'
                                    onChange={this.contentChanged.bind(this)} />
                        <FieldGroup id="formControlsHref"
                                    type="text"
                                    label="Href"
                                    placeholder='Href'
                                    bsSize='lg'
                                    onChange={this.hrefChanged.bind(this)} />
                        <FormGroup controlId="formControlsbulletinType">
                            <ControlLabel>BulletinType</ControlLabel>
                            <FormControl componentClass="select" placeholder="select" onChange={this.bulletinTypeChanged.bind(this)}>
                                <option value="0">Normal</option>
                                <option value="1">Info</option>
                                <option value="2">Urgent</option>
                            </FormControl>
                        </FormGroup>
                        <Checkbox checked={this.state.stickey}
                                  onChange={this.stickeyChanged.bind(this)}>
                            Sticky
                        </Checkbox>
                        <p>{this.state.errorText}</p>
                        <Button onClick={this.createNewBulletin.bind(this)}>
                            Create bulletin
                        </Button>
                    </form>
                </div>
                <div>
                    <br /><br />
                    <Bulletin 
                        title={this.state.title}
                        content={this.state.content} />
                </div>
            </div>

        );
    }


}
