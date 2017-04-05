var Panel = ReactBootstrap.Panel;
var Button = ReactBootstrap.Button;

class CourseMetaComponent extends React.Component {
    constructor(props) {
        super(props);
    }

    enroll() {
        console.log(this);
        console.log(this.props);
        console.log(this.props.id);
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