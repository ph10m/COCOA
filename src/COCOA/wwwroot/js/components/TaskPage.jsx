var Button = ReactBootstrap.Button;
var PageHeader = ReactBootstrap.PageHeader;
var Panel = ReactBootstrap.Panel;

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

class TaskPage extends React.Component {
    constructor(props) {
        super(props);
    }

    render() {
        const elementList = this.props.enrolledCourses.map((c) => {
            return (
                <Bulletin
                    course={{ id: c.courseId, name: c.courseName, description: c.courseDescription }} 
    hoverPlate={true} />
);
    });

    return (
        <div>
            <PageHeader>Task</PageHeader>
            <div className="scroll">{elementList}</div>
        </div>
    );
    }


}
