// //let BaseURL = window.SERVER_URL;

import SessionManager from "../Auth/SessionManager";

// import axios from "axios";

// export function getData(endPoint)
// {
//     let baseURL = "https://localhost:7142/";
//     //return axios.get(baseURL+endPoint);

//     var response = axios.get("https://localhost:7142/api/Customer");
//     return response.data;

//     try{
//         const response = axios.get();
//         return response

//     } catch(err){
//         throw err;
//     }
//     // .then(result => {
//     // return result.data;
//     // //return response;
//     // //this.setState({ customers: response, loading: false, error: "" });
//     // }).catch(error => {

//     //     return error;
//     //     //this.setState({ customers: [], loading: false, failed: true, error: "Customers could not be loaded!" });
//     // });

//     console.log("Log: Base URL : " + baseURL);
// }

export function getData(type) {
    //let token=Auth.getToken();
    //let token='eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJtYWhlZGVlIiwianRpIjoiNDY2ODk3OWUtMjhjNS00NjFkLWI4M2YtMzVlY2U0ZWEzNGFlIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZSI6Im1haGVkZWUiLCJVc2VySWQiOiI0NjY4OTc5ZS0yOGM1LTQ2MWQtYjgzZi0zNWVjZTRlYTM0YWUiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbiIsImV4cCI6MTY0MzM2NTM4NSwiaXNzIjoiand0IiwiYXVkIjoiand0In0.n9fwbKP5rhqG_rAr8GHs8CyiGJA8myp2fTiLALc1j08';
    //let BaseURL = window.SERVER_URL;

    //console.log("Log Token: " + token);

    let token=SessionManager.getToken();
    let BaseURL = "https://localhost:7142/";
    let payload = {
        method: 'GET',
        headers: {   
            "access-control-allow-origin" : "*", 
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + token
         },
    }
    return fetch(BaseURL + type, payload)
    .then(function(response) {
        if (!response.ok) {
            throw Error(response.statusText);
        }
        return response.json();
    }).then(function(result) {
        return result;
    }).catch(function(error) {
        console.log(error);
    });
}

export function postDataForLogin(type, userData) {
    //let BaseURL = window.SERVER_URL;
    let BaseURL = "https://localhost:7142/";
    let payload = {
        method: 'POST',
        headers: {   
            "access-control-allow-origin" : "*",
            'Content-Type': 'application/json' 
        },
        body: JSON.stringify(userData)

    }
    return fetch(BaseURL + type, payload)
    .then(function(response) {
        return response.json();
    }).then(function(result) {
        return result;
    }).catch(function(error) {
        console.log(error);
    });
}

export function putData(endPoint, obj) {
    //let BaseURL = window.SERVER_URL;
    let token=SessionManager.getToken();
    let BaseURL = "https://localhost:7142/";
    let payload = {
        method: 'PUT',
        headers: {   
            "access-control-allow-origin" : "*", 
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + token
        },
        body: JSON.stringify(obj)

    }
    return fetch(BaseURL + endPoint, payload)
    .then(function(response) {
        return response.json();
    }).then(function(result) {
        return result;
    }).catch(function(error) {
        console.log(error);
    });
}