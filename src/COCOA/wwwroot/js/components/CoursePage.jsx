var Button = ReactBootstrap.Button;
var PageHeader = ReactBootstrap.PageHeader;
var Panel = ReactBootstrap.Panel;

class CoursePage extends React.Component {
    constructor(props) {
        super(props);
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
                <div className="panel panel-primary" id={c.id}>
                    <div className="panel-heading">
                        {c.title}
                    </div>
                    <div className="panel-body">
                        {c.content}
                    <br />
                    Publisert av {c.authorName}, {c.publishedDate}.
                </div>
            </div>
            );
        });

        return (
            <div className="content">
                <PageHeader>Welcome to COCOA!</PageHeader>
                <div className="scroll">
                    {sticky}
                    {normal}
                </div>
            </div>
        );
    }
}