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
            // id: this.state.id,
            // fullName: this.state.fullName,
            // email: this.state.email,
            userName: this.state.userName,
            roles: this.state.userRoles
        }

        putData('api/User/EditUserRoles', userRoles).then((result) => {
            let responseJson = result;
            //console.log("update response: ");
            
            if(responseJson){
                console.log(responseJson);
                //history.push('/admin/roles');
            }
        }

        );

        alert('Save all info');

    }

    handleCheckboxChange = (event) => {
    //handleCheckboxChange(e){
        alert('Checkbox event');
        if(event.target.checked){
            ///event.target.checked = true;
            alert('checked');
            alert(event.target.value);

            if(!this.state.userRoles.includes(event.target.value)){
                this.setState(prevState => ({userRoles: [...prevState.userRoles, event.target.value]}));
                //alert(this.state.userRoles);
            }
        } else{
            alert('uncheked')
            this.setState(prevState => ({userRoles: prevState.userRoles.filter(roleName => roleName !== event.target.value)}));
        }

    }



    onChange(e) {
        this.setState({ [e.target.name]: e.target.value });
    }

    componentDidMount() {
        this.getAllRoles();
    }


    onSearch(userName) {

        alert(userName);
        // this.renderRoleList();

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

        this.renderRoleList();
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
                            <li key={index}>
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

    renderRoleList() {

        // console.log('user roles');
        // console.log(this.state.userRoles);
        return (
            <RoleList roles={this.state.roles} userRoles={this.state.userRoles} onChange={this.handleCheckboxChange} />
        );
    };


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

                {/* <button onClick={() => this.onUserCreate()} className="btn btn-primary">Create new user</button> */}
                {/* {content} */}

                {/* {renderCheckbox} */}

                {/* <RoleList roles = {this.state.roles} userRoles = {this.state.userRoles}/> */}

                <form onSubmit={this.onSubmit}>
                    <div className="form-group">
                        {renderCheckbox}
                    </div>
                    <div className="form-group">
                        <input type="submit" value="Save" className="btn btn-primary"></input>
                    </div>

                </form>

            </div>


        );
    }
}