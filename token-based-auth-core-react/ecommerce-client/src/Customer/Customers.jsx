import axios from "axios";
import { Component } from "react";

export class Customers extends Component
{
    constructor(props){
        super(props);

        this.state = {
            customers: [],
            loading: true,
            failder: false,
            error: ''
        }
    }

    populateCustomersData(){
        axios.get("api/Employees/GetEmployees").then(result => {
            const response = result.data;
            this.setState({customers: response, loading: false, error: ""});
        }).catch(error => {
            this.setState({customers: [], loading: false, failed: true, error: "Customers could not be loaded!"});
        });
    }


}