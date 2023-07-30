// Instead of creating many folders and files for Redux (actions, reducers, types,…), with redux-toolkit we just need all-in-one: slice.
// A slice is a collection of Redux reducer logic and actions for a single feature.
// For creating a slice, we need: name to identify the slice, initial state value, one or more reducer functions to define how the state can be updated
// Redux Toolkit provides createSlice() function that will auto-generate the action types and action creators for you, based on the names of the reducer functions you provide.

// We also need to use Redux Toolkit createAsyncThunk which provides a thunk that will take care of the action types and dispatching the right actions based on the returned promise
import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import CustomerDataService from "../../services/CustomerService";

const initialState = [];

export const createCustomer = createAsyncThunk(
  "customers/create",
  async (inputData) => {
    const response = await CustomerDataService.create(inputData);
    return response.data;
  }
);

export const retrieveCustomers = createAsyncThunk(
  "customers/retrieve",
  async () => {
    debugger;
    const res = await CustomerDataService.getAll();
    return res.data;
  }
);

export const updateCustomer = createAsyncThunk(
  "customers/update",
  async ({ id, data }) => {
    const res = await CustomerDataService.update(id, data);
    return res.data;
  }
);

export const deleteCustomer = createAsyncThunk(
  "customers/delete",
  async (id) => {
    await CustomerDataService.remove(id);
    return { id };
  }
);

export const deleteAllCustomer = createAsyncThunk(
  "customers/deleteAll",
  async () => {
    const res = await CustomerDataService.removeAll();
    return res.data;
  }
);

export const findCustomerByName = createAsyncThunk(
  "customers/findByName",
  async ({ name }) => {
    name = name === null || name === "" ? "all" : name;
    const res = await CustomerDataService.findByName(name);
    return res.data;
  }
);

const customerSlice = createSlice({
  name: "customer",
  initialState,
  extraReducers: {
    [createCustomer.fulfilled]: (state, action) => {
      state.push(action.payload);
    },

    [retrieveCustomers.fulfilled]: (state, action) => {
      return [...action.payload];
    },

    [updateCustomer.fulfilled]: (state, action) => {
      const index = state.findIndex(
        (tutorial) => tutorial.id === action.payload.id
      );
      state[index] = {
        ...state[index],
        ...action.payload,
      };
    },

    [deleteCustomer.fulfilled]: (state, action) => {
      let index = state.findIndex(({ id }) => id === action.payload.id);
      state.splice(index, 1);
    },
    [deleteAllCustomer.fulfilled]: (state, action) => {
      return [];
    },
    [findCustomerByName.fulfilled]: (state, action) => {
      return [...action.payload];
    },
  },
});

// destructure reducer from customerSlice
// name mustbe reducer
const { reducer } = customerSlice;
export default reducer;
