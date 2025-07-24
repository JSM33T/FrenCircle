'use client';

import React, { createContext, useContext, useState, useEffect, useCallback } from 'react';
import { LanguageCode, defaultLanguage, TranslationKey, loadTranslations, Translations } from '@/lib/i18n';
import { preloadTranslationsOnIdle } from '@/lib/preload-translations';

interface LanguageContextType {
    language: LanguageCode;
    setLanguage: (language: LanguageCode) => void;
    t: (key: TranslationKey) => string;
    isLoading: boolean;
}

const LanguageContext = createContext<LanguageContextType | undefined>(undefined);

const LANGUAGE_STORAGE_KEY = 'frencircle-language';

// Cache for loaded translations to avoid re-loading
const translationCache = new Map<LanguageCode, Translations>();

export function LanguageProvider({ children }: { children: React.ReactNode }) {
    const [language, setLanguageState] = useState<LanguageCode>(defaultLanguage);
    const [currentTranslations, setCurrentTranslations] = useState<Translations | null>(null);
    const [isLoading, setIsLoading] = useState(true);

    // Load translations for a specific language
    const loadLanguage = useCallback(async (lang: LanguageCode) => {
        setIsLoading(true);

        // Check cache first
        if (translationCache.has(lang)) {
            setCurrentTranslations(translationCache.get(lang)!);
            setIsLoading(false);
            return;
        }

        try {
            const translations = await loadTranslations(lang);
            translationCache.set(lang, translations);
            setCurrentTranslations(translations);
        } catch (error) {
            console.error(`Failed to load translations for ${lang}:`, error);
            // Fallback to default language if not already trying to load it
            if (lang !== defaultLanguage) {
                await loadLanguage(defaultLanguage);
                return;
            }
        } finally {
            setIsLoading(false);
        }
    }, []);

    // Load language from localStorage on mount and load translations
    useEffect(() => {
        const initializeLanguage = async () => {
            let initialLanguage = defaultLanguage;

            if (typeof window !== 'undefined') {
                const stored = localStorage.getItem(LANGUAGE_STORAGE_KEY) as LanguageCode;
                if (stored && ['en', 'hi', 'ko'].includes(stored)) {
                    initialLanguage = stored;
                }
            }

            setLanguageState(initialLanguage);
            await loadLanguage(initialLanguage);

            // Preload other translations in the background
            preloadTranslationsOnIdle();
        };

        initializeLanguage();
    }, [loadLanguage]);

    const setLanguage = useCallback(async (newLanguage: LanguageCode) => {
        if (newLanguage === language) return;

        setLanguageState(newLanguage);

        if (typeof window !== 'undefined') {
            localStorage.setItem(LANGUAGE_STORAGE_KEY, newLanguage);
        }

        await loadLanguage(newLanguage);
    }, [language, loadLanguage]);

    const t = useCallback((key: TranslationKey): string => {
        if (!currentTranslations) {
            return key; // Return key as fallback while loading
        }
        return currentTranslations[key] || key;
    }, [currentTranslations]);

    return (
        <LanguageContext.Provider value={{ language, setLanguage, t, isLoading }}>
            {children}
        </LanguageContext.Provider>
    );
}

export function useLanguage() {
    const context = useContext(LanguageContext);
    if (context === undefined) {
        throw new Error('useLanguage must be used within a LanguageProvider');
    }
    return context;
}
