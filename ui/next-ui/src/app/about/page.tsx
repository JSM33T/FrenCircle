'use client';
import { motion } from "framer-motion";
import { Button } from "@/components/ui/button";
import { useLanguage } from "@/contexts/language-context";
import {
	Card,
	CardContent,
	CardDescription,
	CardFooter,
	CardHeader,
	CardTitle,
} from "@/components/ui/card";

const tilt = { rotate: 2, scale: 1.03 };
const lift = { y: -5, scale: 1.05 };

export default function About() {
	const { t } = useLanguage();

	return (
		<div className="min-h-screen bg-gradient-to-br from-gray-50 via-white to-gray-50 dark:from-gray-950 dark:via-gray-900 dark:to-gray-950">
			<section className="container mx-auto px-4 py-20">
				<div className="mx-auto max-w-6xl">
					<div className="mb-16 text-center">
						<h2 className="mb-4 text-3xl font-bold tracking-tight text-gray-900 dark:text-white sm:text-4xl">
							{t('aboutUs')}
						</h2>
						<p className="text-xl text-gray-600 dark:text-gray-300">
							{t('aboutDescription')}
						</p>
					</div>
					<div className="grid gap-8 md:grid-cols-2 lg:grid-cols-4">
						{[
							{
								title: t('faq'),
								desc: t('faqDescription'),
								features: [
									"REST & GraphQL APIs",
									"Webhooks",
									"SDKs for popular languages",
								],
								button: <Button variant="outline">View Docs</Button>,
							},
							{
								title: "Free Forever",
								desc: "Start for free, no credit card required. Upgrade anytime.",
								features: [
									"Generous free tier",
									"No hidden fees",
									"Cancel anytime",
								],
								button: <Button variant="outline">Get Started</Button>,
							},
							{
								title: "Scalable & Reliable",
								desc: "Enterprise-grade infrastructure for teams of any size.",
								features: [
									"99.99% uptime SLA",
									"Auto-scaling",
									"Global CDN",
								],
								button: <Button variant="link">Learn More</Button>,
							},
							{
								title: "Secure by Design",
								desc: "Your data is protected with best-in-class security features.",
								features: [
									"End-to-end encryption",
									"GDPR & SOC2 compliant",
									"SSO & 2FA support",
								],
								button: <Button variant="outline">Security Details</Button>,
							},
						].map((item, i) => (
							<motion.div
								key={i}
								whileHover={tilt}
								transition={{ type: "spring", stiffness: 200, damping: 10 }}
								className="w-full max-w-sm mx-auto"
							>
								<Card>
									<CardHeader>
										<CardTitle>{item.title}</CardTitle>
										<CardDescription>{item.desc}</CardDescription>
									</CardHeader>
									<CardContent>
										<ul className="text-sm text-gray-600 dark:text-gray-300 space-y-2">
											{item.features.map((f, idx) => (
												<li key={idx}>• {f}</li>
											))}
										</ul>
									</CardContent>
									<CardFooter>
										<motion.div whileHover={lift} className="w-full">
											{item.button}
										</motion.div>
									</CardFooter>
								</Card>
							</motion.div>
						))}
					</div>
				</div>
			</section>
		</div>
	);
}
