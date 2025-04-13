type ToastType = "success" | "error" | "info";

interface Toast {
  title: string;
  message: string;
  type: ToastType;
}

let _showToast: (toast: Toast) => void = () => {};

export const setToastHandler = (handler: typeof _showToast) => {
  _showToast = handler;
};

export const showToast = (toast: Toast) => {
  _showToast(toast);
};
