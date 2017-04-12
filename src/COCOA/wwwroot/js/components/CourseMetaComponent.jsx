var Button = ReactBootstrap.Button;

class CourseMetaComponent extends React.Component {
    constructor(props) {
        super(props);
    }

    enroll() {
        var xhr = new XMLHttpRequest();
        var id = this.props.id;
        xhr.open('get', "/course/enrolltocourse?id=" + id, true);
        xhr.onload = function () {
            if (xhr.status == 200) {
                console.log("Enrolled to " + this.props.name);
            } else {
                console.log("Did not enroll");
            }
        }.bind(this);
        xhr.send();
    }

    render() {
        return (
            <Panel className="panel" onSelect={this.props.onSelect}>
                <div className="panelHeaderNormal">
                    {this.props.name}
                </div>
                <div className="panelBody">
                    {this.props.description}
                    <br /><br />
                    <Button onClick={this.enroll.bind(this)}>
                        Enroll
                    </Button>
                </div>
                

            </Panel>
        );
    }
}