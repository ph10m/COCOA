﻿var Button = ReactBootstrap.Button;
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
}