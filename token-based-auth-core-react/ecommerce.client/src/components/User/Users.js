import { Component } from "react";
import { getData } from "../services/AccessAPI";

export default class Users extends Component {
    constructor(props) {
        super(props);
        this.state = {
            users: [],
            loading: true
        };

        this.onUserCreate = this.onUserCreate.bind(this);
        this.onUserDelete = this.onUserDelete.bind(this);
    }


    componentDidMount() {
        this.getAllUsersData();
    }

    onUserCreate(){
        const{history} = this.props;
        history.push('/admin/user/create');
    }


    onUserEdit(id){
        const { history } = this.props;
        history.push('/admin/user/edit/' + id);
    }

    onUserDelete(id){
        const {history} = this.props;
        history.push('/admin/user/delete/' + id);
    }

    getAllUsersData() {
        getData('api/User/GetAll').then(
            (result) => {
                if (result) {
                    this.setState({
                        users: result,
                        loading: false
                    });
                }
            }
        );

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
                                    <button onClick={() => this.onUserDelete(user.id)} className="btn btn-danger">Delete</button></td>
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
                <button onClick={() => this.onUserCreate()} className="btn btn-primary">Create new user</button>
                {content}
            </div>
        );
    }
}