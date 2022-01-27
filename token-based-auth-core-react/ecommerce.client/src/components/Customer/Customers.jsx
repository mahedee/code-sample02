import React, { Component } from 'react';
import axios from 'axios';

// export keyword is a new feature in ES6 let export your functions , 
// variables so you can get access to them in other js files

export class Customers extends Component {
    //“Props” is a special keyword in React, which stands for properties and is being used for passing data from one component to another.
    constructor(props) {

        //If you do not call super(props) method, this. props will be undefined 
        super(props);

        this.OncustomerEdit = this.OncustomerEdit.bind(this);
        this.OncustomerDelete = this.OncustomerDelete.bind(this);
        this.onCustomerCreate = this.onCustomerCreate.bind(this);

        this.state = {
            customers: [],
            loading: true,
            failed: false,
            error: ''
        }
    }

    /*Lifecycle Method: The componentDidMount() method runs after 
    the component output has been rendered to the DOM.*/

    componentDidMount() {
        this.populateCustomersData();
    }

    // Event handler for create button
    onCustomerCreate() {
        const { history } = this.props;
        history.push('/banking/customer/create');
    }

    // Event handler for edit button
    OncustomerEdit(id) {
        const { history } = this.props;
        history.push('/banking/customer/edit/' + id);
    }

    // Event handler for delete button
    OncustomerDelete(id) {
        const { history } = this.props;
        history.push('/banking/customer/delete/' + id);
    }

    populateCustomersData() {
        // Axios is a library that helps us make http requests to external resources
        axios.get("http://localhost:8001/api/Customers").then(result => {
            const response = result.data;
            this.setState({ customers: response, loading: false, error: "" });
        }).catch(error => {
            this.setState({ customers: [], loading: false, failed: true, error: "Customers could not be loaded!" });
        });
    }

    renderAllCustomersTable(customers) {
        return (
            <table className="table table-striped">
                <thead>
                    <tr>
                        <th>Full Name</th>
                        <th>Contact Number</th>
                        <th>Nominee Name</th>
                        <th>Nominee's Contact Number</th>
                        <th>Nominee's DOB</th>
                        <th>Actions</th>
                    </tr>
                </thead>
                <tbody>
                    {
                        customers.map(customer => (
                            <tr key={customer.id}>
                                <td>{customer.fullName}</td>
                                <td>{customer.contactNumber}</td>
                                <td>{customer.nomineeName}</td>
                                <td>{customer.nomineeContactNumber}</td>
                                <td>{new Date(customer.nomineeDateOfBirth).toISOString().slice(0, 10)}</td>
                                <td><button onClick={() => this.OncustomerEdit(customer.id)} className="btn btn-success">Edit</button> ||
                                <button onClick={() => this.OncustomerDelete(customer.id)} className="btn btn-danger">Delete</button></td>
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
                this.renderAllCustomersTable(this.state.customers)
            )

        return (
            <div>
                <h2>Customer</h2>
                <button onClick={() => this.onCustomerCreate()} className="btn btn-primary">Create</button>
                {content}
            </div>
        );
    }

}