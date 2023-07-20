import logo from "./logo.svg";
import "./App.css";
import "./App.css";
import { BrowserRouter, Route, Routes } from "react-router-dom";
import CustomerPage from "./pages/CustomerPage";
import { createBrowserHistory } from "history";

const defaultHistory = createBrowserHistory();
function App({ history = defaultHistory }) {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<CustomerPage />} />
        <Route path="/customers" element={<CustomerPage />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;
