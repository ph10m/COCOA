var Button = ReactBootstrap.Button;
var ButtonToolbar = ReactBootstrap.ButtonToolbar;
var PageHeader = ReactBootstrap.PageHeader;
var Panel = ReactBootstrap.Panel;
var Col = ReactBootstrap.Col;

class CoursePage extends React.Component {
    constructor(props) {
        super(props);

        this.state = { materialId: -1 };

        this.checkMaterial = this.checkMaterial.bind(this);
    }

    render() {
        const sticky = this.props.data.stickyBulletins.map((c) => {
            return (
                <div className="panel panel-primary" id={c.id}>
                    <div className="panel-heading">
                        {c.title} - STICKEYYYYY
                    </div>
                    <div className="panel-body">
                        {c.content}
                        <br />
                        Publisert av {c.authorName}, {c.publishedDate}.
                    </div>
                </div>
            );
        });

        const normal = this.props.data.bulletins.map((c) => {
            return (
                <div className="panel" id={c.id}>
                    <div className="panelHeaderNormal">
                        {c.title}
                    </div>
                    <div className="panelBody">
                        {c.content}
                        <br /><br />
                        <ButtonToolbar className="materialToolbar">
                            <Button onClick={this.setMaterial.bind(this, 1)} active={this.checkMaterial(1)}>Document 1</Button>
                            <Button onClick={this.setMaterial.bind(this, 2)} active={this.checkMaterial(2)}>Document 2</Button>
                        </ButtonToolbar>
                        <p className="panelFooter">Published by <b>{c.authorName}</b>, {c.publishedDate}</p>
                    </div>
                    <div className={"panelMaterialView " + (this.state.materialId == -1 ? "panelMaterialViewClosed" : "")}>
                        <iframe className={this.state.materialId == -1 ? "materialHidden" : "materialVisible"} src={"https://localhost:44395/course/getdocumentdata?documentid=" + this.state.materialId} height="600" width="100%"></iframe>
                        
                    </div>
                </div>
            );
        });

        return (
            <div>
                <PageHeader>{this.props.data.courseName}</PageHeader>
                <div>
                    <Col md={8}>
                        {sticky}
                        {normal}
                    </Col>
                    <Col md={4}>
                        <h3>Overview</h3>
                        {this.props.data.courseDescription}
                        <h3>Managment</h3>
                        {this.props.data.courseManagment.map(function (element) {
                            return (
                                <p>{element}</p>    
                            );
                        })}
                    </Col>
                </div>
            </div>
        );
    }

    setMaterial(id) {
        if (this.state.materialId == id) {
            this.setState({ materialId: -1 });
        } else {
            this.setState({ materialId: id });
        }
    }

    checkMaterial(id) {
        return (id == this.state.materialId);
    }
}