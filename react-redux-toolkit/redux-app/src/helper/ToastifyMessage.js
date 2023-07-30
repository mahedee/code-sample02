import { toast } from "react-toastify";

export const SuccessToastify = (message) => {
  toast.success(message, {
    position: toast.POSITION.TOP_CENTER,
    autoClose: 3000, // auto close after 30 seconds
  });
};

export const WarningTostify = (message) => {
  toast.warning(message, {
    position: toast.POSITION.TOP_RIGHT,
    autoClose: 3000, // auto close after 30 seconds
  });
};

export const ErrorToastify = (message) => {
  toast.error(message, {
    position: toast.POSITION.TOP_RIGHT,
    autoClose: 3000, // auto close after 30 seconds
  });
};

export const InfoToastify = (message) => {
  toast.info(message, {
    position: toast.POSITION.TOP_RIGHT,
    autoClose: 3000, // auto close after 30 seconds
  });
};
