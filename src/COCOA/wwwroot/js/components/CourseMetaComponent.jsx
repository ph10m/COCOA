var Panel = ReactBootstrap.Panel;
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
            <Panel header={this.props.name} onSelect={this.props.onSelect}>
                <div float='left'>
                {this.props.description}
                </div>
                <div float='right'>
                    <Button onClick={this.enroll.bind(this)}>
                        Enroll
                    </Button>
                </div>
                

            </Panel>
        );
    }
}