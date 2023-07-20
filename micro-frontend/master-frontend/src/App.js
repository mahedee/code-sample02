import { BrowserRouter, Routes, Route } from "react-router-dom";
//import TopNav from "./common/TopNav";
import SideNavbar from "./common/SideNavbar";
import { Home } from "./components/Home";
import {
  ProductList,
  CategoryList,
} from "./micro-services/ProductMicroServices";
import { Header } from "./micro-services/LayoutMicroServices";
import { CustomerList } from "./micro-services/CustomerMicroServices";

function App() {
  return (
    <>
      <BrowserRouter>
        <Header />
        <SideNavbar />
        <div style={{ marginLeft: "80px" }}>
          <Routes>
            <Route path="/" element={<Home />} />
            <Route path="/home" element={<Home />} />
            <Route path="/product-list" element={<ProductList />} />
            <Route path="/product-categories" element={<CategoryList />} />
            <Route path="/customers" element={<CustomerList />} />
          </Routes>
        </div>
      </BrowserRouter>
    </>
  );
}

export default App;
