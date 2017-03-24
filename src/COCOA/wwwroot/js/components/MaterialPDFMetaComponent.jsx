var Panel = ReactBootstrap.Panel;

class MaterialPDFMetaComponent extends React.Component {
    constructor(props) {
        super(props);
    }
    render() {
        return (
            <Panel header={this.props.name}>
                {this.props.description}
            </Panel>
        );
    }
}