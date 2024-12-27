import "../css/toast.css";
import { ToastContainer as TContainer, Bounce } from "react-toastify";

const ToastContainer: React.FC = () => (
    <TContainer
        className="toast-container"
        position="top-right"
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
