import axios from "axios";
import React, { Component } from "react";

export class Edit extends Component {
    constructor(props) {
        super(props);

        this.onChangeFullName = this.onChangeFullName.bind(this);
        this.onChangeContactNumber = this.onChangeContactNumber.bind(this);
        this.onChangeNomineeName = this.onChangeNomineeName.bind(this);
        this.onChangeNomineeContactNumber = this.onChangeNomineeContactNumber.bind(this);
        this.onChangeNomineeDOB = this.onChangeNomineeDOB.bind(this);
        this.onSubmit = this.onSubmit.bind(this);

        this.state = {
            id: '',
            fullName: '',
            contactNumber: '',
            nomineeName: '',
            nomineeContactNumber: '',
            //This is date time object
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
        //alert(id);
    }

    onChangeFullName(e) {
        this.setState({
            fullName: e.target.value
        });
    }

    onChangeContactNumber(e) {
        this.setState({
            contactNumber: e.target.value
        });
    }

    onChangeNomineeName(e) {
        this.setState({
            nomineeName: e.target.value
        });

    }

    onChangeNomineeContactNumber(e) {
        this.setState({
            nomineeContactNumber: e.target.value
        });

    }

    onChangeNomineeDOB(e) {
        this.setState({
            nomineeDateOfBirth: e.target.value
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
            fullName: this.state.fullName,
            contactNumber: this.state.contactNumber,
            nomineeName: this.state.nomineeName,
            nomineeContactNumber: this.state.nomineeContactNumber,
            nomineeDateOfBirth: new Date(this.state.nomineeDateOfBirth).toISOString()
        }

        axios.put("http://localhost:8001/api/Customers/" + id, customerObj).then(result => {
            history.push('/banking/customers');
        })
    }

    render() {
        return (
            <div className="row">
                <div className="col-md-4">
                    <h3>Edit Customer</h3>
                    <form onSubmit={this.onSubmit}>
                        <div className="form-group">
                            <label className="control-label">Full Name: </label>
                            <input className="form-control" type="text" value={this.state.fullName} onChange={this.onChangeFullName}></input>
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
                            <label className="control-label">Nominee's Contact Number: </label>
                            <input className="form-control" type="text" value={this.state.nomineeContactNumber} onChange={this.onChangeNomineeContactNumber}></input>
                        </div>

                        <div className="form-group">
                            <label className="control-label">Nominee's DOB: </label>
                            <input className="form-control" type="date" value={this.state.nomineeDateOfBirth} onChange={this.onChangeNomineeDOB}></input>
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