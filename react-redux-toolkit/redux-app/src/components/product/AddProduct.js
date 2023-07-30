import { useState } from "react";
import { useDispatch } from "react-redux";
import { createProduct } from "../../redux/slices/productsSlice";
import { useNavigate } from "react-router-dom";
import { SuccessToastify } from "../../helper/ToastifyMessage";

const AddProduct = () => {
  const initialProductState = {
    id: 0,
    name: "",
    description: "",
    price: 0,
    stockQuantity: 0,
  };

  const [product, setProduct] = useState(initialProductState);
  const navigate = useNavigate();

  const dispatch = useDispatch();

  // The event object is automatically passed to this function when the input field's value changes
  const handleInputChange = (event) => {
    // This line uses object destructuring to extract two properties from the event.target object.
    const { name, value } = event.target;

    // creates a shallow copy of the existing product state. This ensures that other properties of the product object remain unchanged.
    // this line updates the product state by modifying only the property specified by the name attribute of the input field, while keeping the rest of the properties intact.
    setProduct({ ...product, [name]: value });
  };

  const saveProduct = () => {
    dispatch(createProduct(product))
      .unwrap()
      .then((data) => {
        console.log(data);
        setProduct({
          id: data.id,
          name: data.title,
          description: data.description,
          price: data.price,
          stockQuantity: data.stockQuantity,
        });
        SuccessToastify("Product information saved successfully!!");
        navigate("/products");
      })
      .catch((e) => {
        console.log(e);
      });
  };

  const handleBackToList = () => {
    navigate("/products");
  };

  return (
    <div className="card">
      <div className="card-body">
        <div className="row">
          <div className="col-md-3">
            <h3>Create Product</h3>
            <div>
              <div className="form-group">
                <label htmlFor="name">Name</label>
                <input
                  type="text"
                  className="form-control"
                  id="name"
                  required
                  value={product.name || ""}
                  onChange={handleInputChange}
                  name="name"
                />
              </div>
              <div className="form-group">
                <label htmlFor="description">Description</label>
                <input
                  type="text"
                  className="form-control"
                  id="description"
                  required
                  value={product.description || ""}
                  onChange={handleInputChange}
                  name="description"
                />
              </div>
              <div className="form-group">
                <label htmlFor="price">Price</label>
                <input
                  type="text"
                  className="form-control"
                  id="price"
                  required
                  value={product.price || ""}
                  onChange={handleInputChange}
                  name="price"
                />
              </div>
              <div className="form-group">
                <label htmlFor="stockQuantity">Stock Quantity</label>
                <input
                  type="text"
                  className="form-control"
                  id="stockQuantity"
                  required
                  value={product.stockQuantity || ""}
                  onChange={handleInputChange}
                  name="stockQuantity"
                />
              </div>
              <button
                type="button"
                onClick={saveProduct}
                className="btn btn-success"
              >
                Submit
              </button>
              ||
              <button
                type="button"
                className="btn btn-info"
                onClick={handleBackToList}
              >
                Back to List
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default AddProduct;
