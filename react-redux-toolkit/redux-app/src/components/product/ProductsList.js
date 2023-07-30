import { useCallback, useEffect, useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import { Link } from "react-router-dom";
import {
  deleteProduct,
  retrieveProducts,
} from "../../redux/slices/productsSlice";
import { WarningTostify } from "../../helper/ToastifyMessage";

const ProductList = () => {
  // useSelector() is a hook provided by the React Redux library that allows functional components to extract data from the Redux store
  // The state.products is a property in the Redux store that holds information related to products

  const products = useSelector((state) => state.products);

  //  useDispatch() is a hook provided by the React Redux library that allows functional components to interact with the Redux store by dispatching actions.
  // dispatching actions is the process of sending a signal to the Redux store to update its state.
  const dispatch = useDispatch();

  const initFetch = useCallback(() => {
    // retrieveProducts is an action creator that returns an action object with the type 'RETRIEVE_PRODUCTS'
    dispatch(retrieveProducts());
  }, [dispatch]);

  useEffect(() => {
    initFetch();
  }, [initFetch]);

  function onDeleteProduct(id) {
    debugger;
    dispatch(deleteProduct(id))
      .then((response) => {
        WarningTostify(`Product with id: ${id} has been deleted.`);
      })
      .catch((e) => {
        console.log(e);
      });
  }

  return (
    <div className="card">
      <div className="card-body">
        <div>
          <h3>Product List</h3>
          <Link to={"/add-product/"}>
            <button className="btn btn-primary">Create</button>
          </Link>

          <table className="table table-stripped">
            <thead>
              <tr>
                <th>Id</th>
                <th>Name</th>
                <th>Price</th>
                <th>Stock Quantity</th>
                <th>Actions</th>
              </tr>
            </thead>
            <tbody>
              {products &&
                products.map((product) => (
                  <tr key={product.id}>
                    <td>{product.id}</td>
                    <td>{product.name}</td>
                    <td>{product.price}</td>
                    <td>{product.stockQuantity}</td>
                    <td>
                      {" "}
                      <Link
                        to={"/edit-product/" + product.id}
                        className="badge badge-warning"
                      >
                        Edit
                      </Link>
                      ||
                      <Link
                        onClick={() => onDeleteProduct(product.id)}
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

export default ProductList;
