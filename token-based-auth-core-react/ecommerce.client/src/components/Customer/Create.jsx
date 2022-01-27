import React, { Component } from "react";
import axios from "axios";

export class Create extends Component {

    constructor(props) {
        super(props);

        this.onChangeFullName = this.onChangeFullName.bind(this);
        this.onChangeContactNumber = this.onChangeContactNumber.bind(this);
        this.onChangeNomineeName = this.onChangeNomineeName.bind(this);
        this.onChangeNomineeContactNumber = this.onChangeNomineeContactNumber.bind(this);
        this.onChangeNomineeDOB = this.onChangeNomineeDOB.bind(this);
        this.onSubmit = this.onSubmit.bind(this);


        this.state = {
            firstName: '',
            lastName: '',
            email: '',
            contactNumber: '',
            //This is date time object
            address: ''
        }
    }

    onChangeFullName(e) {
        this.setState({
            fullName: e.target.value
        })
    }

    onChangeContactNumber(e) {
        this.setState({
            contactNumber: e.target.value
        })
    }

    onChangeNomineeName(e) {
        this.setState({
            nomineeName: e.target.value
        })

    }

    onChangeNomineeContactNumber(e) {
        this.setState({
            nomineeContactNumber: e.target.value
        })

    }

    onChangeNomineeDOB(e) {
        this.setState({
            nomineeDateOfBirth: e.target.value
        })

    }

    onSubmit(e) {
        e.preventDefault();
        const { history } = this.props;

        let customerObj = {
            firstName: this.state.firstName,
            lastName: this.state.lastName,
            email: this.state.email,
            contactNumber: this.state.contactNumber,
            address: this.state.address
        }

        axios.post("https://localhost:7142/api/Customer", customerObj).then(result => {
            history.push('/banking/customers');
        })
    }

    render() {
        return (
            <div className="row">
                <div className="col-md-4">
                    <h3>Add new customer</h3>
                    <form onSubmit={this.onSubmit}>
                        <div className="form-group">
                            <label className="control-label">First Name: </label>
                            <input className="form-control" type="text" value={this.state.firstName} onChange={this.onChangeFullName}></input>
                        </div>

                        <div className="form-group">
                            <label className="control-label">Contact Number: </label>
                            <input className="form-control" type="text" value={this.state.contactNumber} onChange={this.onChangeContactNumber}></input>
                        </div>

                        <div className="form-group">
                            <label className="control-label">Nominee Name: </label>
                            <input className="form-control" type="text" value={this.state.nomineeName} onChange={this.onChangeNomineeName}></input>
                        </div>

                        <div className="form-group">
                            <label className="control-label">Nominee Contact Number: </label>
                            <input className="form-control" type="text" value={this.state.nomineeContactNumber} onChange={this.onChangeNomineeContactNumber}></input>
                        </div>

                        <div className="form-group">
                            <label className="control-label">Nominee's DOB: </label>
                            <input className="form-control" type="date" value={this.state.nomineeDateOfBirth} onChange={this.onChangeNomineeDOB}></input>
                        </div>

                        <div className="form-group">
                            <input type="submit" value="Add Customer" className="btn btn-primary"></input>
                        </div>

                    </form>

                </div>
            </div>
        )
    }

}