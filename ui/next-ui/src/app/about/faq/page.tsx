"use client";
import * as React from "react";
import { Accordion, AccordionItem } from "@/components/ui/accordion";
import { Input } from "@/components/ui/input";
import { useLanguage } from "@/contexts/language-context";

export default function About() {
	const [search, setSearch] = React.useState("");
	const { language, t } = useLanguage();
	const [faqItems, setFaqItems] = React.useState<AccordionItem[]>([]);

	// Load FAQ items from translations
	React.useEffect(() => {
		const loadFaqItems = async () => {
			try {
				const translations = await import(`@/lib/translations/${language}`);
				setFaqItems(translations.default.faqItems || []);
			} catch (error) {
				console.error('Failed to load FAQ items:', error);
				// Fallback to English
				const englishTranslations = await import(`@/lib/translations/en`);
				setFaqItems(englishTranslations.default.faqItems || []);
			}
		};

		loadFaqItems();
	}, [language]);

	const filteredItems = React.useMemo(() => {
		if (!search.trim()) return faqItems;
		return faqItems.filter(
			(item: AccordionItem) =>
				item.question.toLowerCase().includes(search.toLowerCase()) ||
				item.answer.toLowerCase().includes(search.toLowerCase())
		);
	}, [search, faqItems]);



	return (
		<div className="min-h-screen bg-gradient-to-br from-gray-50 via-white to-gray-50 dark:from-gray-950 dark:via-gray-900 dark:to-gray-950">
			<section className="container mx-auto px-4 py-20">
				<div className="mx-auto max-w-6xl">
					<div className="mb-16 text-center">
						<h2 className="mb-4 text-3xl font-bold tracking-tight text-gray-900 dark:text-white sm:text-4xl">
							{t('frequentlyAskedQuestions')}
						</h2>
						<p className="text-xl text-gray-600 dark:text-gray-300">
							{t('faqDescription')}
						</p>
					</div>
					<div className="max-w-xl mx-auto mb-8">
						<Input
							placeholder={t('searchFaqPlaceholder')}
							value={search}
							onChange={e => setSearch(e.target.value)}
							className="bg-white dark:bg-gray-900"
						/>
					</div>
					<Accordion items={filteredItems} />
				</div>
			</section>
		</div>
	);
}

