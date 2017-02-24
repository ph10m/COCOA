var PageHeader = ReactBootstrap.PageHeader;

class HomePage extends React.Component {
    constructor(props) {
        super(props);
    }

    render() {
        return (

            <div>
                <CocoaHeader/>
                <PageHeader>Welcome to COCOA!</PageHeader>
            </div>
            
        );
    }


}
