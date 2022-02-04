import { Component } from "react";
import { getData, putData } from "../services/AccessAPI";
import RoleList from "./RoleList";

export default class UsersRole extends Component {
    constructor(props) {
        super(props);
        this.state = {
            userId: '',
            fullName: '',
            userName: '',
            userRoles: [],
            roles: [],
            msg: '',
            loading: true
        };

        this.onSearch = this.onSearch.bind(this);
        this.onChange = this.onChange.bind(this);
        this.onSubmit = this.onSubmit.bind(this);
        this.handleCheckboxChange = this.handleCheckboxChange.bind(this);
    }

    onSubmit(e) {
        e.preventDefault();
        let userRoles = {
            userName: this.state.userName,
            roles: this.state.userRoles
        }
        putData('api/User/EditUserRoles', userRoles).then((result) => {
            let responseJson = result;
            if (responseJson) {
                this.setState({ msg: "User's roles updated successfully!" });
            }
        }

        );
    }

    handleCheckboxChange = (event) => {

        if (event.target.checked) {
            if (!this.state.userRoles.includes(event.target.value)) {
                this.setState(prevState => ({ userRoles: [...prevState.userRoles, event.target.value] }));
            }
        } else {
            this.setState(prevState => ({ userRoles: prevState.userRoles.filter(roleName => roleName !== event.target.value) }));
        }

    }

    onChange(e) {
        this.setState({ [e.target.name]: e.target.value });
    }


    componentDidMount() {
        this.getAllRoles();
    }


    onSearch(userName) {
        getData('api/User/GetUserDetailsByUserName/' + userName).then(
            (result) => {
                if (result) {
                    this.setState({
                        userRoles: result.roles,
                        fullName: result.fullName,
                        userName: result.userName,
                        loading: false
                    });
                }
            }
        );
    }

    getAllRoles() {
        //debugger;
        getData('api/Role/GetAll').then(
            (result) => {
                if (result) {
                    this.setState({
                        roles: result,
                        loading: false
                    });
                }
            }
        );
    }


    renderRoleList() {
        return (
            <RoleList roles={this.state.roles} userRoles={this.state.userRoles} onChange={this.handleCheckboxChange} />
        );
    };


    render() {

        let renderCheckbox = this.renderRoleList();


        return (
            <div>
                <h3>Users Role</h3>
                <div className="input-group">
                    <input className="col-md-3" type="text" name="userName" placeholder="Enter user name" value={this.state.userName} onChange={this.onChange}></input>
                    <span class="input-group-addon">&nbsp;</span>
                    <button className="btn btn-primary" onClick={() => this.onSearch(this.state.userName)}>
                        Search
                    </button>
                </div>
                <label>Full Name: {this.state.fullName}</label>
                <label className="col-md-4">User Name: {this.state.userName}</label>
                <hr></hr>



                <form onSubmit={this.onSubmit}>
                    <div className="form-group">
                        {renderCheckbox}
                    </div>
                    <div className="form-group">
                        <input type="submit" value="Save" className="btn btn-primary"></input>
                    </div>
                </form>
                <label>{this.state.msg}</label>
            </div>
        );
    }
}