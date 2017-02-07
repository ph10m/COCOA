var Button = ReactBootstrap.Button;

class HomePage extends React.Component {
    constructor(props) {
        super(props);
    }

    render() {
        return (
            <div>
                <p>Hello COCOA!</p>
                <Button bsStyle="success">Lets rumble!</Button>
            </div>
        );
    }

    handleRumbleClick(e) {
        console.log("Rumbling!");
    }
}