import { Component } from "react";
import { getData } from "../services/AccessAPI";
import RoleList from "./RoleList";

export default class UsersRole extends Component {
    constructor(props) {
        super(props);
        this.state = {
            userId:  '',
            fullName: '',
            userName: '',
            userRoles: [],
            roles: [],
            loading: true
        };

        this.onUserCreate = this.onUserCreate.bind(this);
        this.onUserDelete = this.onUserDelete.bind(this);
        this.onSearch = this.onSearch.bind(this);
        this.onChange = this.onChange.bind(this);
    }

    onChange(e) {
        this.setState({ [e.target.name]: e.target.value });
    }

    componentDidMount() {
        this.getAllRoles();
    }

    onUserCreate() {
        const { history } = this.props;
        history.push('/admin/user/create');
    }


    onUserEdit(id) {
        const { history } = this.props;
        history.push('/admin/user/edit/' + id);
    }

    onUserDelete(id) {
        const { history } = this.props;
        history.push('/admin/user/delete/' + id);
    }


    onSearch(userName){

        alert(userName);

        getData('api/User/GetUserDetailsByUserName/' + userName).then(
            (result) => {
                if (result) {

                    //console.log(result.roles);
                    this.setState({
                        userRoles: result.roles,
                        loading: false
                    });
                    //console.log(this.state.userRoles);
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

        //console.log("users list: " + this.state.users);
    }

    // const renderList = renderRoleList({
    //     return(
    //         <RoleList roles = {this.state.roles} userRoles = {this.state.userRoles}/>
    //     );
    // });

    renderAllRoles(roles) {
        return (

            <div>
                <hr></hr>
                <label>User Name: </label>
                <span class="input-group-addon">&nbsp;</span>
                <label>User name</label>

                <h4>Roles</h4>
                <ul className="checkBoxList">
                {
                    roles.map((role, index) => (
                        <li key = {index}>
                            <input type="checkbox" value="mahedee"></input>
                            <span class="input-group-addon">&nbsp;</span>
                            <label>{role.roleName}</label>
                        </li>
                    ))
                }
                </ul>
            </div>
        );
    }

    render() {
        let content = this.state.loading ? (
            <p>
                <em>Loading...</em>
            </p>
        ) : (
            this.renderAllRoles(this.state.roles)
        )
        {
            console.log('Roles data: ');
            console.log(this.state.roles)
        }
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

                {/* <button onClick={() => this.onUserCreate()} className="btn btn-primary">Create new user</button> */}
                {content}

                {/* {renderRoleList()} */}
            
                <RoleList roles = {this.state.roles} userRoles = {this.state.userRoles}/>
            </div>

            
        );
    }
}