import React, { Component } from "react";
import { deleteData, getData } from "../services/AccessAPI";

export class Delete extends Component {
    constructor(props) {
        super(props);

        this.onCancel = this.onCancel.bind(this);
        this.onConfirmation = this.onConfirmation.bind(this);

        this.state = {
            firstName: '',
            lastName: '',
            email: '',
            contactNumber: '',
            address: ''
        }
    }

    componentDidMount() {
        const { id } = this.props.match.params;
        this.getCustomer(id);
    }

    getCustomer(id) {
        getData('api/Customer/' + id).then(
            (result) => {
                if (result) {
                    this.setState({
                        id: result.id,
                        firstName: result.firstName,
                        lastName: result.lastName,
                        email: result.email,
                        contactNumber: result.contactNumber,
                        address: result.address
                        //loading: false
                    });
                }
            }
        );
    }

    onCancel() {
        const { history } = this.props;
        history.push('/banking/customers');
    }

    onConfirmation(e) {
        e.preventDefault();

        const { id } = this.props.match.params;
        const { history } = this.props;

        deleteData('api/Customer/Delete/' + id).then((result) => {
            let responseJson = result;
            if (responseJson) {
                history.push('/banking/customers');
            }
        }
        );
    }


    render() {
        return (
            <div>
                <h2>Delete</h2>
                <h3>Are you sure you want to delete this?</h3>
                <div>
                    <h4>Customer</h4>
                    <dl class="row">
                        <dt class="col-sm-2">
                            First Name:
                        </dt>
                        <dd class="col-sm-10">
                            {this.state.firstName}
                        </dd>
                        <dt class="col-sm-2">
                            Last Name:
                        </dt>
                        <dd class="col-sm-10">
                            {this.state.lastName}
                        </dd>
                        <dt class="col-sm-2">
                            Email:
                        </dt>
                        <dd class="col-sm-10">
                            {this.state.email}
                        </dd>
                        <dt class="col-sm-2">
                            Contact Number:
                        </dt>
                        <dd class="col-sm-10">
                            {this.state.contactNumber}
                        </dd>

                        <dt class="col-sm-2">
                            Address:
                        </dt>
                        <dd class="col-sm-10">
                            {this.state.address}
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