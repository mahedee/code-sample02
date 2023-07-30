import { useCallback, useEffect, useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import { Link } from "react-router-dom";
import {
  deleteCustomer,
  findCustomerByName,
  retrieveCustomers,
} from "../../redux/slices/customersSlice";
import { ConvertDateISOString } from "../../utils/Conversion";
import { WarningTostify } from "../../helper/ToastifyMessage";

const CustomerList = () => {
  const [searchCustomer, setSearchCustomer] = useState("");

  // useSelector() is a hook provided by the React Redux library that allows functional components to extract data from the Redux store
  // The state.products is a property in the Redux store that holds information related to products

  debugger;
  const customers = useSelector((state) => state.customers);

  //  useDispatch() is a hook provided by the React Redux library that allows functional components to interact with the Redux store by dispatching actions.
  // dispatching actions is the process of sending a signal to the Redux store to update its state.
  const dispatch = useDispatch();
  const initFetch = useCallback(() => {
    // retrieveCustomers is an action creator that returns an action object with the type 'RETRIEVE_CUSTOMERS'
    dispatch(retrieveCustomers());
  }, [dispatch]);

  useEffect(() => {
    initFetch();
  }, [initFetch]);

  function onDeleteCustomer(id) {
    dispatch(deleteCustomer(id))
      .then((response) => {
        console.log(response);
        WarningTostify("Customer has been deleted");
      })
      .catch((e) => {
        console.log(e);
      });
  }

  const onChangeSearchCustomer = (e) => {
    const searchCustomer = e.target.value;

    setSearchCustomer(searchCustomer);
  };

  const findByCustomerName = () => {
    debugger;
    dispatch(findCustomerByName({ name: searchCustomer }));
  };

  return (
    <div className="card">
      <div className="card-body">
        <div>
          <h3>Customer List</h3>
          <Link to={"/add-customer/"}>
            <button className="btn btn-primary">Create</button>
          </Link>

          <br></br>
          <br></br>
          <div className="input-group mb-3">
            <input
              type="text"
              placeholder="Search by customer name"
              className="form-control"
              value={searchCustomer}
              onChange={onChangeSearchCustomer}
            />
            <div className="input-group-append">
              <button
                className="btn btn-info "
                type="button"
                onClick={findByCustomerName}
              >
                Search
              </button>
            </div>
          </div>

          <table className="table table-stripped">
            <thead>
              <tr>
                <th>Id</th>
                <th>First Name</th>
                <th>Last Name</th>
                <th>Email</th>
                <th>Phone</th>
                <th>Birth Date</th>
                <th>Actions</th>
              </tr>
            </thead>
            <tbody>
              {customers &&
                customers.map((customer) => (
                  <tr key={customer.id}>
                    <td>{customer.id}</td>
                    <td>{customer.firstName}</td>
                    <td>{customer.lastName}</td>
                    <td>{customer.email}</td>
                    <td>{customer.phone}</td>
                    <td>{ConvertDateISOString(customer.birthDate)}</td>
                    <td>
                      {" "}
                      <Link
                        to={"/edit-customer/" + customer.id}
                        className="badge badge-warning"
                      >
                        Edit
                      </Link>
                      ||
                      <Link
                        onClick={() => onDeleteCustomer(customer.id)}
                        className="badge badge-danger"
                      >
                        Delete
                      </Link>
                    </td>
                  </tr>
                ))}
            </tbody>
          </table>
        </div>
      </div>
    </div>
  );
};

export default CustomerList;
