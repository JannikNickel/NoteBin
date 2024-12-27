import "../css/note.css";
import "../css/syntax.css";
import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import hljs from "highlight.js"; //TODO dont load all languages
import { apiRequest, Note } from "../api";

const NotePage: React.FC = () => {
    const { id } = useParams<{ id: string }>();
    const [note, setNote] = useState<Note | null>(null);
    const [loading, setLoading] = useState<boolean>(true);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        const fetchData = async () => {
            setLoading(true);
            setError(null);

            const response = await apiRequest<{}, Note>(`/api/note/${id}`, {}, {
                method: "GET"
            });
            if (response.ok) {
                setNote(response.value);
                setLoading(false);
            } else {
                setError(response.error);
                setLoading(false);
            }
        };

        fetchData();
    }, [id]);

    useEffect(() => {
        if (note?.content) {
            document.querySelectorAll("pre code").forEach((block) => {
                hljs.highlightElement(block as HTMLElement);
            });
        }
    }, [note]);

    if (loading) {
        return <p>Loading...</p> //TODO larger information
    }
    if (error) {
        return <p>Error: {error}</p> // TODO larger information
    }

    let language = `language-${note?.syntax || "plaintext"}`;
    return (
        <>
            <div className="flex justify-center items-center p-0"> 
                <div className="text-container">
                    <pre>
                        <code className={language}>{note?.content}</code>
                    </pre>
                </div>
            </div>
        </>
    )
};

export default NotePage;
