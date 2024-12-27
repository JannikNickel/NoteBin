import langs from "./assets/languages.json";

export type ProgrammingLanguage = {
    id: string;
    display: string;
};

export const languages: ProgrammingLanguage[] = langs;
