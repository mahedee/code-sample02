import { Component } from "react";
import { getData, putData } from "../services/AccessAPI";

export default class UpdateUser extends Component{
    constructor(props){
        super(props);
        this.state = {
            id: '',
            fullName: '',
            userName: '',
            email: '',
            roles: []
        };

        this.onChange = this.onChange.bind(this);
        this.onSubmit = this.onSubmit.bind(this);

    }

    onChange(e) {
        this.setState({ [e.target.name]: e.target.value });
    }

    onKeyDown = (e) => {
        if (e.key === 'Enter') {
            this.update(false);
        }
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

    onSubmit(e){
        e.preventDefault();
        const {history} = this.props;
        const {id} = this.props.match.params;

        let userProfile = {
            id: this.state.id,
            fullName: this.state.fullName,
            email: this.state.email,
            roles: this.state.roles
        }

        putData('api/User/EditUserProfile/' + id, userProfile).then((result) => {
            let responseJson = result;
            console.log("update response: ");
            
            if(responseJson){
                console.log(responseJson);
                history.push('/admin/users');
            }
        }

        );
    }

    render(){
        return(
            <div className="row">
                <div className="col-md-4">
                    <h3>Edit User</h3>
                    <form onSubmit={this.onSubmit}>
                        <div className="form-group">
                            <label className="control-label">Full Name: </label>
                            <input className="form-control" type="text" value={this.state.fullName} onChange={this.onChange} name="fullName"
                            onKeyDown={this.onKeyDown} ></input>
                        </div>

                        <div className="form-group">
                            <label className="control-label">User Name: </label>
                            <input className="form-control" type="text" value={this.state.userName} disabled = {true} readOnly = {true}></input>
                        </div>

                        <div className="form-group">
                            <label className="control-label">Email: </label>
                            <input className="form-control" type="text" value={this.state.email} onChange={this.onChange} name="email"
                            onKeyDown={this.onKeyDown}></input>
                        </div>

                        <div className="form-group">
                            <button onClick={this.onUpdateCancel} className="btn btn-default">Cancel</button>
                            <input type="submit" value="Edit" className="btn btn-primary"></input>
                        </div>

                    </form>

                </div>
            </div>
        );
    }
}