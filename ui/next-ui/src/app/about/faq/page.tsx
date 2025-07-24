"use client";
import * as React from "react";
import { Accordion, sampleAccordionItems } from "@/components/ui/accordion";
import { Input } from "@/components/ui/input";

export default function About() {
	const [search, setSearch] = React.useState("");
	const filteredItems = React.useMemo(() => {
		if (!search.trim()) return sampleAccordionItems;
		return sampleAccordionItems.filter(
			(item) =>
				item.question.toLowerCase().includes(search.toLowerCase()) ||
				item.answer.toLowerCase().includes(search.toLowerCase())
		);
	}, [search]);

	return (
		<div className="min-h-screen bg-gradient-to-br from-gray-50 via-white to-gray-50 dark:from-gray-950 dark:via-gray-900 dark:to-gray-950">
			<section className="container mx-auto px-4 py-20">
				<div className="mx-auto max-w-6xl">
					<div className="mb-16 text-center">
						<h2 className="mb-4 text-3xl font-bold tracking-tight text-gray-900 dark:text-white sm:text-4xl">
							Faqs
						</h2>
						<p className="text-xl text-gray-600 dark:text-gray-300">
							Frequently asked questions about us.
						</p>
					</div>
					<div className="max-w-xl mx-auto mb-8">
						<Input
							placeholder="Search FAQs..."
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
