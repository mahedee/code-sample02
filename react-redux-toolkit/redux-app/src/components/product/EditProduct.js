import { useNavigate, useParams } from "react-router-dom";

import React, { useEffect, useState } from "react";
import { useDispatch } from "react-redux";
import ProductDataService from "../../services/ProductService";
import { updateProduct } from "../../redux/slices/productsSlice";
import { SuccessToastify } from "../../helper/ToastifyMessage";

const EditProduct = () => {
  const { id } = useParams();
  let navigate = useNavigate();

  const initialProductState = {
    id: 0,
    name: "",
    description: "",
    price: 0,
    stockQuantity: 0,
  };

  const [productObj, setProductObj] = useState(initialProductState);
  const dispatch = useDispatch();

  const getProduct = (id) => {
    ProductDataService.get(id)
      .then((response) => {
        setProductObj(response.data);
      })
      .catch((e) => {
        console.log(e);
      });
  };

  useEffect(() => {
    if (id) {
      getProduct(id);
    }
  }, [id]);

  const handleInputChange = (event) => {
    const { name, value } = event.target;
    setProductObj({ ...productObj, [name]: value });
  };

  const updateProductContent = () => {
    dispatch(updateProduct({ id: productObj.id, data: productObj }))
      .unwrap()
      .then((response) => {
        SuccessToastify("Product information has been updated successfully!");
        navigate("/products");
      })
      .catch((e) => {
        console.log(e);
      });
  };

  const backtoList = () => {
    navigate("/products");
  };

  return (
    <div className="card">
      <div className="card-body">
        <div className="row">
          <div className="col-md-3">
            <h3>Edit Product</h3>
            <form>
              <div className="form-group">
                <label htmlFor="name">Name</label>
                <input
                  type="text"
                  className="form-control"
                  id="name"
                  name="name"
                  value={productObj.name}
                  onChange={handleInputChange}
                />
              </div>
              <div className="form-group">
                <label htmlFor="description">Description</label>
                <input
                  type="text"
                  className="form-control"
                  id="description"
                  name="description"
                  value={productObj.description}
                  onChange={handleInputChange}
                />
              </div>

              <div className="form-group">
                <label htmlFor="price">Price</label>
                <input
                  type="text"
                  className="form-control"
                  id="price"
                  name="price"
                  value={productObj.price}
                  onChange={handleInputChange}
                />
              </div>

              <div className="form-group">
                <label htmlFor="stockQuantity">Stock Quantity</label>
                <input
                  type="text"
                  className="form-control"
                  id="stockQuantity"
                  name="stockQuantity"
                  value={productObj.stockQuantity}
                  onChange={handleInputChange}
                />
              </div>
            </form>
            <div className="form-group">
              <input
                type="button"
                value="Edit"
                className="btn btn-primary"
                onClick={updateProductContent}
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

export default EditProduct;
