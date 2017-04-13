var Panel = ReactBootstrap.Panel;
var Button = ReactBootstrap.Button;
var ButtonToolbar = ReactBootstrap.ButtonToolbar;

class MaterialPDFMetaComponent extends React.Component {
    constructor(props) {
        super(props);

        this.state = { view: false };
    }
    render() {
        return (
            <div className="panel" header={this.props.name} onSelect={this.props.onSelect}>
                <div className="panelHeaderNormal">
                {this.props.name}
                </div>
                <div className="panelBody">
                    {this.props.description}
                    <br /><br />
                    <ButtonToolbar>
                        <Button onClick={this.toggleView.bind(this)}>
                            View
                        </Button>
                        <Button download href={"/course/getdocumentdata?documentid=" + this.props.id}>
                            <Glyphicon glyph="download" />
                        </Button>
                    </ButtonToolbar>
                </div>
                <div className={"panelMaterialView " + (!this.state.view ? "panelMaterialViewClosed" : "")}>
                        <iframe className={this.state.materialId == -1 ? "materialHidden" : "materialVisible"} src={"https://localhost:44395/course/getdocumentdata?documentid=" + this.props.id + "#page=2"} height="600" width="100%"></iframe>
                </div>
            </div>
        );
    }

    toggleView() {
        this.setState({ view: !this.state.view }); 
    }
}