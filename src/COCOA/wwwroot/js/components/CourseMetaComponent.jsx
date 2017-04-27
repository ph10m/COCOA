var Button = ReactBootstrap.Button;
var Panel = ReactBootstrap.Panel;
var Modal = ReactBootstrap.Modal;


class CourseMetaComponent extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            showModal: false,
            enrolled: false,
            followText: "Follow"
        };
        this.enroll = this.enroll.bind(this);
        this.closeModal = this.closeModal.bind(this);
        this.openModal = this.openModal.bind(this);
        this.unfollow = this.unfollow.bind(this);
    }

    closeModal() {
        this.setState({
            showModal: false
        });
    }
    openModal() {
        this.setState({
            showModal: true
        });
        console.log('opening modal');
    }
    unfollow() {
        this.setState({
            enrolled: false
        });
        this.closeModal();
    }

    enroll() {
        this.setState({ enrolled: true });
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
        this.openModal();


    }

    render() {
        return (
            <Panel className="panel">
                <div className="panelHeaderNormal">
                    {this.props.name}
                </div>
                <div className="panelBody">
                    {this.props.description}
                    <br /><br />
                    <Button disabled={this.state.enrolled} onClick={this.enroll.bind(this)}>
                        Follow
                    </Button>
                    <Modal show={this.state.showModal} onHide={this.closeModal}>
                        <Modal.Header closeButton>
                            <Modal.Title>Now following {this.props.name}</Modal.Title>
                        </Modal.Header>
                        <Modal.Body>
                            <p>If this was a mistake, click Unfollow.</p>
                        </Modal.Body>
                        <Modal.Footer>
                            <Button onClick={this.unfollow}>Unfollow</Button>
                            <Button onClick={this.closeModal}>Close</Button>
                        </Modal.Footer>
                    </Modal>
                </div>
                

            </Panel>
        );
    }
}