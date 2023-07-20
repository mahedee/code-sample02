import React from "react";
import { useLocation, useNavigate } from "react-router-dom";
import SideNav, { NavItem, NavIcon, NavText } from "@trendmicro/react-sidenav";

const SideNavbar = () => {
  const location = useLocation();
  const history = useNavigate();
  return (
    <SideNav className="sidenav">
      <SideNav
        onSelect={(selected) => {
          const to = "/" + selected;
          if (location.pathname !== to) {
            console.log("Path Name:", location.pathname);
            console.log("To:", to);
            history(to);
          }
        }}
      >
        <SideNav.Toggle />
        <SideNav.Nav defaultSelected="">
          <NavItem eventKey="home">
            <NavIcon>
              <i className="fa fa-fw fa-home" style={{ fontSize: "1.75em" }} />
            </NavIcon>
            <NavText>Dashboard</NavText>
          </NavItem>

          <NavItem eventKey="product-list">
            <NavIcon>
              <i
                className="fa fa-fw fa-device"
                style={{ fontSize: "1.75em" }}
              />
            </NavIcon>
            <NavText>Product</NavText>
          </NavItem>

          <NavItem eventKey="product-categories">
            <NavIcon>
              <i
                className="fa fa-fw fa-device"
                style={{ fontSize: "1.75em" }}
              />
            </NavIcon>
            <NavText>Categories</NavText>
          </NavItem>

          <NavItem eventKey="customers">
            <NavIcon>
              <i
                className="fa fa-fw fa-device"
                style={{ fontSize: "1.75em" }}
              />
            </NavIcon>
            <NavText>Customers</NavText>
          </NavItem>
        </SideNav.Nav>
      </SideNav>
    </SideNav>
  );
};

export default SideNavbar;
