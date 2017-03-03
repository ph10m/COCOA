var PageHeader = ReactBootstrap.PageHeader;
var Panel = ReactBootstrap.Panel;

class HomePage extends React.Component {
    constructor(props) {
        super(props);
        this.data = [JSON.parse('{ "header": "TDT4145", "text": "tralala" }')];
        this.indexTest = 4100;
        this.id = 0;
    }
    clickedPanel() {
        alert('You have clicked on me');
    }

    addPanel() {
        //this.data.push(JSON.parse('{ "header": "TDT4145", "text": "tralala" }'));
        //this.data.push(JSON.parse('{ "header": "TDT4145", "text": "blablabla" }'));
        this.data.unshift((JSON.parse('{ "header": "TDT'+this.indexTest+'", "text": "lalala" }')));
        this.forceUpdate();
        this.indexTest++;
        console.log(this.data);
    }

   removePanel() {
        this.data.pop();
        this.forceUpdate();
        console.log(this.data);
   }

   onClickPanel(clickedPanel) {
       var panel = clickedPanel.target.parentNode
       panel.parentNode.removeChild(panel);
   }
    
    render() {
        var elementList = this.data.map((element) => {
            return (
                <Panel header={element.header} onClick={this.onClickPanel.bind(this)}>
                    {element.text}
                </Panel>
            );
        })

        return (

            <div>
                <CocoaHeader/>
                <PageHeader>Welcome to COCOA!</PageHeader>
                <button onClick={this.addPanel.bind(this)}>Add panel</button>
                <button onClick={this.removePanel.bind(this)}>Remove panel</button>
                <div className="scroll">{elementList}</div>
                <button onClick={this.addPanel.bind(this)}>Add panel</button>
            </div>
            
        );
    }


}