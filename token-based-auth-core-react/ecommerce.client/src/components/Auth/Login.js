import { Component } from "react";
import { toast, ToastContainer } from "react-toastify";
import 'react-toastify/dist/ReactToastify.css';
import { Container } from "reactstrap";
import LoginMenu from "../LoginMenu";
import { postDataForLogin } from "../services/AccessAPI";
import SessionManager from "./SessionManager";


export default class Login extends Component {
    constructor() {
        super();
        this.state = {
            userName: "",
            password: "",
            loading: false,
            failed: false,
            error: ''
        };

        this.login = this.login.bind(this);
        this.onChange = this.onChange.bind(this);
    }


    onChange(e) {
        this.setState({ [e.target.name]: e.target.value });
    }

    onKeyDown = (e) => {
        if (e.key === 'Enter') {
            this.login();
        }
    }

    login() {
        let userInfo = this.state;
        this.setState({
            loading: true
        });

        //console.log("login info: " + userInfo.password);
        postDataForLogin('api/Auth/Login', userInfo).then((result) => {
            if (result?.token) {

                SessionManager.setUserSession(result.userName, result.token, result.userId, result.usersRole)

                if (SessionManager.getToken()) {
                    this.setState({
                        loading: false
                    });

                    // <LoginMenu menuText = 'Logout' menuURL = '/logout' />

                    // If login successful and get token
                    // redirect to dashboard
                    window.location.href = "/home";
                }
            }

            else {
                let errors = '';
                for (const key in result?.errors) {
                    if (Object.hasOwnProperty.call(result.errors, key)) {
                        errors += result.errors[key];

                    }
                }
                errors = errors === '' ? 'Login is unsuccessfull!' : errors;
                toast.error(errors, {
                    position: "top-right",
                    autoClose: 5000,
                    hideProgressBar: true,
                    closeOnClick: true,
                    pauseOnHover: true,
                    draggable: true
                });

                this.setState({
                    errors: "Login failed!",
                    loading: false
                });
            }

        });
    }

    registration(){
        window.location.href = "/admin/user/register";

    }

    render() {
        let content;
        if (this.state.loading) {
            content = <div>Loading...</div>;
        }

        return (
            <div className="row" style={{textAlign: "center"}}>
            <div className="login-box col-md-4">
                <div className="login-logo">
                    <a href="/"><b>ECommerce</b></a>
                </div>
                <div className="login-box-body">
                    <p className="login-box-msg">Sign in to access the application</p>

                    <div className="form-group has-feedback">
                        <input
                            type="text"
                            className="form-control"
                            placeholder="Please Enter Username"
                            name="userName"
                            onChange={this.onChange}
                            onKeyDown={this.onKeyDown}
                        />
                        <span className="glyphicon glyphicon-user form-control-feedback" />
                    </div>
                    <div className="form-group has-feedback">
                        <input type="password" className="form-control" placeholder="Please Enter Password" name="password"
                            onChange={this.onChange} onKeyDown={this.onKeyDown}
                        />
                        <span className="glyphicon glyphicon-lock form-control-feedback" />
                    </div>
                    <div className="row">
                        <div className="col-md-4">
                            <button className="btn btn-primary btn-block" onClick={this.login}>
                                Sign In
                            </button>
                        </div>
                        <div className="col-md-6">
                            <button className="btn btn-primary btn-block" onClick={this.registration}>
                                Create an account
                            </button>
                        </div>
                        <div className="col-md-2">
                            {content}
                        </div>
                    </div>
                    <div className="row">
                        <div className="col-md-8" style={{ textAlign: "center", paddingTop: "16px" }}>
                            <strong className="has-error" style={{ color: "red" }}>{this.state.errorMsg}</strong>
                        </div>
                        <div className="col-md-4">
                            <ToastContainer></ToastContainer>
                        </div>
                    </div>
                </div>
            </div>

            </div>
        );
    }
}