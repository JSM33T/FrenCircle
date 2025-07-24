'use client';

import { useLanguage } from '@/contexts/language-context';
import { ReactNode } from 'react';

interface TranslationGuardProps {
    children: ReactNode;
    fallback?: ReactNode;
}

export function TranslationGuard({ children, fallback }: TranslationGuardProps) {
    const { isLoading } = useLanguage();

    if (isLoading) {
        return (
            fallback || (
                <div className="flex items-center justify-center min-h-screen">
                    <div className="animate-spin rounded-full h-8 w-8 border-b-2 border-gray-900 dark:border-white"></div>
                </div>
            )
        );
    }

    return <>{children}</>;
}
