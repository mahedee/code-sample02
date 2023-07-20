import MicroFrontend from "../MicroFrontend";

const { REACT_APP_CUSTOMER_HOST: customerHost } = process.env;

export function CustomerList({ history }) {
  return (
    <MicroFrontend history={history} host={customerHost} name="Customer" />
  );
}
