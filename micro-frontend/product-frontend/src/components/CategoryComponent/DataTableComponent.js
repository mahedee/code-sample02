import React from "react";
import { Card } from "@mui/material";
import SortIcon from "@mui/icons-material/ArrowDownward";
import DataTable from "react-data-table-component";

const DataTableComponent = ({ data }) => {
  const columns = [
    {
      id: 1,
      name: "Name",
      selector: (row) => row.name,
      sortable: true,
      reorder: true,
    },
    {
      id: 2,
      name: "Display Name",
      selector: (row) => row.displayName,
      sortable: true,
      right: true,
      reorder: true,
    },
  ];
  return (
    <div className="data-table">
      <Card>
        <DataTable
          title="Product Category List"
          columns={columns}
          data={data}
          defaultSortFieldId={4}
          sortIcon={<SortIcon />}
          pagination
          selectableRows
        />
      </Card>
    </div>
  );
};

export default DataTableComponent;
