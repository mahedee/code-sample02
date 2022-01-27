import React, { Component } from "react";
import axios from "axios";

export class CreateCustomer extends Component {

    constructor(props) {
        super(props);

        this.onChangeName = this.onChangeName.bind(this);
        this.onChangeDesignation = this.onChangeDesignation.bind(this);
        this.onChangeFathersName = this.onChangeFathersName.bind(this);
        this.onChangeMothersName = this.onChangeMothersName.bind(this);
        this.onChangeDOB = this.onChangeDOB.bind(this);
        this.onSubmit = this.onSubmit.bind(this);


        this.state = {
            name: '',
            designation: '',
            fathersName: '',
            mothersName: '',
            //This is date time object
            dateOfBirth: null
        }
    }

    onChangeName(e) {
        this.setState({
            name: e.target.value
        })
    }

    onChangeDesignation(e) {
        this.setState({
            designation: e.target.value
        })
    }

    onChangeFathersName(e) {
        this.setState({
            fathersName: e.target.value
        })

    }

    onChangeMothersName(e) {
        this.setState({
            mothersName: e.target.value
        })

    }

    onChangeDOB(e) {
        this.setState({
            dateOfBirth: e.target.value
        })

    }

    onSubmit(e) {
        e.preventDefault();
        const { history } = this.props;

        let employeeObj = {
            name: this.state.name,
            designation: this.state.designation,
            fathersName: this.state.fathersName,
            mothersName: this.state.mothersName,
            dateOfBirth: this.state.dateOfBirth
        }

        axios.post("http://localhost:8003/api/Employees/AddEmployee", employeeObj).then(result => {
            history.push('/employees');
        })
    }

    render() {
        return (
            <div className="row">
                <div className="col-md-4">
                    <h3>Add new employee</h3>
                    <form onSubmit={this.onSubmit}>
                        <div className="form-group">
                            <label className="control-label">Name: </label>
                            <input className="form-control" type="text" value={this.state.name} onChange={this.onChangeName}></input>
                        </div>

                        <div className="form-group">
                            <label className="control-label">Designation: </label>
                            <input className="form-control" type="text" value={this.state.designation} onChange={this.onChangeDesignation}></input>
                        </div>

                        <div className="form-group">
                            <label className="control-label">Father's Name: </label>
                            <input className="form-control" type="text" value={this.state.fathersName} onChange={this.onChangeFathersName}></input>
                        </div>

                        <div className="form-group">
                            <label className="control-label">Mother's Name: </label>
                            <input className="form-control" type="text" value={this.state.mothersName} onChange={this.onChangeMothersName}></input>
                        </div>

                        <div className="form-group">
                            <label className="control-label">Date of Birth: </label>
                            <input className="form-control" type="date" value={this.state.dateOfBirth} onChange={this.onChangeDOB}></input>
                        </div>

                        <div className="form-group">
                            <input type="submit" value="Add Employee" className="btn btn-primary"></input>
                        </div>

                    </form>

                </div>
            </div>
        )
    }

}