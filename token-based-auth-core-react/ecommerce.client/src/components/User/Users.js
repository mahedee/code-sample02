import { Component } from "react";
import { getData } from "../services/AccessAPI";

export default class Users extends Component {
    constructor(props) {
        super(props);
        this.state = {
            users: [],
            loading: true
        };
    }


    componentDidMount() {
        this.getAllUsersData();
    }

    onUserEdit(id){
        const { history } = this.props;
        history.push('/admin/user/edit/' + id);
    }
    getAllUsersData() {
        getData('api/User/GetAll').then(
            (result) => {
                //let responseJson = result;
                //console.log("users list: ");
                //console.log(result);
                if (result) {
                    this.setState({
                        users: result,
                        loading: false
                    });
                }
            }
        );

        //console.log("users list: " + this.state.users);
    }

    renderAllUsersTable(users) {
        return (
            <table className="table table-striped">
                <thead>
                    <tr>
                        <th>Full Name</th>
                        <th>User Name</th>
                        <th>Email</th>
                        <th>Actions</th>
                    </tr>
                </thead>
                <tbody>
                    {
                        users.map(user => (
                            <tr key={user.id}>
                                <td>{user.fullName}</td>
                                <td>{user.userName}</td>
                                <td>{user.email}</td>
                                <td><button onClick={() => this.onUserEdit(user.id)} className="btn btn-success">Edit</button> ||
                                    <button onClick={() => this.OncustomerDelete(user.id)} className="btn btn-danger">Delete</button></td>
                            </tr>
                        ))
                    }
                </tbody>
            </table>
        );
    }

    render() {
        let content = this.state.loading ? (
            <p>
                <em>Loading...</em>
            </p>
        ) : (
            this.renderAllUsersTable(this.state.users)
        )

        return (
            <div>
                <h3>List of Users</h3>
                <button onClick={() => this.onCustomerCreate()} className="btn btn-primary">Create new user</button>
                {content}
            </div>
        );
    }
}