export const languages = {
    en: {
        code: 'en',
        name: 'English',
        flag: '🇺🇸'
    },
    hi: {
        code: 'hi',
        name: 'हिंदी',
        flag: '🇮🇳'
    },
    ko: {
        code: 'ko',
        name: '한국어',
        flag: '🇰🇷'
    }
} as const;

export type LanguageCode = keyof typeof languages;

export const defaultLanguage: LanguageCode = 'en';

// Translation interface to ensure consistency across all language files
export interface Translations {
    // Navigation
    home: string;
    about: string;
    faq: string;
    login: string;

    // Common
    language: string;
    theme: string;
    light: string;
    dark: string;
    system: string;

    // About page
    aboutUs: string;
    aboutDescription: string;

    // FAQ
    frequentlyAskedQuestions: string;
    faqDescription: string;
    searchFaqPlaceholder: string;

    // FAQ Items
    faqItems: {
        question: string;
        answer: string;
    }[];

    // General
    welcome: string;
    description: string;
    browseProducts: string;
    latestProducts: string;
}

export type TranslationKey = keyof Translations;

// Dynamic import function for translations
export async function loadTranslations(language: LanguageCode): Promise<Translations> {
    try {
        const translations = await import(`./translations/${language}.ts`);
        return translations.default;
    } catch (error) {
        console.warn(`Failed to load translations for ${language}, falling back to English`);
        const fallback = await import(`./translations/${defaultLanguage}.ts`);
        return fallback.default;
    }
}
