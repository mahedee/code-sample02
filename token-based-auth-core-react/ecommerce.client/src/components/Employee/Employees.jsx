import React, { Component } from 'react';
import axios from 'axios';

// export keyword is a new feature in ES6 let export your functions , 
// variables so you can get access to them in other js files

export class Employees extends Component {
    //“Props” is a special keyword in React, which stands for properties and is being used for passing data from one component to another.
    constructor(props) {

        //If you do not call super(props) method, this. props will be undefined 
        super(props);

        this.OnEmployeeEdit = this.OnEmployeeEdit.bind(this);
        this.OnEmployeeDelete = this.OnEmployeeDelete.bind(this);
        this.onEmployeeCreate = this.onEmployeeCreate.bind(this);

        this.state = {
            employees: [],
            loading: true,
            failed: false,
            error: ''
        }
    }

    /*Lifecycle Method: The componentDidMount() method runs after 
    the component output has been rendered to the DOM.*/

    componentDidMount() {
        this.populateEmployeesData();
    }

    // Event handler for create button
    onEmployeeCreate() {
        const { history } = this.props;
        history.push('/create');
    }

    // Event handler for edit button
    OnEmployeeEdit(id) {
        const { history } = this.props;
        history.push('/edit/' + id);
    }

    // Event handler for delete button
    OnEmployeeDelete(id) {
        const { history } = this.props;
        history.push('/delete/' + id);
    }

    populateEmployeesData() {
        // Axios is a library that helps us make http requests to external resources
        axios.get("http://localhost:8003/api/Employees/GetEmployees").then(result => {
            const response = result.data;
            this.setState({ employees: response, loading: false, error: "" });
        }).catch(error => {
            this.setState({ employees: [], loading: false, failed: true, error: "Employess could not be loaded!" });
        });
    }

    renderAllEmployeeTable(employees) {
        return (
            <table className="table table-striped">
                <thead>
                    <tr>
                        <th>Name</th>
                        <th>Designation</th>
                        <th>Father's Name</th>
                        <th>Mother's Name</th>
                        <th>Date of Birth</th>
                        <th>Actions</th>
                    </tr>
                </thead>
                <tbody>
                    {
                        employees.map(employee => (
                            <tr key={employee.id}>
                                <td>{employee.name}</td>
                                <td>{employee.designation}</td>
                                <td>{employee.fathersName}</td>
                                <td>{employee.mothersName}</td>
                                <td>{new Date(employee.dateOfBirth).toISOString().slice(0, 10)}</td>
                                <td><button onClick={() => this.OnEmployeeEdit(employee.id)} className="btn btn-success">Edit</button> ||
                                <button onClick={() => this.OnEmployeeDelete(employee.id)} className="btn btn-danger">Delete</button></td>
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
                this.renderAllEmployeeTable(this.state.employees)
            )

        return (
            <div>
                <h2>Employee</h2>
                <button onClick={() => this.onEmployeeCreate()} className="btn btn-primary">Create</button>
                {content}
            </div>
        );
    }

}