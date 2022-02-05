
import SessionManager from "../Auth/SessionManager";
import { BASE_URL } from "./Settings";


export function getData(endPoint) {

    let token=SessionManager.getToken();

    let payload = {
        method: 'GET',
        headers: {   
            "access-control-allow-origin" : "*", 
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + token
         },
    }
    return fetch(BASE_URL + endPoint, payload)
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
    //let BaseURL = "https://localhost:7142/";
    let payload = {
        method: 'POST',
        headers: {   
            "access-control-allow-origin" : "*",
            'Content-Type': 'application/json' 
        },
        body: JSON.stringify(userData)

    }
    return fetch(BASE_URL + type, payload)
    .then(function(response) {
        return response.json();
    }).then(function(result) {
        return result;
    }).catch(function(error) {
        console.log(error);
    });
}

export function postData(endPoint, inputObj) {
    let token=SessionManager.getToken();
    let payload = {
        method: 'POST',
        headers: {   
            "access-control-allow-origin" : "*", 
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + token
        },
        body: JSON.stringify(inputObj)

    }
    return fetch(BASE_URL + endPoint, payload)
    .then(function(response) {
        return response.json();
    }).then(function(result) {
        return result;
    }).catch(function(error) {
        console.log(error);
    });
}

export function deleteData(endPoint) {
    let token=SessionManager.getToken();
    let payload = {
        method: 'DELETE',
        headers: {   
            "access-control-allow-origin" : "*", 
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + token
         },
    }
    return fetch(BASE_URL + endPoint, payload)
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

export function putData(endPoint, obj) {
    let token=SessionManager.getToken();
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
    return fetch(BASE_URL + endPoint, payload)
    .then(function(response) {
        return response.json();
    }).then(function(result) {
        return result;
    }).catch(function(error) {
        console.log(error);
    });
}