import langs from "./assets/languages.json";

export type ProgrammingLanguage = {
    id: string,
    display: string,
    extensions: string[]
};

export const getLanguageDisplayName = (id: string): string => {
    const language = languages.find(lang => lang.id === id);
    return language?.display.toUpperCase() || id.toUpperCase();
};

export const getLanguageFromExtension = (extension: string): ProgrammingLanguage | null => {
    extension = extension.toLowerCase();
    return languages.find(lang => lang.extensions.includes(extension)) || null;
};

export const languages: ProgrammingLanguage[] = langs;
