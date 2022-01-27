//let BaseURL = window.SERVER_URL;

import axios from "axios";

export function getData(endPoint)
{
    let baseURL = "https://localhost:7142/";
    //return axios.get(baseURL+endPoint);

    var response = axios.get("https://localhost:7142/api/Customer");
    return response.data;
    // .then(result => {
    // return result.data;
    // //return response;
    // //this.setState({ customers: response, loading: false, error: "" });
    // }).catch(error => {

    //     return error;
    //     //this.setState({ customers: [], loading: false, failed: true, error: "Customers could not be loaded!" });
    // });

    console.log("Log: Base URL : " + baseURL);
}

