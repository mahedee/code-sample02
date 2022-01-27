import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { FetchData } from './components/FetchData';
import { Counter } from './components/Counter';

import './custom.css'

import { Employees } from './components/Employee/Employees';
// import { Create } from './components/Employee/Create';
// import { Edit } from './components/Employee/Edit';
// import { Delete } from './components/Employee/Delete';
import { UnderConstruction } from './components/UnderConstructions/UnderConstruction';
import { Customers } from './components/Customer/Customers';
import { Create } from './components/Customer/Create';
import { Edit } from './components/Customer/Edit';
import { Delete } from './components/Customer/Delete';

import {CreateCustomer } from './components/Employee/Create';
import { EditEmployee } from './components/Employee/Edit';
import { DeleteEmployee } from './components/Employee/Delete';

export default class App extends Component {
  static displayName = App.name;


  render() {
    return (
      <Layout>
        <Route exact path='/' component={Home} />
        <Route path='/employees' component={Employees} />
        <Route path='/create' component={CreateCustomer}></Route>
        <Route path='/Edit/:id' component={EditEmployee}></Route>
        <Route path='/Delete/:id' component={DeleteEmployee}></Route>

        <Route path='/banking/customers' component={Customers} />
        <Route path='/banking/customer/create' component={Create} />
        <Route path='/banking/customer/edit/:id' component={Edit}></Route>
        <Route path='/banking/customer/delete/:id' component={Delete}></Route>

        <Route path='/underConstructions' component={UnderConstruction} />

        <Route path='/counter' component={Counter} />
        <Route path='/fetch-data' component={FetchData} />

      </Layout>
    );
  }
}
