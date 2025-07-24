import { LanguageCode, loadTranslations } from '@/lib/i18n';

// Preload common languages in the background
export async function preloadTranslations(languages: LanguageCode[] = ['en', 'hi', 'ko']) {
    const preloadPromises = languages.map(async (lang) => {
        try {
            await loadTranslations(lang);
            console.log(`✓ Preloaded translations for ${lang}`);
        } catch (error) {
            console.warn(`Failed to preload translations for ${lang}:`, error);
        }
    });

    return Promise.allSettled(preloadPromises);
}

// Preload translations on idle
export function preloadTranslationsOnIdle() {
    if (typeof window !== 'undefined' && 'requestIdleCallback' in window) {
        window.requestIdleCallback(() => {
            preloadTranslations();
        });
    } else {
        // Fallback for browsers without requestIdleCallback
        setTimeout(() => {
            preloadTranslations();
        }, 1000);
    }
}
