import React from "react";
import { Link } from "react-router-dom";

const TopNav = () => {
  return (
    <nav className="navbar navbar-expand navbar-dark bg-dark">
      <a href="/products" className="navbar-brand">
        Mahedee.net
      </a>
      <div className="navbar-nav mr-auto">
        <li className="nav-item">
          <Link to={"/products"} className="nav-link">
            Products
          </Link>
        </li>

        <li className="nav-item">
          <Link to={"/customers"} className="nav-link">
            Customers
          </Link>
        </li>
      </div>
    </nav>
  );
};

export default TopNav;
