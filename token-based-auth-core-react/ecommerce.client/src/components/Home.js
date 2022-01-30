import React, { Component } from 'react';

export class Home extends Component {
  static displayName = Home.name;

  render() {
    const mystyle = {
      width: "15rem",
      margin: "5px",
      background: "gray",
      float: "center"
    };

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
