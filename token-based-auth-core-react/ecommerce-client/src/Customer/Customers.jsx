import axios from "axios";
import { Component } from "react";

export class Customers extends Component
{
    constructor(props){
        super(props);
        this.onCustomerCreate = this.onCustomerCreate.bind(this);

        this.state = {
            customers: [],
            loading: true,
            failder: false,
            error: ''
        }
    }

    // Event handler for create button
    onCustomerCreate()
    {
        const {history} = this.props;
        history.push('/create');
    }

    populateCustomersData(){
        axios.get("api/Employees/GetEmployees").then(result => {
            const response = result.data;
            this.setState({customers: response, loading: false, error: ""});
        }).catch(error => {
            this.setState({customers: [], loading: false, failed: true, error: "Customers could not be loaded!"});
        });
    }


    renderAllCustomerTable(employees){
        return(
            <table className="table table-striped">
                <thead>
                    <tr>
                        <th>First Name</th>
                        <th>Last Name</th>
                        <th>Email</th>
                        {/* <th>Mother's Name</th>
                        <th>Date of Birth</th>
                        <th>Actions</th> */}
                    </tr>
                </thead>
                <tbody>
                    {
                        employees.map(employee => (
                            <tr key={employee.id}>
                                <td>{employee.firstname}</td>
                                <td>{employee.lastname}</td>
                                <td>{employee.email}</td>
                                {/* <td>{employee.fathersName}</td>
                                <td>{employee.mothersName}</td>
                                <td>{ new Date(employee.dateOfBirth).toISOString().slice(0,10)}</td>
                                <td><button onClick={()=> this.OnEmployeeEdit(employee.id)}  className= "btn btn-success">Edit</button> || 
                                <button onClick={()=> this.OnEmployeeDelete(employee.id)} className= "btn btn-danger">Delete</button></td> */}
                            </tr>
                        ))
                    }
                </tbody>
            </table>
        );
    }


    render(){

        let content = this.state.loading ? (
            <p>
                <em>Loading...</em>
            </p>
        ):(
            this.renderAllCustomerTable(this.state.customers)
        )

        return(
            <div>
                <h2>Customers</h2>
                <button onClick={()=> this.onCustomerCreate()} className="btn btn-primary">Create</button>
                {content}
            </div>
        );
    }


}


// https://blog.telexarsoftware.com/integrating-a-bootstrap-template-to-a-reactjs-application/