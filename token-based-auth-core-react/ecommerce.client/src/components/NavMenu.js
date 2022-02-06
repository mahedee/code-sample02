import React, { Component } from 'react';
import {
    Collapse, Container, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink, UncontrolledDropdown,
    DropdownToggle,
    DropdownMenu,
    DropdownItem,
} from 'reactstrap';

import { Link } from 'react-router-dom';
import './NavMenu.css';
import LoginMenu from './LoginMenu';
//import { TempDropdown } from './TempDropdown';


export class NavMenu extends Component {
    static displayName = NavMenu.name;

    constructor(props) {
        super(props);

        this.toggleNavbar = this.toggleNavbar.bind(this);
        this.state = {
            collapsed: true
        };
    }

    toggleNavbar() {
        this.setState({
            collapsed: !this.state.collapsed
        });
    }

    render() {
        return (
            <header>
                <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" light>
                    <Container>
                        <NavbarBrand tag={Link} to="/">Auth</NavbarBrand>
                        <NavbarToggler onClick={this.toggleNavbar} className="mr-2" />
                        <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={!this.state.collapsed} navbar>
                            <ul className="navbar-nav flex-grow">
                                <NavItem>
                                    <NavLink tag={Link} className="text-dark" to="/home">Home</NavLink>
                                </NavItem>

                                <UncontrolledDropdown nav inNavbar>
                                    <DropdownToggle nav caret>
                                        Banking
                                    </DropdownToggle>
                                    <DropdownMenu right>
                                        <DropdownItem>
                                            <NavLink tag={Link} className="text-dark" to="/banking/customers">Customer</NavLink>
                                        </DropdownItem>
                                    </DropdownMenu>
                                </UncontrolledDropdown>

                                <UncontrolledDropdown nav inNavbar>
                                    <DropdownToggle nav caret>
                                        Admin
                                    </DropdownToggle>
                                    <DropdownMenu right>
                                        <DropdownItem>
                                            <NavLink tag={Link} className="text-dark" to="/admin/users">Users</NavLink>
                                        </DropdownItem>
                                        <DropdownItem divider />
                                        <DropdownItem>
                                            <NavLink tag={Link} className="text-dark" to="/admin/roles">Roles</NavLink>
                                        </DropdownItem>

                                        <DropdownItem>
                                            <NavLink tag={Link} className="text-dark" to="/admin/usersroles">Users Roles</NavLink>
                                        </DropdownItem>


                                    </DropdownMenu>
                                </UncontrolledDropdown>

                                {/* <NavItem>
                                    <NavLink tag={Link} className='text-dark' to="/login">Login</NavLink>
                                </NavItem> */}

                                <NavItem>
                                    <NavLink tag={Link} className='text-dark' to="/logout">Logout</NavLink>
                                </NavItem>


                            </ul>
                        </Collapse>
                    </Container>
                </Navbar>
            </header>
        );
    }
}
