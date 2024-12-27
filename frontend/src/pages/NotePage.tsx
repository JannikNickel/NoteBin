import "../css/note.css"
import { useParams } from "react-router-dom";

const NotePage: React.FC = () => {
    const { id } = useParams<{ id: string }>();

    return (
        <>
            <div className="flex justify-center items-center p-0">
                <p>{id}</p>
            </div>
        </>
    )
};

export default NotePage;
