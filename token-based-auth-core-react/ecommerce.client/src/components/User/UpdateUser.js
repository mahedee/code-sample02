import { Component } from "react";
import { getData } from "../services/AccessAPI";

export default class UpdateUser extends Component{
    constructor(props){
        super(props);
        this.state = {
            id: '',
            fullName: '',
            userName: '',
            email: ''
        };

    }

    componentDidMount(){
        const {id} = this.props.match.params;
        getData('api/User/GetUserDetails/' + id).then(
            (result) => {
                //let responseJson = result;
                console.log("user for edit: ");
                console.log(result);
                if (result) {
                    this.setState({
                        //users: result,
                        id: result.id,
                        fullName: result.fullName,
                        userName: result.userName,
                        email: result.email,
                        loading: false
                    });
                }
            }
        );
    }

    render(){
        return(
            <h3>Edit user information</h3>
        );
    }
}