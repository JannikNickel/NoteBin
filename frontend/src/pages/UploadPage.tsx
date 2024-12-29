import "../css/upload.css";
import { useState, useRef, ChangeEvent } from "react";
import { useNavigate } from "react-router-dom";
import ToastContainer from "../components/ToastContainer";
import SyntaxSelector from "../components/SyntaxSelector";
import showErrorToast from "../utils/toast-utils";
import { ProgrammingLanguage, languages } from "../language";
import { apiRequest, NoteCreateRequest, NoteCreateResponse } from "../api";

const UploadPage: React.FC = () => {
    const [language, setLanguage] = useState<ProgrammingLanguage>(languages[0]);
    const [submitting, setSubmitting] = useState<boolean>(false);
    const codeEditorRef = useRef<HTMLTextAreaElement>(null);
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
                <textarea
                    ref={codeEditorRef}
                    className="text-area"
                    placeholder="..." />
            </div>
            <button
                className="flex fixed justify-center items-center text-4xl w-12 h-12 bottom-6 right-6 bg-primary-color rounded-lg"
                onClick={handleSubmit}
                disabled={submitting}>
                    +
            </button>
            <SyntaxSelector
                className="syntax-dropdown fixed bottom-6 right-20 rounded-lg"
                selectedLanguage={language}
                languages={languages}
                onChange={handleSyntaxChange} />
            <ToastContainer />
        </>
    )
};

export default UploadPage;
