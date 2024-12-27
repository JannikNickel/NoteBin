import { toast } from "react-toastify";

const showErrorToast = (err: string, options?: object): void => {
    toast.error(err, {
        ...options
    });
}

export default showErrorToast;
