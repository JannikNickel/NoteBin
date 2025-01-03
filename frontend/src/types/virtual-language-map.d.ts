declare module "virtual:hljs-lang-import-map" {
    const hljsLangImportMap: Record<string, () => Promise<{ default: any }>>;
    export default hljsLangImportMap;
}
