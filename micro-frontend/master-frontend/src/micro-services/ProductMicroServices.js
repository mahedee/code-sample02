import MicroFrontend from "../MicroFrontend";

const { REACT_APP_PRODUCT_HOST: productHost } = process.env;

export function ProductList({ history }) {
  return <MicroFrontend history={history} host={productHost} name="Product" />;
}

export function CategoryList({ history }) {
  return <MicroFrontend history={history} host={productHost} name="Product" />;
}
