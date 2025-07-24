"use client"

import { Badge } from "@/components/ui/badge";
import { Button } from "@/components/ui/button";

import {
	Card,
	CardContent,
	CardDescription,
	CardFooter,
	CardHeader,
	CardTitle,
} from "@/components/ui/card";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { DatePickerWithRange } from "@/components/ui/date-range-picker";
import { ArrowRight, Shield, Zap, Users, BarChart3 } from "lucide-react";
import { useLanguage } from "@/contexts/language-context";

import Link from "next/link";
import { useState } from "react";
import { motion } from "framer-motion";
import { DateRange } from "react-day-picker";

export default function Home() {
	const [date, setDate] = useState<DateRange | undefined>({
		from: new Date(),
		to: undefined,
	});
	const { t } = useLanguage();

	return (
		<div className="min-h-screen bg-gradient-to-br from-gray-50 via-white to-gray-50 dark:from-gray-950 dark:via-gray-900 dark:to-gray-950">
			{/* Hero Section */}
			{/* <section className="container mx-auto px-4 py-20 text-center"> */}
			<section className="container mx-auto py-20 text-center">
				<div className="mx-auto max-w-4xl">
					<Badge className="mb-6" variant="secondary">
						FrenCircle
					</Badge>
					<motion.h1
						className="mb-6 text-4xl font-bold tracking-tight text-gray-900 dark:text-white sm:text-6xl"
						initial="hidden"
						animate="visible"
						variants={{
							hidden: {},
							visible: {
								transition: {
									staggerChildren: 0.18
								}
							}
						}}
					>
						<motion.span
							variants={{
								hidden: { opacity: 0, y: 40 },
								visible: { opacity: 1, y: 0, transition: { duration: 0.6, ease: "easeOut" } }
							}}
							style={{ display: "inline-block", marginRight: 8 }}
						>
							{t('welcome')}
						</motion.span>
					</motion.h1>
					<p className="mb-8 text-xl text-gray-600 dark:text-gray-300 sm:text-2xl">
						{t('description')}
					</p>

					{/* Date Range Picker */}
					<div className="mb-8 flex justify-center">
						<DatePickerWithRange
							date={date}
							onDateChange={setDate}
							placeholder="Select date range for events"
						/>
					</div>

					<div className="flex flex-col gap-4 sm:flex-row sm:justify-center">
						<Button size="lg" className="text-md">
							Get Started Free
							<ArrowRight className="ml-2 h-5 w-5" />
						</Button>

						<Link href="/about">
							<Button size="lg" variant="outline" className="text-md">
								{t('about')}
							</Button>
						</Link>
					</div>
				</div>
			</section>
		</div>
	);
}
