import "../css/note.css";
import "../css/syntax.css";
import "../css/toolbar.css";
import React, { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import hljs from "highlight.js/lib/core";
import hljsLangImportMap from "virtual:hljs-lang-import-map";
import { apiRequest, Note } from "../api";
import { languages, ProgrammingLanguage } from "../language";

const NotePage: React.FC = () => {
    const { id } = useParams<{ id: string }>();
    const [note, setNote] = useState<Note | null>(null);
    const [loading, setLoading] = useState<boolean>(true);
    const [error, setError] = useState<string | null>(null);
    const navigate = useNavigate();

    const loadAllLanguages = async () => {
        const languagePromises = languages.map(async (lang: ProgrammingLanguage) => {
            await loadLanguage(lang.id);
        });
        await Promise.all(languagePromises);
    };

    const loadLanguage = async (id: string) => {
        if (id !== "auto" && !hljs.getLanguage(id)) {
            const langModule = await hljsLangImportMap[id]();
            hljs.registerLanguage(id, langModule.default);
        }
    };

    const handleCreateFork = () => {
        navigate(`/?fork=${id}`);
    };

    const handleOwnerClick = () => {
        if (note?.owner) {
            navigate(`/user/${note.owner}`);
        }
    };

    const handleForkClick = () => {
        if (note?.fork) {
            navigate(`/note/${note.fork}`);
        }
    };

    const getSyntaxDisplayName = (syntax: string) => {
        const language = languages.find(lang => lang.id === syntax);
        return language?.display.toUpperCase() || syntax.toUpperCase();
    };

    useEffect(() => {
        const fetchData = async () => {
            setLoading(true);
            setError(null);

            const response = await apiRequest<{}, Note>(`/api/note/${id}`, {}, {
                method: "GET"
            });
            if (response.ok) {
                setNote(response.value);
            } else {
                setError(response.error.message);
            }
            setLoading(false);
        };

        fetchData();
    }, [id]);

    useEffect(() => {
        if (note?.content) {
            document.querySelectorAll("pre code").forEach((block) => {
                if (note.syntax === "auto") {
                    loadAllLanguages().then(() => {
                        const result = hljs.highlightAuto(note.content);
                        block.innerHTML = result.value;
                    });
                } else {
                    loadLanguage(note.syntax).then(() => {
                        hljs.highlightElement(block as HTMLElement);
                    });
                }
            });
        }
    }, [note]);

    if (loading) {
        return <p className="p-2">Loading...</p>
    }
    if (error) {
        return <p className="p-2">Error: {error}</p>
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
                <div className="toolbar">
                    <button
                        className="toolbar-element primary"
                        onClick={handleCreateFork}>
                            FORK
                    </button>
                    <button
                        className="toolbar-element secondary"
                        onClick={handleOwnerClick}
                        disabled={!note?.owner}>
                            {note?.owner ? `OWNER: [${note.owner}]` : "[UNOWNED]"}
                    </button>
                    {note?.fork && 
                        <button
                            className="toolbar-element secondary"
                            onClick={handleForkClick}>
                                {`FORKED FROM: [${note.fork}]`}
                        </button>
                    }
                    <div className="toolbar-element secondary pointer-events-none select-none">
                        {note?.syntax && getSyntaxDisplayName(note?.syntax)}
                    </div>
                </div>
            </div>
        </>
    )
};

export default NotePage;
