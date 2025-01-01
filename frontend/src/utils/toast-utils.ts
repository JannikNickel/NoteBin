import { toast } from "react-toastify";

export const showErrorToast = (err: string, options?: object): void => {
    toast.error(err, {
        ...options
    });
}

export const showSuccessToast = (msg: string, options?: object): void => {
    toast.success(msg, {
        ...options
    });
};
