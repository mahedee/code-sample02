import React, { Component } from 'react';
import { Container } from 'reactstrap';
import SessionManager from './Auth/SessionManager';
import { NavMenu } from './NavMenu';

export class Layout extends Component {
  static displayName = Layout.name;

  render() {
    return (

      SessionManager.getToken() ? (
        <div>
          <NavMenu />
          <Container>
            {this.props.children}
          </Container>
        </div>

      ) :
        (
          <div>
            <Container>
              {this.props.children}
            </Container>
          </div>
        )

    );
  }
}
