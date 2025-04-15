import eslintPluginVue from "eslint-plugin-vue";
import typescriptEslintPlugin from "@typescript-eslint/eslint-plugin";
import typescriptParser from "@typescript-eslint/parser";
import vueParser from "vue-eslint-parser";
import globals from "globals";

export default [
  {
    files: ["**/*.vue", "**/*.ts"], // Target Vue and TypeScript files
    languageOptions: {
      ecmaVersion: 2021,
      sourceType: "module",
      parser: vueParser,
      parserOptions: {
        parser: typescriptParser,
      },
      globals: {
        ...globals.browser,
        ...globals.es2021,
      },
    },
    plugins: {
      vue: eslintPluginVue,
      "@typescript-eslint": typescriptEslintPlugin,
    },
    rules: {
      "vue/multi-word-component-names": "off",
      "@typescript-eslint/no-explicit-any": "warn",
    },
  },
];
