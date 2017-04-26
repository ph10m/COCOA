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
        return (
            <div>
                <PageHeader>{this.props.data.courseName}</PageHeader>
                <div>
                    <Col md={8}>
                        <h3>Overview</h3>{this.props.data.courseDescription}

                    </Col>
                    <Col md={2}>
                        <h3>Relevant links</h3>
                        <Button>Wikipedia</Button>
                    <p></p>
                        <Button>Wikipendium</Button>
                    <p></p>
                        <h3>Coordinator</h3>
                        <p>Morten Hovd</p>
                        <a href={`mailto:${this.props.data.courseCoordinator}`}>
                            {this.props.data.courseCoordinator}
                        </a>
                    </Col>
  
                </div>
            </div>
        );
    }
}