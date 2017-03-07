var PageHeader = ReactBootstrap.PageHeader;
var Panel = ReactBootstrap.Panel;

class HomePage extends React.Component {
    constructor(props) {
        super(props);
        this.id = 0;
        this.data = [JSON.parse('{ "id": "0", "header": "TDT4145", "text": "tralala" }')];
        this.indexTest = 4100;
    }

    addPanel() {
        this.id++;
        //this.data.push(JSON.parse('{ "header": "TDT4145", "text": "tralala" }'));
        //this.data.push(JSON.parse('{ "header": "TDT4145", "text": "blablabla" }'));
        this.data.unshift((JSON.parse('{ "id": '+this.id+', "header": "TDT'+this.indexTest+'", "text": "lalala" }')));
        this.forceUpdate();
        this.indexTest++;
    }

   removePanel() {
        this.data.pop();
        this.forceUpdate();
   }

   onClickPanel(element) {
       this.data.splice(this.getPanelIndexFromId(element.target.parentNode.id), 1);
       this.forceUpdate();
       //var panel = clickedPanel.target.parentNode
       //panel.parentNode.removeChild(panel);
   }

   getPanelIndexFromId(id) {
       for (var i = 0; i < this.data.length; i++) {
           if (this.data[i].id == id) {
               return i;
           }
       }
   }
    
    render() {
        var elementList = this.data.map((element) => {
            return (
                <Panel id={element.id} header={element.header} bsStyle="primary" onClick={this.onClickPanel.bind(this) }>
                    {element.text}
                </Panel>
            );
        })

        return (

            <div>
                <PageHeader>Welcome to COCOA!</PageHeader>
                <button onClick={this.addPanel.bind(this)}>Add panel</button>
                <button onClick={this.removePanel.bind(this)}>Remove panel</button>
                <div className="scroll">{elementList}</div>
                <button onClick={this.addPanel.bind(this)}>Add panel</button>
            </div>
            
        );
    }


}