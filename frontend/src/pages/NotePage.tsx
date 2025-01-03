import "../css/note.css";
import "../css/syntax.css";
import "../css/toolbar.css";
import React, { useEffect, useState } from "react";
import { Link, useParams } from "react-router-dom";
import { PencilIcon } from "@heroicons/react/24/outline";
import hljs from "highlight.js/lib/core";
import hljsLangImportMap from "virtual:hljs-lang-import-map";
import { apiRequest, Note } from "../utils/api";
import { getLanguageDisplayName, languages, ProgrammingLanguage } from "../utils/language";

const NotePage: React.FC = () => {
    const { id } = useParams<{ id: string }>();
    const [note, setNote] = useState<Note | null>(null);
    const [loading, setLoading] = useState<boolean>(true);
    const [error, setError] = useState<string | null>(null);

    const loadAllLanguages = async (): Promise<void> => {
        const languagePromises = languages.map(async (lang: ProgrammingLanguage) => {
            await loadLanguage(lang.id);
        });
        await Promise.all(languagePromises);
    };

    const loadLanguage = async (id: string): Promise<void> => {
        if (id !== "auto" && !hljs.getLanguage(id)) {
            const langModule = await hljsLangImportMap[id]();
            hljs.registerLanguage(id, langModule.default);
        }
    };

    const fetchNote = async (): Promise<void> => {
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

    const updateSyntaxHighlighting = async (): Promise<void> => {
        if (note?.content) {
            document.querySelectorAll("pre code").forEach((block) => {
                if (note.syntax === "auto") {
                    loadAllLanguages().then(() => {
                        const result = hljs.highlightAuto(note.content || "");
                        block.innerHTML = result.value;
                    });
                } else {
                    loadLanguage(note.syntax).then(() => {
                        hljs.highlightElement(block as HTMLElement);
                    });
                }
            });
        }
    };

    useEffect(() => {
        fetchNote();
    }, [id]);

    useEffect(() => {
        updateSyntaxHighlighting();
    }, [note]);

    if (loading) {
        return <p className="status-text">Loading...</p>
    }
    if (error) {
        return <p className="status-text">Error: {error}</p>
    }

    return (
        <>
            <div className="page-root"> 
                <div className="text-container">
                    <pre>
                        <code className={`language-${note?.syntax || "plaintext"}`}>{note?.content}</code>
                    </pre>
                </div>
                <div className="toolbar">
                    <Link className="toolbar-element primary" to={`/?fork=${id}`}>
                        FORK
                    </Link>
                    <Link className="toolbar-element secondary" to={note?.owner ? `/user/${note?.owner}` : "/notes"}>
                        {note?.owner ? `OWNER: [${note.owner}]` : "[UNOWNED]"}
                    </Link>
                    {note?.fork && 
                        <Link className="toolbar-element secondary" to={`/note/${note.fork}`}>
                            {`FORKED FROM: [${note.fork}]`}
                        </Link>
                    }
                    {note?.name &&
                        <div className="toolbar-element secondary toolbar-element-passive">
                            [{note?.name}]
                        </div>
                    }
                    <div className="toolbar-element secondary toolbar-element-passive">
                        {note?.syntax && getLanguageDisplayName(note?.syntax)}
                    </div>
                    <Link className="toolbar-element secondary" to="/">
                        <PencilIcon className="h-4 w-4" />
                    </Link>
                </div>
            </div>
        </>
    )
};

export default NotePage;
