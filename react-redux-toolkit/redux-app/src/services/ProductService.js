//ProductDataService to make asynchronous HTTP requests
import http from "../config/http-common";

const getAll = () => {
  return http.get("/Products");
};

const get = (id) => {
  return http.get(`/Products/${id}`);
};

const create = (data) => {
  return http.post("/Products", data);
};

const update = (id, data) => {
  return http.put(`/Products/${id}`, data);
};

const remove = (id) => {
  return http.delete(`/Products/${id}`);
};
const ProductDataService = {
  getAll,
  get,
  create,
  update,
  remove,
};

export default ProductDataService;
