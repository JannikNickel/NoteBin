import React, { ChangeEvent } from "react"
import { ProgrammingLanguage } from "../language";

interface SyntaxSelectorProps {
    className?: string;
    selectedLanguage: ProgrammingLanguage;
    languages: ProgrammingLanguage[];
    onChange: (e: ChangeEvent<HTMLSelectElement>) => void;
}

const SyntaxSelector: React.FC<SyntaxSelectorProps> = ({ className, selectedLanguage, languages, onChange }) => (
    <select
        className={className}
        value={selectedLanguage.id}
        onChange={onChange}
    >
        {languages.map((lang) => (
            <option key={lang.id} value={lang.id}>
                {lang.display.toUpperCase()}
            </option>
        ))};
    </select>
);

export default SyntaxSelector;
