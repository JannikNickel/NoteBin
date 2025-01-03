import React, { useImperativeHandle, useRef } from "react";

const TAB_SIZE: number = 4;

interface CodeEditorProps {
    reference?: React.Ref<CodeEditorRef>;
    className?: string;
    placeholder?: string;
    value?: string;
    onChange?: (value: string) => void;
    onFileDrop?: (file: string) => void;
}

export interface CodeEditorRef {
    value: string;
}

const CodeEditor: React.FC<CodeEditorProps> = (({ reference, className, placeholder, value, onChange, onFileDrop }) => {
    const editorRef = useRef<HTMLTextAreaElement>(null);

    useImperativeHandle(reference, (): CodeEditorRef => ({
        get value() {
            return editorRef.current?.value || "";
        },
        set value(value: string) {
            setValue(value);
        }
    }));

    const setValue = (value: string): void => {
        if (editorRef.current) {
            editorRef.current.value = value;
            handleChange({ target: editorRef.current } as React.ChangeEvent<HTMLTextAreaElement>);
        }
    };

    const handleChange = (e: React.ChangeEvent<HTMLTextAreaElement>): void => {
        if (onChange) {
            onChange(e.target.value);
        }
    };

    const handleKeyDown = (e: React.KeyboardEvent<HTMLTextAreaElement>): void => {
        if (editorRef.current) {
            let codeEditor: HTMLTextAreaElement = editorRef.current;
            const { selectionStart, selectionEnd } = codeEditor;
            const value = codeEditor.value || "";
            const lineStart = value.lastIndexOf('\n', selectionStart - 1) + 1;
            const indent = selectionStart - lineStart;

            if (e.key === "Tab") {
                e.preventDefault();
                if (!e.shiftKey) {
                    const spaces = TAB_SIZE - (indent % TAB_SIZE);
                    const tab = " ".repeat(spaces);
                    setValue(value.substring(0, selectionStart) + tab + value.substring(selectionEnd));
                    codeEditor.selectionStart = selectionStart + tab.length;
                } else {
                    const lineEnd = value.indexOf('\n', selectionStart);
                    const line = value.substring(lineStart, lineEnd === -1 ? value.length : lineEnd);
                    let leadingSpaces = 0;
                    while (leadingSpaces < line.length && line[leadingSpaces] === " ") {
                        leadingSpaces++;
                    }
                    if (leadingSpaces > 0) {
                        const spaces = leadingSpaces % TAB_SIZE || TAB_SIZE;
                        setValue(value.substring(0, lineStart) + line.substring(spaces) + value.substring(lineEnd === -1 ? value.length : lineEnd));
                        codeEditor.selectionStart = codeEditor.selectionEnd = selectionStart - spaces;
                    }
                }
                codeEditor.selectionEnd = codeEditor.selectionStart;
            } 
        }
    };

    const handleDrop = (e: React.DragEvent<HTMLTextAreaElement>): void => {
        e.preventDefault();
        const files = Array.from(e.dataTransfer.files);
        const file = files[0];
        if (file) {
            const reader = new FileReader();
            reader.onload = (event: ProgressEvent<FileReader>) => {
                const text = event.target?.result as string;
                if (text) {
                    setValue(text);
                    onFileDrop?.(file.name);
                }
            };
            reader.readAsText(file);
        }
    };

    return (
        <textarea
            ref={editorRef}
            className={className}
            placeholder={placeholder}
            value={value}
            onChange={handleChange}
            onKeyDown={handleKeyDown}
            onDrop={handleDrop}
            onDragOver={e => e.preventDefault()} />
    );
});

export default CodeEditor;
