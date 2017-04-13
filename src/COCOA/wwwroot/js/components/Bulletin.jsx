
class Bulletin extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            selectedMaterial: -1
        };
    }

    render() {
        return (
            <div className="panel hoverPlate">
                <div className="panelHeaderNormal">
                    {this.props.title}
                </div>
                <div className="panelBody">
                    {(this.props.course) &&
                    <p className="bulletinCourseTitle">
                        <a href={"/course/index/" + this.props.course.id}>
                            {this.props.course.name}
                        </a>
                    </p>}
                    {this.props.course.description}
                    {this.props.content}
                    <br /><br />
                    {(this.props.materials != undefined) &&
                    <ButtonToolbar className="materialToolbar">
                        {this.props.materials.map((element, index) => {
                            return (
                                <Button onClick={this.setMaterial.bind(this, index)} active={this.checkMaterial(index)}>
                                    {element.name}
                                </Button>
                            );
                        })}
                    </ButtonToolbar>}
                    {(this.props.author != null) && (this.props.timestamp != null) && 
                    (<p className="panelFooter">Published by <b>{this.props.author}</b>, {this.props.timestamp}</p>)}
                </div>
                {(this.props.materials != undefined) &&
                <div className={"panelMaterialView " + (this.state.selectedMaterial == -1 ? "panelMaterialViewClosed" : "")}>
                    <iframe className={this.state.selectedMaterial == -1 ? "materialHidden" : "materialVisible"} src={"https://localhost:44395/course/getdocumentdata?documentid=" + this.props.materials[this.state.selectedMaterial].id + "#page=" + this.props.materials[this.state.selectedMaterial].page} height="600" width="100%"></iframe>
                </div>
                }
            </div>  
        );
    }
}