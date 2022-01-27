import React, { Component } from 'react';

export class Home extends Component {
  static displayName = Home.name;

  render() {
    const mystyle = {
      width: "15rem",
      margin: "5px",
      background: "gray",
      float: "left"
    };

    return (
      <div>
        <div class="card" style={mystyle} >
          <div class="card-body">
            Programs
          </div>
        </div>

        <div class="card" style={mystyle} >
          <div class="card-body">
            Banking
          </div>
        </div>
        <div class="card" style={mystyle} >
          <div class="card-body">
            Accounts
          </div>
        </div>

        <div class="card" style={mystyle} >
          <div class="card-body">
            Monitoring
          </div>
        </div>

        <div class="card" style={mystyle} >
          <div class="card-body">
            HRM
          </div>
        </div>

        <div class="card" style={mystyle} >
          <div class="card-body">
            Security
          </div>
        </div>

        <div class="card" style={mystyle} >
          <div class="card-body">
            Admin
          </div>
        </div>

        <div class="card" style={mystyle} >
          <div class="card-body">
            Audit
          </div>
        </div>


      </div>
    );
  }
}
