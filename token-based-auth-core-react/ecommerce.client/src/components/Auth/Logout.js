import { Component } from "react";
import SessionManager from "./SessionManager";

export default class Logout extends Component{
    constructor(){
        super();
        this.state = {

        }
    }

    componentDidMount(){
        console.log("component did mount for logout");
        SessionManager.removeUserSession();
        window.location.href = "/login";
    }

    render(){
        return(
            <div></div>
        );
    }

}