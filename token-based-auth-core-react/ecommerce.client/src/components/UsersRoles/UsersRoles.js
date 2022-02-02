import { Component } from "react";
import { getData } from "../services/AccessAPI";

export default class UsersRole extends Component {
    constructor(props) {
        super(props);
        this.state = {
            roles: [],
            loading: true
        };

        this.onUserCreate = this.onUserCreate.bind(this);
        this.onUserDelete = this.onUserDelete.bind(this);
        this.onSearch = this.onSearch.bind(this);
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
        getData('api/Role/GetAll/' + userName).then(
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

    getAllRoles() {
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
                    roles.map(role => (
                        <li>
                            <input type="checkbox" value="mahedee"></input>
                            <span class="input-group-addon">&nbsp;</span>
                            <label>{role.roleName}</label>
                        </li>
                    ))
                }
                </ul>
            </div>

            // <table className="table table-striped">
            //     <thead>
            //         <tr>
            //             <th>Full Name</th>
            //             <th>User Name</th>
            //             <th>Email</th>
            //             <th>Actions</th>
            //         </tr>
            //     </thead>
            //     <tbody>
            //         {
            //             users.map(user => (
            //                 <tr key={user.id}>
            //                     <td>{user.fullName}</td>
            //                     <td>{user.userName}</td>
            //                     <td>{user.email}</td>
            //                     <td><button onClick={() => this.onUserEdit(user.id)} className="btn btn-success">Edit</button> ||
            //                         <button onClick={() => this.onUserDelete(user.id)} className="btn btn-danger">Delete</button></td>
            //                 </tr>
            //             ))
            //         }
            //     </tbody>
            // </table>
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

        return (
            <div>
                <h3>Users Role</h3>
                <div className="input-group">
                    <input className="col-md-3" type="text" name="fullName" placeholder="Enter user name" value={this.state.fullName} onChange={this.onChange}></input>
                    <span class="input-group-addon">&nbsp;</span>
                    <button className="btn btn-primary" onClick={this.registration}>
                        Search
                    </button>
                </div>

                {/* <button onClick={() => this.onUserCreate()} className="btn btn-primary">Create new user</button> */}
                {content}
            </div>
        );
    }
}