var PageHeader = ReactBootstrap.PageHeader;
var Panel = ReactBootstrap.Panel;

class HomePage extends React.Component {
    constructor(props) {
        super(props);
        this.id = 0;
        this.data = [JSON.parse('{ "id": "0", "header": "TDT4145", "text": "tralala" }')];
        this.indexTest = 4100;
    }

    // Temporary on click listener for adding panel for testing
    addPanel() {
        this.id++;
        this.data.unshift((JSON.parse('{ "id": '+this.id+', "header": "TDT'+this.indexTest+'", "text": "lalala" }')));
        this.forceUpdate();
        this.indexTest++;
    }

    // Temporary on click listener for removing panel for testing
   removePanel() {
        this.data.pop();
        this.forceUpdate();
   }

    // On click listener for removing panel
   onClickClose(element) {
       this.data.splice(this.getPanelIndexFromId(element.target.parentNode.parentNode.id), 1);
       this.forceUpdate();
   }

    // Dummy on click listener fo panel
   onClickPanel(element) {
       console.log(element.target.parentNode.id);
   }

    // Get index of panel in data list
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
                <div className="panel panel-primary" id={element.id}>
                    <div className="panel-heading">
                        {element.header}
                        <button type="button" className="close" onClick={this.onClickClose.bind(this)}>
                            &times;
                        </button>
                    </div>
                    <div className="panel-body" onClick={this.onClickPanel.bind(this)}>
                        {element.text}
                    </div>
                </div>
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