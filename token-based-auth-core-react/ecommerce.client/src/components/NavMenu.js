import React, { Component } from 'react';
import {
    Collapse, Container, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink, UncontrolledDropdown,
    DropdownToggle,
    DropdownMenu,
    DropdownItem,
} from 'reactstrap';

import { Link } from 'react-router-dom';
import './NavMenu.css';
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
                        <NavbarBrand tag={Link} to="/">AMBS</NavbarBrand>
                        <NavbarToggler onClick={this.toggleNavbar} className="mr-2" />
                        <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={!this.state.collapsed} navbar>
                            <ul className="navbar-nav flex-grow">
                                <NavItem>
                                    <NavLink tag={Link} className="text-dark" to="/">Home</NavLink>
                                </NavItem>

                                <UncontrolledDropdown nav inNavbar>
                                    <DropdownToggle nav caret>
                                        Programs
                                    </DropdownToggle>
                                    <DropdownMenu right>
                                        <DropdownItem>
                                            <NavLink tag={Link} className="text-dark" to="/counter">Loan Officer</NavLink>
                                        </DropdownItem>
                                        <DropdownItem>
                                            <NavLink tag={Link} className="text-dark" to="/counter">Master Roll</NavLink>
                                        </DropdownItem>
                                        <DropdownItem divider />
                                        <DropdownItem>
                                            <NavItem>
                                                <NavLink tag={Link} className="text-dark" to="/underConstructions">Monthly Report</NavLink>
                                            </NavItem>
                                        </DropdownItem>
                                        <DropdownItem>
                                            <NavLink tag={Link} className="text-dark" to="/underConstructions">Daily Overdue Report</NavLink>
                                        </DropdownItem>
                                    </DropdownMenu>
                                </UncontrolledDropdown>

                                <UncontrolledDropdown nav inNavbar>
                                    <DropdownToggle nav caret>
                                        Banking
                                    </DropdownToggle>
                                    <DropdownMenu right>
                                        <DropdownItem>
                                            <NavLink tag={Link} className="text-dark" to="/banking/customers">Customer</NavLink>
                                        </DropdownItem>
                                        <DropdownItem>
                                            <NavLink tag={Link} className="text-dark" to="/underConstructions">Account</NavLink>
                                        </DropdownItem>
                                        <DropdownItem divider />
                                        <DropdownItem>
                                            <NavItem>
                                                <NavLink tag={Link} className="text-dark" to="/underConstructions">Banking Report</NavLink>
                                            </NavItem>
                                        </DropdownItem>
                                        <DropdownItem>
                                            <NavLink tag={Link} className="text-dark" to="/underConstructions">View Statement</NavLink>
                                        </DropdownItem>
                                    </DropdownMenu>
                                </UncontrolledDropdown>


                                <UncontrolledDropdown nav inNavbar>
                                    <DropdownToggle nav caret>
                                        Accounts
                                    </DropdownToggle>
                                    <DropdownMenu right>
                                        <DropdownItem>
                                            <NavLink tag={Link} className="text-dark" to="/underConstructions">Chart of Account</NavLink>
                                        </DropdownItem>
                                        <DropdownItem>
                                            <NavLink tag={Link} className="text-dark" to="/underConstructions">Currency Rate</NavLink>
                                        </DropdownItem>
                                        <DropdownItem divider />
                                        <DropdownItem>
                                            <NavItem>
                                                <NavLink tag={Link} className="text-dark" to="/underConstructions">Accounts Report</NavLink>
                                            </NavItem>
                                        </DropdownItem>
                                        <DropdownItem>
                                            <NavLink tag={Link} className="text-dark" to="/underConstructions">Trail Balance Report</NavLink>
                                        </DropdownItem>
                                    </DropdownMenu>
                                </UncontrolledDropdown>


                                <UncontrolledDropdown nav inNavbar>
                                    <DropdownToggle nav caret>
                                        Monitoring
                                    </DropdownToggle>
                                    <DropdownMenu right>
                                        <DropdownItem>
                                            <NavLink tag={Link} className="text-dark" to="/underConstructions">BM Daily Activity</NavLink>
                                        </DropdownItem>
                                        <DropdownItem>
                                            <NavLink tag={Link} className="text-dark" to="/underConstructions">RM Monitoring Checklist</NavLink>
                                        </DropdownItem>
                                        <DropdownItem divider />
                                        <DropdownItem>
                                            <NavItem>
                                                <NavLink tag={Link} className="text-dark" to="/underConstructions">Monitoring Report</NavLink>
                                            </NavItem>
                                        </DropdownItem>
                                        <DropdownItem>
                                            <NavLink tag={Link} className="text-dark" to="/underConstructions">Others Report</NavLink>
                                        </DropdownItem>
                                    </DropdownMenu>
                                </UncontrolledDropdown>


                                <UncontrolledDropdown nav inNavbar>
                                    <DropdownToggle nav caret>
                                        HRM
                                    </DropdownToggle>
                                    <DropdownMenu right>
                                        <DropdownItem>
                                            <NavLink tag={Link} className="text-dark" to="/employees">Employee</NavLink>
                                        </DropdownItem>
                                        <DropdownItem>
                                            <NavLink tag={Link} className="text-dark" to="/underConstructions">Employee Promotion</NavLink>
                                        </DropdownItem>
                                        <DropdownItem divider />
                                        <DropdownItem>
                                            <NavItem>
                                                <NavLink tag={Link} className="text-dark" to="/underConstructions">HRM Report</NavLink>
                                            </NavItem>
                                        </DropdownItem>
                                        <DropdownItem>
                                            <NavLink tag={Link} className="text-dark" to="/underConstructions">Attendance Report</NavLink>
                                        </DropdownItem>
                                    </DropdownMenu>
                                </UncontrolledDropdown>



                                <NavItem>
                                    <NavLink tag={Link} className="text-dark" to="/">Security</NavLink>
                                </NavItem>

                                <NavItem>
                                    <NavLink tag={Link} className="text-dark" to="/">Admin</NavLink>
                                </NavItem>

                                {/* <NavItem>
                                    <NavLink tag={Link} className="text-dark" to="/employees">Employees</NavLink>
                                </NavItem> */}

                                {/* <NavItem>
                                    <NavLink tag={Link} className="text-dark" to="/counter">Counter</NavLink>
                                </NavItem>
                                <NavItem>
                                    <NavLink tag={Link} className="text-dark" to="/fetch-data">Fetch data</NavLink>
                                </NavItem>

                                <UncontrolledDropdown nav inNavbar>
                                    <DropdownToggle nav caret>
                                        Options
                                    </DropdownToggle>
                                    <DropdownMenu right>
                                        <DropdownItem>
                                            <NavLink tag={Link} className="text-dark" to="/counter">Fetch data</NavLink>
                                        </DropdownItem>
                                        <DropdownItem>
                                            Option 2
                                    </DropdownItem>
                                        <DropdownItem divider />
                                        <DropdownItem>
                                            Reset
                                        </DropdownItem>
                                    </DropdownMenu>
                                </UncontrolledDropdown> */}

                                {/* <TempDropdown>Test</TempDropdown> */}

                            </ul>
                        </Collapse>
                    </Container>
                </Navbar>
            </header>
        );
    }
}
