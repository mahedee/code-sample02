import React, { Component } from "react";
import { getData, putData } from "../services/AccessAPI";

export class Edit extends Component {
    constructor(props) {
        super(props);

        this.onChangeFirstName = this.onChangeFirstName.bind(this);
        this.onChangeLastName = this.onChangeLastName.bind(this);
        this.onChangeEmail = this.onChangeEmail.bind(this);
        this.onChangeContactNumber = this.onChangeContactNumber.bind(this);
        this.onChangeAddress = this.onChangeAddress.bind(this);
        this.onSubmit = this.onSubmit.bind(this);

        this.state = {
            id: '',
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

    onChangeFirstName(e) {
        this.setState({
            firstName: e.target.value
        });
    }

    onChangeLastName(e) {
        this.setState({
            lastName: e.target.value
        });
    }

    onChangeEmail(e) {
        this.setState({
            email: e.target.value
        });
    }

    onChangeContactNumber(e) {
        this.setState({
            contactNumber: e.target.value
        });

    }

    onChangeAddress(e) {
        this.setState({
            address: e.target.value
        });
    }


    onUpdateCancel() {
        const { history } = this.props;
        history.push('/banking/customers');
    }

    onSubmit(e) {

        e.preventDefault();
        const { history } = this.props;
        const { id } = this.props.match.params;
        let customerObj = {
            id: this.state.id,
            firstName: this.state.firstName,
            lastName: this.state.lastName,
            contactNumber: this.state.contactNumber,
            email: this.state.email,
            address: this.state.address
        }

        putData('api/Customer/Edit/' + id, customerObj).then((result) => {
            let responseJson = result;
            if (responseJson) {
                console.log(responseJson);
                history.push('/banking/customers');
            }
        }

        );
    }

    render() {
        return (
            <div className="row">
                <div className="col-md-4">
                    <h3>Edit Customer</h3>
                    <form onSubmit={this.onSubmit}>
                        <div className="form-group">
                            <label className="control-label">First Name: </label>
                            <input className="form-control" type="text" value={this.state.firstName} onChange={this.onChangeFirstName}></input>
                        </div>

                        <div className="form-group">
                            <label className="control-label">Last Name: </label>
                            <input className="form-control" type="text" value={this.state.lastName} onChange={this.onChangeLastName}></input>
                        </div>

                        <div className="form-group">
                            <label className="control-label">Email: </label>
                            <input className="form-control" type="text" value={this.state.email} onChange={this.onChangeEmail}></input>
                        </div>

                        <div className="form-group">
                            <label className="control-label">Contact Number: </label>
                            <input className="form-control" type="text" value={this.state.contactNumber} onChange={this.onChangeContactNumber}></input>
                        </div>

                        <div className="form-group">
                            <label className="control-label">Address: </label>
                            <input className="form-control" type="text" value={this.state.address} onChange={this.onChangeAddress}></input>
                        </div>

                        <div className="form-group">
                            <button onClick={this.onUpdateCancel} className="btn btn-default">Cancel</button>
                            <input type="submit" value="Edit" className="btn btn-primary"></input>
                        </div>

                    </form>

                </div>
            </div>
        )
    }
}