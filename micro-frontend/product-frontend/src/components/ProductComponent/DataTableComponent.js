import React from "react";
import { Card } from "@mui/material";
import SortIcon from "@mui/icons-material/ArrowDownward";
import DataTable from "react-data-table-component";
import { convertDateFormat } from "../../utils/Conversions";

const DataTableComponent = ({ data }) => {
  console.log(data);
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
      name: "Short Name",
      selector: (row) => row.shortName,
      sortable: true,
      right: true,
      reorder: true,
    },
    {
      id: 3,
      name: "Price",
      selector: (row) => row.price,
      sortable: true,
      right: true,
      reorder: true,
    },

    {
      id: 4,
      name: "Manufacture Date",
      selector: (row) => convertDateFormat(row.manufactureDate),
      sortable: true,
      right: true,
      reorder: true,
    },
    {
      id: 5,
      name: "Expiry Date",
      selector: (row) => convertDateFormat(row.expiryDate),
      sortable: true,
      right: true,
      reorder: true,
    },
  ];
  return (
    <div className="data-table">
      <Card>
        <DataTable
          title="Product List"
          columns={columns}
          data={data}
          defaultSortFieldId={4}
          sortIcon={<SortIcon />}
          pagination
          selectableRows
          paginationPerPage={5}
          paginationRowsPerPageOptions={[5, 10, 15, 20]}
        />
      </Card>
    </div>
  );
};

export default DataTableComponent;
