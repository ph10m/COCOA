﻿var Button = ReactBootstrap.Button;
var PageHeader = ReactBootstrap.PageHeader;
var Panel = ReactBootstrap.Panel;

class HomePage extends React.Component {
    constructor(props) {
        super(props);

        //this.data = [JSON.parse('{ "id": "0", "header": "TDT4145", "text": "tralala" }')];
        //this.indexTest = 4100;
    }

    // Temporary on click listener for adding panel for testing
    addPanel() {
        this.id++;
        this.data.unshift((JSON.parse(`{ "id": ${this.id}, "header": "TDT${this.indexTest}", "text": "lalala" }`)));
        this.forceUpdate();
        this.indexTest++;
    }

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
       for (let i = 0; i < this.data.length; i++) {
           if (this.data[i].id === id) {
               return i;
           }
       }
   }

   render() {
        const enrolled = this.props.enrolledCourses.map((c) => {
            return (
                <Bulletin
                    course={{ id: c.courseId, name: c.courseName, description: c.courseDescription }} 
                    hoverPlate={true} />
            );
        });

       const assigned = this.props.assignedCourses.map((c) => {
           return (
               <Bulletin
                   course={{ id: c.courseId, name: c.courseName }} 
                   hoverPlate={true} />
            );
        });

        return (
            <div>
                <PageHeader>Courses</PageHeader>
                <div className="scroll">
                    {enrolled}
                    <hr />
                    {assigned}
                </div>
            </div>
        );
    }
}