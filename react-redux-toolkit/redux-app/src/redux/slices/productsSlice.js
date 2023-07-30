// Instead of creating many folders and files for Redux (actions, reducers, types,…), with redux-toolkit we just need all-in-one: slice.
// A slice is a collection of Redux reducer logic and actions for a single feature.
// For creating a slice, we need: name to identify the slice, initial state value, one or more reducer functions to define how the state can be updated
// Redux Toolkit provides createSlice() function that will auto-generate the action types and action creators for you, based on the names of the reducer functions you provide.

// We also need to use Redux Toolkit createAsyncThunk which provides a thunk that will take care of the action types and dispatching the right actions based on the returned promise
import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import ProductDataService from "../../services/ProductService";

const initialState = [];

export const createProduct = createAsyncThunk(
  "products/create",
  async (inputData) => {
    const res = await ProductDataService.create(inputData);
    return res.data;
  }
);

export const retrieveProducts = createAsyncThunk(
  "products/retrieve",
  async () => {
    const res = await ProductDataService.getAll();
    return res.data;
  }
);

export const updateProduct = createAsyncThunk(
  "products/update",
  async ({ id, data }) => {
    debugger;
    const res = await ProductDataService.update(id, data);
    return res.data;
  }
);

export const deleteProduct = createAsyncThunk("products/delete", async (id) => {
  debugger;
  await ProductDataService.remove(id);
  return { id };
});

const productSlice = createSlice({
  name: "product",
  initialState,
  extraReducers: {
    [createProduct.fulfilled]: (state, action) => {
      state.push(action.payload);
    },

    [retrieveProducts.fulfilled]: (state, action) => {
      return [...action.payload];
    },

    [updateProduct.fulfilled]: (state, action) => {
      const index = state.findIndex(
        (tutorial) => tutorial.id === action.payload.id
      );
      state[index] = {
        ...state[index],
        ...action.payload,
      };
    },

    [deleteProduct.fulfilled]: (state, action) => {
      let index = state.findIndex(({ id }) => id === action.payload.id);
      state.splice(index, 1);
    },
  },
});

// destructure reducer from productSlice
// name mustbe reducer
const { reducer } = productSlice;
export default reducer;
