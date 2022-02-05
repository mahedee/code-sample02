import { Component } from "react";
import { getData } from "../services/AccessAPI";

export default class Roles extends Component {
    constructor(props) {
        super(props);
        this.state = {
            roles: [],
            loading: true
        };

        this.onRoleCreate = this.onRoleCreate.bind(this);
        this.onRoleEdit = this.onRoleEdit.bind(this);
        this.onRoleDelete = this.onRoleDelete.bind(this);

    }

    onRoleCreate(){
        const { history } = this.props;
        history.push('/admin/role/create');
    }

    onRoleEdit(id){
        const {history} = this.props;
        history.push('/admin/role/edit/' + id);
    }

    onRoleDelete(id){
        const{history} = this.props;
        history.push('/admin/role/delete/' + id);
    }

    componentDidMount() {
        this.getAllRoles();
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
    }

    populateRolesTable(roles) {
        return (
            <table className="table table-striped">
                <thead>
                    <tr>
                        <th>Roles</th>
                        <th>Actions</th>
                    </tr>
                </thead>
                <tbody>
                    {
                        roles.map(role => (
                            <tr key={role.id}>
                                <td>{role.roleName}</td>
                                <td><button onClick={() => this.onRoleEdit(role.id)} className="btn btn-success">Edit</button> ||
                                    <button onClick={() => this.onRoleDelete(role.id)} className="btn btn-danger">Delete</button></td>
                            </tr>
                        ))
                    }
                </tbody>
            </table>
        );
    }

    render() {
        let contnet = this.state.loading ? (
            <p>
                <em>Loading ... </em>
            </p>

        ) : (
            this.populateRolesTable(this.state.roles)
        )
        return (
            <div>
                <h4>List of roles</h4>
                <button onClick={() => this.onRoleCreate()} className="btn btn-primary">Create new role</button>
                {contnet}
            </div>
        );
    }
}