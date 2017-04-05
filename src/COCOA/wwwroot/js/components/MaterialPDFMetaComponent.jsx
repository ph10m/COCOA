var Panel = ReactBootstrap.Panel;
var Button = ReactBootstrap.Button;

class MaterialPDFMetaComponent extends React.Component {
    constructor(props) {
        super(props);
    }
    render() {
        return (
            <Panel header={this.props.name} onSelect={this.props.onSelect}>
                <div float='left'>
                {this.props.description}
                </div>
                <div float='right'>
                    <Button target="_blank" href={"/course/getdocumentdata?documentid=" + this.props.id}>
                        Download
                    </Button>
                </div>
                

            </Panel>
        );
    }
}