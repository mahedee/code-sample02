import React from "react";
import ReactDOM from "react-dom/client";
//import ReactDOM from "react-dom";
import "./index.css";
import App from "./App";
import reportWebVitals from "./reportWebVitals";

window.renderProduct = (masterId, history) => {
  const root = ReactDOM.createRoot(document.getElementById(masterId));

  root.render(
    <React.StrictMode>
      <App history={history} />
    </React.StrictMode>
  );
};

window.unmountProduct = (masterId) => {
  //debugger;
  const root = document.getElementById(masterId);
  //debugger;
  //root.unmount();
  //root.remove();
  //ReactDOM.unmountComponentAtNode(document.getElementById(masterId));
};

if (!document.getElementById("Product-master")) {
  const root = ReactDOM.createRoot(document.getElementById("root"));
  root.render(
    <React.StrictMode>
      <App />
    </React.StrictMode>
  );
}

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
