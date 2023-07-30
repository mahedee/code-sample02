// This Store will bring Actions and Reducers together and hold the Application state.

import { configureStore } from "@reduxjs/toolkit";
import productReducer from "./slices/productsSlice";
import customerReducer from "./slices/customersSlice";

const reducer = {
  products: productReducer,
  customers: customerReducer,
};

// The Redux Toolkit configureStore() function automatically: enable the Redux DevTools Extension, sets up the thunk middleware by default, so you can immediately write thunks without more configuration.

const store = configureStore({
  reducer: reducer,
  devTools: true,
});

export default store;
