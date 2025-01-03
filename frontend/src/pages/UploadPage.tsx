import "../css/upload.css";
import "../css/toolbar.css";
import React, { useState, useRef, ChangeEvent, useEffect } from "react";
import { Link, useNavigate, useSearchParams } from "react-router-dom";
import { ListBulletIcon } from "@heroicons/react/24/outline";
import CodeEditor, { CodeEditorRef } from "../components/CodeEditor";
import SyntaxSelector from "../components/SyntaxSelector";
import ToastContainer from "../components/ToastContainer";
import { showErrorToast } from "../utils/toast-utils";
import { ProgrammingLanguage, getLanguageFromExtension, languages } from "../utils/language";
import { apiRequest, Note, NoteCreateRequest, NoteCreateResponse } from "../utils/api";
import { getUser } from "../utils/storage";

const UploadPage: React.FC = () => {
    const [searchParams] = useSearchParams();
    const forkId = searchParams.get("fork");
    const [language, setLanguage] = useState<ProgrammingLanguage>(languages[0]);
    const [title, setTitle] = useState<string>("");
    const [titleInputFocused, setTitleInputFocused] = useState<boolean>(false);
    const [editorContent, setEditorContent] = useState<string>("");
    const [submitting, setSubmitting] = useState<boolean>(false);
    const [isAuthenticated, setIsAuthenticated] = useState<boolean | null>(null);
    const [isLoadingFork, setIsLoadingFork] = useState<boolean>(forkId !== null);
    const codeEditorRef = useRef<CodeEditorRef>(null);
    const titleInputRef = useRef<HTMLInputElement>(null);
    const navigate = useNavigate();

    const handleSyntaxChange = (e: ChangeEvent<HTMLSelectElement>): void => {
        let lang = languages.find(lang => lang.id === e.target.value) as ProgrammingLanguage;
        setLanguage(lang);
    };

    const handleFileDrop = (filename: string): void => {
        setTitle(filename);
        const extensionIdx = filename.lastIndexOf(".");
        const extension = extensionIdx !== -1 ? filename.substring(extensionIdx) : filename;
        const lang = getLanguageFromExtension(extension);
        setLanguage(lang || languages[0]);
    };

    const handleSubmit = async (): Promise<void> => {
        if (submitting || isAuthenticated === null) {
            return;
        }

        setSubmitting(true);
        try {
            const requestBody: NoteCreateRequest = {
                name: title.trim() || undefined,
                fork: forkId || undefined,
                syntax: language.id,
                content: codeEditorRef.current?.value || ""
            };
            const response = await apiRequest<NoteCreateRequest, NoteCreateResponse>("/api/note", requestBody, {
                method: "POST"
            }, isAuthenticated);
            if (response.ok) {
                navigate(`/note/${response.value.id}`);
            } else {
                showErrorToast(response.error.message);
            }
        } finally {
            setSubmitting(false);
        }
    };

    const validateUser = async (): Promise<void> => {
        const response = await apiRequest<{}, {}>("/api/auth", {}, { method: "GET" }, true);
        setIsAuthenticated(response.ok);
    };

    const fetchFork = async (): Promise<void> => {
        const response = await apiRequest<{}, Note>(`/api/note/${forkId}`, {}, {
            method: "GET"
        });
        if (response.ok) {
            const note = response.value;
            setTitle(note.name);
            setLanguage(languages.find(lang => lang.id === note.syntax) || languages[0]);
            setEditorContent(note.content || "");
        }
        setIsLoadingFork(false);
    };

    useEffect(() => {
        validateUser();
    }, []);
    
    useEffect(() => {
        if (titleInputRef.current) {
            titleInputRef.current.style.width = `${titleInputRef.current.value.length + 5}ch`;
        }
    }, [title]);

    useEffect(() => {
        if (codeEditorRef.current) {
            codeEditorRef.current.value = editorContent;
        }
    }, [editorContent]);

    useEffect(() => {
        if (forkId) {
            fetchFork();
        }
    }, [forkId]);
    
    if (forkId && isLoadingFork) {
        return <p className="status-text">Loading...</p>
    }

    return (
        <>
            <div className="page-root">
                <CodeEditor
                    reference={codeEditorRef}
                    className="text-area"
                    placeholder="..."
                    onFileDrop={handleFileDrop} />
            </div>
            <div className="toolbar">
                <button
                    className="toolbar-element primary"
                    onClick={handleSubmit}
                    disabled={submitting || isAuthenticated === null}
                >
                    CREATE
                </button>
                <SyntaxSelector
                    className="toolbar-element secondary"
                    selectedLanguage={language}
                    languages={languages}
                    onChange={handleSyntaxChange} />
                <input
                    ref={titleInputRef}
                    className="toolbar-element secondary text-center min-w-36 max-w-80"
                    value={title}
                    maxLength={64}
                    placeholder={!titleInputFocused ? "[UNTITLED]" : ""}
                    onFocus={() => setTitleInputFocused(true)}
                    onBlur={() => setTitleInputFocused(false)}
                    onChange={e => setTitle(e.target.value)} />
                <Link className="toolbar-element secondary" to={isAuthenticated ? `/user/${getUser()}` : "/login"}>
                    [{isAuthenticated && getUser() ? getUser() : "LOGIN"}]
                </Link>
                <Link className="toolbar-element secondary" to="/notes">
                    <ListBulletIcon className="h-5 w-5" />
                </Link>
            </div>
            <ToastContainer />
        </>
    )
};

export default UploadPage;
