import "../css/upload.css";
import { useState, useRef, ChangeEvent } from "react";
import { useNavigate } from "react-router-dom";
import CodeEditor, { CodeEditorRef } from "../components/CodeEditor";
import SyntaxSelector from "../components/SyntaxSelector";
import ToastContainer from "../components/ToastContainer";
import showErrorToast from "../utils/toast-utils";
import { ProgrammingLanguage, languages } from "../language";
import { apiRequest, NoteCreateRequest, NoteCreateResponse } from "../api";

const UploadPage: React.FC = () => {
    const [language, setLanguage] = useState<ProgrammingLanguage>(languages[0]);
    const [submitting, setSubmitting] = useState<boolean>(false);
    const codeEditorRef = useRef<CodeEditorRef>(null);
    const navigate = useNavigate();
    
    const handleSyntaxChange = (e: ChangeEvent<HTMLSelectElement>): void => {
        let lang = languages.find(lang => lang.id === e.target.value) as ProgrammingLanguage;
        setLanguage(lang);
    };

    const handleSubmit = async (): Promise<void> => {
        if (submitting) {
            return;
        }

        setSubmitting(true);
        try {
            const requestBody: NoteCreateRequest = { syntax: language.id, content: codeEditorRef.current?.value || "" };
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
            </div>
            <ToastContainer />
        </>
    )
};

export default UploadPage;
