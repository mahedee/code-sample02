import axios from "axios";
import { Base_URL } from "./BaseURL";

export default axios.create({
  baseURL: Base_URL,
  headers: {
    "Content-type": "application/json",
  },
});
