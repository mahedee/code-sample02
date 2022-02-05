import React, { Component } from 'react';
import SessionManager from './Auth/SessionManager';

export class Home extends Component {

  constructor(props) {
    super(props);
  }
  static displayName = Home.name;

  render() {
    const mystyle = {
      width: "15rem",
      margin: "5px",
      background: "gray",
      float: "center"
    };

    // If not loggedin
    if (!SessionManager.getToken()){
      const { history } = this.props;
      history.push('/login');
      //return window.location.href = "/login";
    }

    return (
      <div>
        <div class="card-body">
          <h3>Welcome to admin panel!</h3>
          <h5>Manage users permission</h5>
        </div>
      </div>
    );
  }
}
