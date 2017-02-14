var Button = ReactBootstrap.Button;

class HomePage extends React.Component {
    constructor(props) {
        super(props);

        this.handleClick = this.handleClick.bind(this);
    }

    render() {
        return (
            <div>
                <p>Hello COCOA!</p>
                <Button bsStyle="success" onClick={this.handleClick}>Lets rumble!</Button>
            </div>
        );
    }

    handleClick(e) {
        console.log("Rumbling!");
    }
}