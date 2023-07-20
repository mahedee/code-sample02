import MicroFrontend from "../MicroFrontend";

const { REACT_APP_HEADER_HOST: headerHost } = process.env;

export function Header({ history }) {
  return <MicroFrontend history={history} host={headerHost} name="Header" />;
}
