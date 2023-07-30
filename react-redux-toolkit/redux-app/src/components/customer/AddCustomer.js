import { useState } from "react";
import { useDispatch } from "react-redux";
import { useNavigate } from "react-router-dom";
import { createCustomer } from "../../redux/slices/customersSlice";
import { SuccessToastify } from "../../helper/ToastifyMessage";

const AddCustomer = () => {
  const initialCustomerState = {
    id: 0,
    firstName: "",
    lastName: "",
    email: "",
    phone: "",
    birthDate: "",
  };

  const [customer, setCustomer] = useState(initialCustomerState);
  const navigate = useNavigate();

  const dispatch = useDispatch();

  // The event object is automatically passed to this function when the input field's value changes
  const handleInputChange = (event) => {
    // This line uses object destructuring to extract two properties from the event.target object.
    const { name, value } = event.target;

    //what it means - debug
    // creates a shallow copy of the existing product state. This ensures that other properties of the product object remain unchanged.
    // this line updates the product state by modifying only the property specified by the name attribute of the input field, while keeping the rest of the properties intact.
    setCustomer({ ...customer, [name]: value });
  };

  const saveCustomer = () => {
    dispatch(createCustomer(customer))
      .unwrap()
      .then((data) => {
        SuccessToastify("Customer has been created successfully!");
        setCustomer({
          id: data.id,
          firstName: data.firstName,
          lastName: data.lastName,
          email: data.email,
          phone: data.phone,
          birthDate: data.birthDate,
        });
        navigate("/customers");
      })
      .catch((e) => {
        console.log(e);
      });
  };

  const backToList = () => {
    navigate("/customers");
  };

  return (
    <div className="card">
      <div className="card-body">
        <div className="row">
          <div className="col-md-3">
            <h3>Create Customer</h3>
            <div>
              <div className="form-group">
                <label htmlFor="firstName">First Name</label>
                <input
                  type="text"
                  className="form-control"
                  id="firstName"
                  required
                  value={customer.firstName || ""}
                  onChange={handleInputChange}
                  name="firstName"
                />
              </div>
              <div className="form-group">
                <label htmlFor="description">Last Name</label>
                <input
                  type="text"
                  className="form-control"
                  id="lastName"
                  required
                  value={customer.lastName || ""}
                  onChange={handleInputChange}
                  name="lastName"
                />
              </div>
              <div className="form-group">
                <label htmlFor="email">Email</label>
                <input
                  type="text"
                  className="form-control"
                  id="email"
                  required
                  value={customer.email || ""}
                  onChange={handleInputChange}
                  name="email"
                />
              </div>
              <div className="form-group">
                <label htmlFor="phone">Phone</label>
                <input
                  type="text"
                  className="form-control"
                  id="phone"
                  required
                  value={customer.phone || ""}
                  onChange={handleInputChange}
                  name="phone"
                />
              </div>
              <div className="form-group">
                <label htmlFor="birthDate">Birth Date</label>
                <input
                  type="date"
                  className="form-control"
                  id="birthDate"
                  required
                  value={customer.birthDate || ""}
                  onChange={handleInputChange}
                  name="birthDate"
                />
              </div>
              <button onClick={saveCustomer} className="btn btn-success">
                Submit
              </button>
              ||
              <button className="btn btn-success" onClick={backToList}>
                Back to List
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default AddCustomer;
