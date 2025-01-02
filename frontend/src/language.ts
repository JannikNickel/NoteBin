import langs from "./assets/languages.json";

export type ProgrammingLanguage = {
    id: string;
    display: string;
};

export const getLanguageDisplayName = (id: string): string => {
    const language = languages.find(lang => lang.id === id);
    return language?.display.toUpperCase() || id.toUpperCase();
};

export const languages: ProgrammingLanguage[] = langs;
