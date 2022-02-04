import React, { Component } from "react";
import { postData } from "../services/AccessAPI";

export class Create extends Component {

    constructor(props) {
        super(props);
        this.state = {
            firstName: '',
            lastName: '',
            email: '',
            contactNumber: '',
            address: ''
        }

        this.onChange = this.onChange.bind(this);
        this.onSubmit = this.onSubmit.bind(this);
    }

    onChange(e) {
        this.setState({ [e.target.name]: e.target.value });
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


        postData('api/Customer/Create', customerObj).then((result) => {
            let responseJson = result;
            if (responseJson) {
                history.push('/banking/customers');
            }
        });
    }

    render() {
        return (
            <div className="row">
                <div className="col-md-4">
                    <h3>Add new customer</h3>
                    <form onSubmit={this.onSubmit}>
                        <div className="form-group">
                            <label className="control-label">First Name: </label>
                            <input className="form-control" type="text" name="firstName" value={this.state.firstName} onChange={this.onChange}></input>
                        </div>

                        <div className="form-group">
                            <label className="control-label">Last Name: </label>
                            <input className="form-control" type="text" name="lastName" value={this.state.lastName} onChange={this.onChange}></input>
                        </div>

                        <div className="form-group">
                            <label className="control-label">Email: </label>
                            <input className="form-control" type="text" name="email" value={this.state.email} onChange={this.onChange}></input>
                        </div>

                        <div className="form-group">
                            <label className="control-label">Contact Number: </label>
                            <input className="form-control" type="text" name="contactNumber" value={this.state.contactNumber} onChange={this.onChange}></input>
                        </div>

                        <div className="form-group">
                            <label className="control-label">Address:  </label>
                            <input className="form-control" type="text" name="address" value={this.state.address} onChange={this.onChange}></input>
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