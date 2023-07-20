import React from "react";
import { Card } from "@mui/material";
import SortIcon from "@mui/icons-material/ArrowDownward";
import DataTable from "react-data-table-component";
import { convertDateFormat } from "../../utils/Conversions";

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
      name: "Phone No",
      selector: (row) => row.phoneNo,
      sortable: true,
      right: true,
      reorder: true,
    },
    {
      id: 3,
      name: "Email Address",
      selector: (row) => row.emailAddress,
      sortable: true,
      right: true,
      reorder: true,
    },
    {
      id: 4,
      name: "Date of Birth",
      selector: (row) => convertDateFormat(row.dob),
      sortable: true,
      right: true,
      reorder: true,
    },
  ];
  return (
    <div className="data-table">
      <Card>
        <DataTable
          title="Customers List"
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
