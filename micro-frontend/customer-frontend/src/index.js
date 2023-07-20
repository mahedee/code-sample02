import React from "react";
import ReactDOM from "react-dom/client";
//import ReactDOM from "react-dom";
import "./index.css";
import App from "./App";
import reportWebVitals from "./reportWebVitals";

window.renderCustomer = (masterId, history) => {
  const customerElement = ReactDOM.createRoot(
    document.getElementById(masterId)
  );

  customerElement.render(
    <React.StrictMode>
      <App history={history} />
    </React.StrictMode>
  );
};

window.unmountCustomer = (masterId) => {
  // unmount of remove component
  //debugger;
  //ReactDOM.unmountComponentAtNode(document.getElementById(masterId));
};

if (!document.getElementById("Customer-master")) {
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
