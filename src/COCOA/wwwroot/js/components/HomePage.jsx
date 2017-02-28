var PageHeader = ReactBootstrap.PageHeader;
var Panel = ReactBootstrap.Panel;

class HomePage extends React.Component {
    constructor(props) {
        super(props);
        
    }
    clickedPanel() {
        alert('You have clicked on me');
    }
    
    render() {
        var data = [];
        data.push(JSON.parse('{ "header": "TDT4145", "text": "tralala" }'));
        data.push(JSON.parse('{ "header": "TDT4145", "text": "blablabla" }'));
        var elementList = data.map((element) => {
            return (
                <Panel header={element.header}>
                    {element.text}
                </Panel>
            );
        })

        return (

            <div>
                <CocoaHeader/>
                <PageHeader>Welcome to COCOA!</PageHeader>
                {elementList}
            </div>
            
        );
    }


}