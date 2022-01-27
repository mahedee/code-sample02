import axios from "axios";
import React, { Component } from "react";

export class Delete extends Component {
    constructor(props) {
        super(props);

        this.onCancel = this.onCancel.bind(this);
        this.onConfirmation = this.onConfirmation.bind(this);

        this.state = {
            fullName: '',
            contactNumber: '',
            nomineeName: '',
            nomineeContactNumber: '',
            nomineeDateOfBirth: null
        }
    }

    componentDidMount() {
        const { id } = this.props.match.params;
        axios.get("http://localhost:8001/api/Customers/" + id).then(customer => {
            const response = customer.data;
            this.setState({
                id: response.id,
                fullName: response.fullName,
                contactNumber: response.contactNumber,
                nomineeName: response.nomineeName,
                nomineeContactNumber: response.nomineeContactNumber,
                nomineeDateOfBirth: new Date(response.nomineeDateOfBirth).toISOString().slice(0, 10)
            })
        })
    }

    onCancel() {
        const { history } = this.props;
        history.push('/banking/customers');
    }

    onConfirmation(e) {
        e.preventDefault();

        const { id } = this.props.match.params;
        const { history } = this.props;

        axios.delete("http://localhost:8001/api/Customers/" + id).then(result => {
            history.push('/banking/customers');
        })

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
                            Full Name:
                        </dt>
                        <dd class="col-sm-10">
                            {this.state.fullName}
                        </dd>
                        <dt class="col-sm-2">
                            Contact Number:
                        </dt>
                        <dd class="col-sm-10">
                            {this.state.contactNumber}
                        </dd>
                        <dt class="col-sm-2">
                            Nominee Name:
                        </dt>
                        <dd class="col-sm-10">
                            {this.state.nomineeName}
                        </dd>
                        <dt class="col-sm-2">
                            Nominee's Contact Number:
                        </dt>
                        <dd class="col-sm-10">
                            {this.state.nomineeContactNumber}
                        </dd>

                        <dt class="col-sm-2">
                           Nominee's DOB:
                        </dt>
                        <dd class="col-sm-10">
                            {this.state.nomineeDateOfBirth}
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