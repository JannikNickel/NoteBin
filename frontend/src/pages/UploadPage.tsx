import "../css/upload.css";
import "../css/toolbar.css";
import React, { useState, useRef, ChangeEvent, useEffect } from "react";
import { useNavigate, useSearchParams } from "react-router-dom";
import CodeEditor, { CodeEditorRef } from "../components/CodeEditor";
import SyntaxSelector from "../components/SyntaxSelector";
import ToastContainer from "../components/ToastContainer";
import { showErrorToast } from "../utils/toast-utils";
import { ProgrammingLanguage, languages } from "../language";
import { apiRequest, Note, NoteCreateRequest, NoteCreateResponse } from "../api";
import { getUser } from "../utils/storage";

const UploadPage: React.FC = () => {
    const [language, setLanguage] = useState<ProgrammingLanguage>(languages[0]);
    const [title, setTitle] = useState<string>("");
    const [titleInputFocused, setTitleInputFocused] = useState<boolean>(false);
    const [submitting, setSubmitting] = useState<boolean>(false);
    const [isAuthenticated, setIsAuthenticated] = useState<boolean | null>(null);
    const [isLoadingFork, setIsLoadingFork] = useState<boolean>(false);
    const codeEditorRef = useRef<CodeEditorRef>(null);
    const titleInputRef = useRef<HTMLInputElement>(null);
    const navigate = useNavigate();
    const [searchParams] = useSearchParams();
    const forkId = searchParams.get("fork");

    const handleSyntaxChange = (e: ChangeEvent<HTMLSelectElement>): void => {
        let lang = languages.find(lang => lang.id === e.target.value) as ProgrammingLanguage;
        setLanguage(lang);
    };

    const handleTitleChange = (e: ChangeEvent<HTMLInputElement>): void => {
        setTitle(e.target.value);
    };

    const handleSubmit = async (): Promise<void> => {
        if (submitting || isAuthenticated === null) {
            return;
        }

        setSubmitting(true);
        try {
            const requestBody: NoteCreateRequest = {
                name: title.trim() !== "" ? title : undefined,
                fork: forkId || undefined,
                syntax: language.id,
                content: codeEditorRef.current?.value || ""
            };
            const response = await apiRequest<NoteCreateRequest, NoteCreateResponse>("/api/note", requestBody, {
                method: "POST"
            }, isAuthenticated !== null);
            if (response.ok) {
                navigate(`/note/${response.value.id}`);
            } else {
                showErrorToast(response.error.message);
            }
        } finally {
            setSubmitting(false);
        }
    };

    const handleAccount = (): void => {
        if (isAuthenticated) {
            navigate(`/user/${getUser()}`);
        } else {
            navigate("/login");
        }
    };

    useEffect(() => {
        const validateUser = async () => {
            const response = await apiRequest<{}, {}>("/api/auth", {}, { method: "GET" }, true);
            setIsAuthenticated(response.ok);
        };
        validateUser();
    }, []);
    
    useEffect(() => {
        if (titleInputRef.current) {
            titleInputRef.current.style.width = `${titleInputRef.current.value.length + 5}ch`;
        }
    }, [title]);

    useEffect(() => {
        const fetchData = async () => {
            const response = await apiRequest<{}, Note>(`/api/note/${forkId}`, {}, {
                method: "GET"
            });
            if (response.ok) {
                const note = response.value;
                setTitle(note.name);
                setLanguage(languages.find(lang => lang.id === note.syntax) || languages[0]);
                if (codeEditorRef.current) {
                    codeEditorRef.current.value = note.content || "";
                }
            }
            setIsLoadingFork(false);
        };

        if (forkId) {
            fetchData();
        }
    }, [forkId]);

    if (forkId && isLoadingFork) {
        return <p className="p-2">Loading...</p>
    }

    return (
        <>
            <div className="flex justify-center items-center p-0">
                <CodeEditor
                    reference={codeEditorRef}
                    className="text-area"
                    placeholder="..." />
            </div>
            <div className="toolbar">
                <button
                    className="toolbar-element primary"
                    onClick={handleSubmit}
                    disabled={submitting || isAuthenticated === null}>
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
                    onChange={handleTitleChange} />
                <button
                    className="toolbar-element secondary"
                    onClick={handleAccount}
                    disabled={submitting}>
                        [{isAuthenticated && getUser() ? getUser() : "LOGIN"}]
                </button>
            </div>
            <ToastContainer />
        </>
    )
};

export default UploadPage;
