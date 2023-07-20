import React, { useEffect, useState } from "react";
import { GetAllCustomers } from "../../services/CustomerService";
import DataTableComponent from "./DataTableComponent";

const CustomersListComponent = () => {
  const [state, setState] = useState([]);

  useEffect(() => {
    GetAllCustomers().then((res) => {
      setState(res.data);
    });
  }, []);

  return (
    <div style={{ padding: "30px" }}>
      <DataTableComponent data={state} />
    </div>
  );
};

export default CustomersListComponent;
