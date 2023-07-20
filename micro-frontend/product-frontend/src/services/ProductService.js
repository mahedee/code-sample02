import axios from "axios";
import { Base_URL_Product } from "../utils/BaseUrl";

export const GetAllProducts = () => {
  try {
    const response = axios.get(Base_URL_Product + `/Products`);
    console.log("response", response);
    return response;
  } catch (er) {
    throw er;
  }
};

export const GetAllCategory = () => {
  try {
    const response = axios.get(Base_URL_Product + `/Categories`);
    return response;
  } catch (er) {
    throw er;
  }
};
