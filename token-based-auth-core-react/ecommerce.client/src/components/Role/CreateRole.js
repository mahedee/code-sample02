import { Component } from "react";

export default class CreateRole extends Component{
    constructor(props){
        super(props);
        this.state = {
            loading: true
        };

    }

    render(){
        return(
            <div>Create roles</div>
        );
    }
}