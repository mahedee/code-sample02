import React, { useEffect, useState } from "react";
import { useDispatch } from "react-redux";
import { useNavigate, useParams } from "react-router-dom";
import CustomerDataService from "../../services/CustomerService";
import { updateCustomer } from "../../redux/slices/customersSlice";
import { SuccessToastify } from "../../helper/ToastifyMessage";
import { ConvertDateISOString } from "../../utils/Conversion";

const EditCustomer = () => {
  const { id } = useParams();
  let navigate = useNavigate();

  const initialCustomerState = {
    id: 0,
    firstName: "",
    lastName: "",
    email: "",
    phone: "",
    birthDate: null,
  };

  const [customerObj, setCustomerObj] = useState(initialCustomerState);

  const dispatch = useDispatch();

  const getCustomer = (id) => {
    CustomerDataService.get(id)
      .then((response) => {
        setCustomerObj(response.data);
      })
      .catch((e) => {
        console.log(e);
      });
  };

  useEffect(() => {
    if (id) {
      getCustomer(id);
    }
  }, [id]);

  const handleInputChange = (event) => {
    const { name, value } = event.target;
    setCustomerObj({ ...customerObj, [name]: value });
  };

  const backtoList = () => {
    navigate("/customers");
  };

  const onSubmit = () => {
    console.log("Clicked submit button");

    dispatch(updateCustomer({ id: customerObj.id, data: customerObj }))
      .unwrap()
      .then((response) => {
        console.log(response);
        SuccessToastify("Customer has been updated successfully!");
        navigate("/customers");
      })
      .catch((e) => {
        console.log(e);
      });
  };

  return (
    <div className="card">
      <div className="card-body">
        <div className="row">
          <div className="col-md-3">
            <h3>Edit Customer</h3>
            <form>
              <div className="form-group">
                <label className="control-label">First Name: </label>
                <input
                  className="form-control"
                  type="text"
                  id="firstName"
                  name="firstName"
                  value={customerObj.firstName}
                  onChange={handleInputChange}
                ></input>
              </div>

              <div className="form-group">
                <label className="control-label">Last Name: </label>
                <input
                  className="form-control"
                  type="text"
                  id="lastName"
                  name="lastName"
                  value={customerObj.lastName}
                  onChange={handleInputChange}
                ></input>
              </div>

              <div className="form-group">
                <label className="control-label">Email: </label>
                <input
                  className="form-control"
                  type="text"
                  id="email"
                  name="email"
                  value={customerObj.email}
                  onChange={handleInputChange}
                ></input>
              </div>

              <div className="form-group">
                <label className="control-label">Phone: </label>
                <input
                  className="form-control"
                  type="text"
                  id="phone"
                  name="phone"
                  value={customerObj.phone}
                  onChange={handleInputChange}
                ></input>
              </div>

              <div className="form-group">
                <label className="control-label">Birth Date: </label>
                <input
                  className="form-control"
                  type="date"
                  id="birthDate"
                  name="birthDate"
                  value={ConvertDateISOString(customerObj.birthDate)}
                  onChange={handleInputChange}
                ></input>
              </div>
            </form>

            <div className="form-group">
              <input
                type="button"
                value="Edit Customer"
                className="btn btn-primary"
                onClick={onSubmit}
              ></input>
              ||
              <input
                type="button"
                value="Back to List"
                className="btn btn-primary"
                onClick={backtoList}
              ></input>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default EditCustomer;
