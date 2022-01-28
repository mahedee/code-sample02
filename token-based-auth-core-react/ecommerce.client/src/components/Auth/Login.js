import { Component } from "react";

export default class Login extends Component {
    constructor() {
        super();
        this.state = {
            userName: "",
            password: "",
            loading: true,
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
    }

    render() {
        return (
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
                        <div className="col-md-6">
                            <button className="btn btn-primary btn-block" onClick={this.login}>
                                Sign In
                            </button>
                        </div>
                        <div className="col-md-2">
                            {/* {content} */}
                        </div>
                    </div>
                    <div className="row">
                        <div className="col-md-8" style={{ textAlign: "center", paddingTop: "16px" }}>
                            <strong className="has-error" style={{ color: "red" }}>{this.state.errorMsg}</strong>
                        </div>
                        <div className="col-md-4">
                            {/* <ToastContainer /> */}
                        </div>
                    </div>
                </div>
            </div>
        );
    }
}