import React, { Component } from "react";
import { deleteData, getData } from "../services/AccessAPI";

export class DeleteRole extends Component {
    constructor(props) {
        super(props);

        this.onCancel = this.onCancel.bind(this);
        this.onConfirmation = this.onConfirmation.bind(this);

        this.state = {
            roleName: '',
            loading: true
        }
    }

    componentDidMount() {
        const { id } = this.props.match.params;

        getData('api/Role/' + id).then(
            (result) => {
                console.log("Role for edit: ");
                console.log(result);
                if (result) {
                    this.setState({
                        id: result.id,
                        roleName: result.roleName,
                        loading: false
                    });
                }
            }
        );
    }

    onCancel() {
        const { history } = this.props;
        history.push('/admin/roles');
    }

    onConfirmation(e) {
        e.preventDefault();

        const { id } = this.props.match.params;
        const { history } = this.props;

        deleteData('api/Role/Delete/' + id).then((result) => {
            let responseJson = result;
            if (responseJson) {
                console.log(responseJson);
                history.push('/admin/roles');
            }
        }
        );

    }


    render() {
        return (
            <div>
                <h2>::Delete role::</h2>
                <h3>Are you sure you want to delete this?</h3>
                <div>
                    <h4>Role Information</h4>
                    <dl class="row">
                        <dt class="col-sm-2">
                            Role Name:
                        </dt>
                        <dd class="col-sm-10">
                            {this.state.roleName}
                        </dd>
                    </dl>

                    <form onSubmit={this.onConfirmation}>
                        <input type="hidden" asp-for="Id" />
                        <button type="submit" class="btn btn-danger">Delete</button> |
                        <button onClick={this.onCancel} className="btn btn-primary">Back to List</button>
                    </form>
                </div>
            </div>
        )
    }
}