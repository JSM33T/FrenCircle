// app/changelog/page.tsx
import { Metadata } from 'next';

import ChangelogClient from './ChangelogClient';
import { defaultMetadata } from '@/lib/defaultMetadata';


export const generateMetadata = async (): Promise<Metadata> => {
    return {
        ...defaultMetadata,
        title: 'Changelog - FrenCircle',
        description: 'Learn more about MyApp and our mission.',
    };
};

export default function ChangelogPage() {
    return <ChangelogClient />;
}
