import React, { useEffect, useState } from "react";
import { GetAllCategory } from "../../services/ProductService";
import DataTableComponent from "./DataTableComponent";

const CategoryListComponent = () => {
  const [state, setState] = useState([]);

  useEffect(() => {
    GetAllCategory().then((res) => {
      console.log("category", res.data);
      setState(res.data);
    });
  }, []);

  return (
    <div style={{ padding: "30px" }}>
      <DataTableComponent data={state} />
    </div>
  );
};

export default CategoryListComponent;
