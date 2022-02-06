import { Component } from "react";
import SessionManager from "../Auth/SessionManager";
import { postData } from "../services/AccessAPI";

export default class CreateUser extends Component {
    constructor(props) {
        super(props);
        this.state = {
            fullName: '',
            email: '',
            userName: '',
            password: '',
            confirmationPassword: '',
            roles: [],
            loading: true
        };

        this.onSubmit = this.onSubmit.bind(this);
        this.onChange = this.onChange.bind(this);
        this.onClickBack = this.onClickBack.bind(this);

    }

    onSubmit(e) {
        e.preventDefault();
        const { history } = this.props;

        if (this.state.password !== this.state.confirmationPassword) {
            alert("Password and confirm password are not same");
            return;
        }

        let userObj = {
            fullName: this.state.userName,
            email: this.state.email,
            userName: this.state.userName,
            password: this.state.password,
            confirmationPassword: this.state.confirmationPassword,
            roles: []
        }

        postData('api/User/Create', userObj).then((result) => {
            let responseJson = result;
            if (responseJson) {
                history.push('/admin/users');
            }
        });
    }

    onClickBack(e){
        e.preventDefault();
        const { history } = this.props;

        if(SessionManager.getToken()){
            history.push('/admin/users');
        }else{
            history.push('/login');
        }   
    }

    onChange(e) {
        this.setState({ [e.target.name]: e.target.value });
    }

    render() {
        return (
            <div className="row">
                <div className="col-md-4">
                    <h3>Create new user</h3>
                    <form onSubmit={this.onSubmit}>
                        <div className="form-group">
                            <label className="control-label">Full Name: </label>
                            <input className="form-control" type="text" name="fullName" value={this.state.fullName} onChange={this.onChange}></input>
                        </div>

                        <div className="form-group">
                            <label className="control-label">Email: </label>
                            <input className="form-control" type="text" name="email" value={this.state.email} onChange={this.onChange}></input>
                        </div>

                        <div className="form-group">
                            <label className="control-label">User Name: </label>
                            <input className="form-control" type="text" name="userName" value={this.state.userName} onChange={this.onChange}></input>
                        </div>

                        <div className="form-group">
                            <label className="control-label">Password: </label>
                            <input className="form-control" type="password" name="password" value={this.state.password} onChange={this.onChange}></input>
                        </div>

                        <div className="form-group">
                            <label className="control-label">Confirm Password: </label>
                            <input className="form-control" type="password" name="confirmationPassword" value={this.state.confirmationPassword} onChange={this.onChange}></input>
                        </div>


                        <div className="form-group">
                            <input type="submit" value="Create User" className="btn btn-primary"></input> &nbsp; &nbsp; 
                            <input type="button" value="Back" onClick={this.onClickBack} className="btn btn-primary"></input>
                        </div>

                    </form>

                </div>
            </div>
        );
    }
}