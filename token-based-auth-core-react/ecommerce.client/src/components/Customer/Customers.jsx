import React, { Component } from 'react';
import { getData } from '../services/AccessAPI';

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

        getData(`api/Customer/getall`).then(
            (result) => {
                let responseJson = result;
                if (responseJson) {
                    this.setState({
                        customers: responseJson,
                        loading: false
                    });
                }
            }
        );
    }

    renderAllCustomersTable(customers) {
        return (
            <table className="table table-striped">
                <thead>
                    <tr>
                        <th>First Name</th>
                        <th>Last Name</th>
                        <th>Email</th>
                        <th>Contact Number</th>
                        <th>Address</th>
                        <th>Actions</th>
                    </tr>
                </thead>
                <tbody>
                    {
                        customers.map(customer => (
                            <tr key={customer.id}>
                                <td>{customer.firstName}</td>
                                <td>{customer.lastName}</td>
                                <td>{customer.email}</td>
                                <td>{customer.contactNumber}</td>
                                <td>{customer.address}</td>
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