import React, { useEffect, useState } from "react";
import { GetAllProducts } from "../../services/ProductService";
import DataTableComponent from "./DataTableComponent";

const ProductListComponent = () => {
  const [state, setState] = useState([]);

  useEffect(() => {
    GetAllProducts().then((res) => {
      console.log(res.data);
      setState(res.data);
    });
  }, []);

  return (
    <div style={{ padding: "30px" }}>
      <DataTableComponent data={state} />
    </div>
  );
};

export default ProductListComponent;
