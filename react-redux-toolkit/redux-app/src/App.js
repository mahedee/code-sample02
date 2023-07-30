import logo from "./logo.svg";
import "./App.css";
import { BrowserRouter, Route, Routes } from "react-router-dom";
import "bootstrap/dist/css/bootstrap.min.css";
import EditProduct from "./components/product/EditProduct";
import TopNav from "./layout/TopNav";
import CustomerList from "./components/customer/CustomerList";
import AddCustomer from "./components/customer/AddCustomer";
import EditCustomer from "./components/customer/EditCustomer";
import { ToastContainer } from "react-toastify";
import ProductList from "./components/product/ProductsList";
import AddProduct from "./components/product/AddProduct";

function App() {
  return (
    <BrowserRouter>
      <TopNav />
      <Routes>
        <Route path="/" element={<ProductList />} />
        <Route path="/products" element={<ProductList />} />
        <Route path="/add-product" element={<AddProduct />} />
        <Route path="/edit-product/:id" element={<EditProduct />} />
        <Route path="/customers" element={<CustomerList />} />
        <Route path="/add-customer" element={<AddCustomer />} />
        <Route path="/edit-customer/:id" element={<EditCustomer />} />
      </Routes>
      <ToastContainer />
    </BrowserRouter>
  );
}

export default App;
