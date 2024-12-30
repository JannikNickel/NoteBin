import "../css/upload.css";
import { useState, useRef, ChangeEvent, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import CodeEditor, { CodeEditorRef } from "../components/CodeEditor";
import SyntaxSelector from "../components/SyntaxSelector";
import ToastContainer from "../components/ToastContainer";
import showErrorToast from "../utils/toast-utils";
import { ProgrammingLanguage, languages } from "../language";
import { apiRequest, NoteCreateRequest, NoteCreateResponse } from "../api";

const UploadPage: React.FC = () => {
    const [language, setLanguage] = useState<ProgrammingLanguage>(languages[0]);
    const [title, setTitle] = useState<string>("");
    const [titleInputFocused, setTitleInputFocused] = useState<boolean>(false);
    const [submitting, setSubmitting] = useState<boolean>(false);
    const codeEditorRef = useRef<CodeEditorRef>(null);
    const titleInputRef = useRef<HTMLInputElement>(null);
    const navigate = useNavigate();
    
    useEffect(() => {
        if (titleInputRef.current) {
            titleInputRef.current.style.width = `${titleInputRef.current.value.length + 5}ch`;
        }
    }, [title]);

    const handleSyntaxChange = (e: ChangeEvent<HTMLSelectElement>): void => {
        let lang = languages.find(lang => lang.id === e.target.value) as ProgrammingLanguage;
        setLanguage(lang);
    };

    const handleTitleChange = (e: ChangeEvent<HTMLInputElement>): void => {
        setTitle(e.target.value);
    };

    const handleSubmit = async (): Promise<void> => {
        if (submitting) {
            return;
        }

        setSubmitting(true);
        try {
            const requestBody: NoteCreateRequest = { name: title, syntax: language.id, content: codeEditorRef.current?.value || "" };
            const response = await apiRequest<NoteCreateRequest, NoteCreateResponse>("/api/note", requestBody, {
                method: "POST"
            });
            if (response.ok) {
                navigate(`/note/${response.value.id}`);
            } else {
                showErrorToast(response.error);
            }
        } finally {
            setSubmitting(false);
        }
    };

    const handleAccount = (): void => {
        navigate("/account");
    };

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
                    disabled={submitting}>
                        CREATE
                </button>
                <SyntaxSelector
                    className="toolbar-element secondary"
                    selectedLanguage={language}
                    languages={languages}
                    onChange={handleSyntaxChange} />
                <input
                    ref={titleInputRef}
                    className="toolbar-element secondary text-center min-w-36 max-w-72"
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
                        [LOGIN]
                </button>
            </div>
            <ToastContainer />
        </>
    )
};

export default UploadPage;
