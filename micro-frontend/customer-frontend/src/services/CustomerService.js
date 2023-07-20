import axios from "axios";
import { Base_URL_Customer } from "../utils/BaseUrl";

export const GetAllCustomers = (offset, pageSize, access_token) => {
  try {
    const response = axios.get(Base_URL_Customer + "/Customers");
    return response;
  } catch (error) {
    throw error;
  }
};
