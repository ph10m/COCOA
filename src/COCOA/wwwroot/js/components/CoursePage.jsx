var Button = ReactBootstrap.Button;
var ButtonToolbar = ReactBootstrap.ButtonToolbar;
var PageHeader = ReactBootstrap.PageHeader;
var Panel = ReactBootstrap.Panel;
var Col = ReactBootstrap.Col;

class CoursePage extends React.Component {
    constructor(props) {
        super(props);
    }

    render() {
        const sticky = this.props.data.stickyBulletins.map((c) => {
            return (
                <Bulletin 
                    id={c.id}
                    title={c.title}
                    content={c.content}
                    author={c.authorName}
                    timestamp={c.publishedDate} />
            );
        });

        const normal = this.props.data.bulletins.map((c) => {
            return (
                <Bulletin 
                    id={c.id}
                    title={c.title}
                    content={c.content}
                    author={c.authorName}
                    timestamp={c.publishedDate} />
            );
        });
        const coordinator = this.props.data.courseCoordinator.split(",");
        const coordName = coordinator[0];
        const coordMail = coordinator[1].replace(/\s+/g, ""); //remove trailing spaces
        const courseCode = this.props.data.courseName.substr(0, this.props.data.courseName.indexOf(" "));
        const prettyText = `<p>${this.props.data.courseDescription.replace(/([!.?,)])/g, "$1<br>")}</p>`;

        return (
            <div>
                <PageHeader>{this.props.data.courseName}</PageHeader>
                <div>
                    <Col md={8}>
                        <h3>Overview</h3>
                        <div dangerouslySetInnerHTML={{ __html: prettyText }}></div>
                    <br />
                    <br />
                        {sticky}
                        {normal}
                    </Col>
                    <Col md={4}>
                     <h3>Additional information</h3>
                        <a href={this.props.data.courseInfolink} target="_blank">
                            <Button>Info page</Button>
                        </a>
                        <p></p>
                            <h3>Coordinator</h3>
                            <p>{coordName}</p>
                            <a href={`mailto:${coordMail}?subject=${courseCode}`}>{coordMail}</a>
                        <p></p>
                        <h3>Search course documents</h3>
                        <Button href={"/course/materialsearch/" + this.props.data.courseId}>
                            Search
                        </Button>
                        <p></p>
                        <ButtonToolbar>
                            {this.props.data.assigned &&
                            (<Button href={"/course/documentupload/" + this.props.data.courseId}>
                                Upload
                            </Button>)}
                            {this.props.data.assigned &&
                            (<Button href={"/course/createbulletin/" + this.props.data.courseId}>
                                Post Bulletin
                            </Button>)}
                        </ButtonToolbar>
                    </Col>
                </div>
            </div>
        );
    }
}