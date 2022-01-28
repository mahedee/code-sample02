// //let BaseURL = window.SERVER_URL;

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

export function GetData(type) {
    //let token=Auth.getToken();
    let token='eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJtYWhlZGVlIiwianRpIjoiNDY2ODk3OWUtMjhjNS00NjFkLWI4M2YtMzVlY2U0ZWEzNGFlIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZSI6Im1haGVkZWUiLCJVc2VySWQiOiI0NjY4OTc5ZS0yOGM1LTQ2MWQtYjgzZi0zNWVjZTRlYTM0YWUiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbiIsImV4cCI6MTY0MzM0OTU0OSwiaXNzIjoiand0IiwiYXVkIjoiand0In0.t4nlZ79uXLlExsHaTibSht5eiMOiYVTBdrlqEUGf7f4';
    //let BaseURL = window.SERVER_URL;

    console.log("Log Token: " + token);
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