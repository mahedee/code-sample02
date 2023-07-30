//CustomerDataService to make asynchronous HTTP requests
import http from "../config/http-common";

const getAll = () => {
  return http.get("/Customers");
};

const get = (id) => {
  return http.get(`/Customers/${id}`);
};

const create = (data) => {
  return http.post("/Customers", data);
};

const update = (id, data) => {
  return http.put(`/Customers/${id}`, data);
};

const remove = (id) => {
  return http.delete(`/Customers/${id}`);
};

const findByName = (name) => {
  return http.get(`/Customers/SearchCustomers?name=${name}`);
};

const CustomerDataService = {
  getAll,
  get,
  create,
  update,
  remove,
  findByName,
};

export default CustomerDataService;
