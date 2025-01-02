import "../css/toast.css";
import { ToastContainer as TContainer, Bounce } from "react-toastify";

export interface ToastContainerProps {
    className?: string;
};

const ToastContainer: React.FC<ToastContainerProps> = ({ className }) => (
    <TContainer
        className={"toast-container " + (className || "")}
        position="bottom-right"
        autoClose={5000}
        hideProgressBar={false}
        closeOnClick={true}
        pauseOnHover={true}
        pauseOnFocusLoss={true}
        draggable={false}
        theme="colored"
        transition={Bounce} />
);

export default ToastContainer;
