import languages from "../assets/languages.json";

//Dynamically generate import maps for supported hljs languages
const hljsLangImportMapGen = () => ({
    name: "hljs-lang-import-map-gen",
    resolveId(source: string) {
        if (source === "virtual:hljs-lang-import-map") {
            return source;
        }
    },
    load(id: string) {
        if (id === "virtual:hljs-lang-import-map") {
            const imports = languages
                .filter((lang) => lang.id !== "auto")
                .map((lang) => `"${lang.id}": () => import("highlight.js/lib/languages/${lang.id}")`)
                .join(",\n");
            
            return `
                const hljsLangImportMap = {
                    ${imports}
                };
                export default hljsLangImportMap;
            `;
        }
    }
});

export default hljsLangImportMapGen;
