'use client'

import * as React from "react"
import { motion, AnimatePresence } from "framer-motion"


export interface AccordionItem {
	question: string
	answer: string
}

// Sample FAQ items
export const sampleAccordionItems: AccordionItem[] = [
	{
		question: "What is FrenCircle?",
		answer: "FrenCircle is a social platform to connect and collaborate with your friends and communities.",
	},
	{
		question: "How do I join a circle?",
		answer: "You can join a circle by accepting an invite or searching for public circles in the app.",
	},
	{
		question: "Is FrenCircle free to use?",
		answer: "Yes, FrenCircle offers a free plan with optional premium features.",
	},
	{
		question: "How do I reset my password?",
		answer: "Go to your account settings and click on 'Reset Password'. Follow the instructions sent to your email.",
	},
]


interface AccordionProps {
	items: AccordionItem[]
	openIndexes?: number[]
}

export function Accordion({ items, openIndexes: defaultOpenIndexes = [] }: AccordionProps) {
	const [openIndex, setOpenIndex] = React.useState<number | null>(
		defaultOpenIndexes.length > 0 ? defaultOpenIndexes[0] : null
	);

	const toggleIndex = (idx: number) => {
		setOpenIndex((prev) => (prev === idx ? null : idx));
	};

	return (
		<div className="space-y-2">
			{items.map((item, idx) => {
				const isOpen = openIndex === idx;
				return (
					<div key={idx} className="border rounded-md bg-white dark:bg-gray-900 overflow-hidden">
						<button
							className="w-full flex justify-between items-center px-4 py-3 text-left font-medium text-gray-900 dark:text-white focus:outline-none"
							onClick={() => toggleIndex(idx)}
							aria-expanded={isOpen}
						>
							<span>{item.question}</span>
							<span className="ml-2">{isOpen ? "-" : "+"}</span>
						</button>
						<AnimatePresence initial={false}>
							{isOpen && (
								<motion.div
									key="content"
									initial={{ height: 0, opacity: 0 }}
									animate={{ height: "auto", opacity: 1 }}
									exit={{ height: 0, opacity: 0 }}
									transition={{ duration: 0.18, ease: [0.4, 0, 0.2, 1] }}
									className="px-4 pb-4 text-gray-700 dark:text-gray-300"
								>
									{item.answer}
								</motion.div>
							)}
						</AnimatePresence>
					</div>
				);
			})}
		</div>
	);
}


