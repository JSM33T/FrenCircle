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
import { ArrowRight, Shield, Zap, Users, BarChart3 } from "lucide-react";

export default function About() {
	return (
		<div className="min-h-screen bg-gradient-to-br from-gray-50 via-white to-gray-50 dark:from-gray-950 dark:via-gray-900 dark:to-gray-950">

			{/* Features Section - SaaS Feature Cards */}
			<section className="container mx-auto px-4 py-20">
				<div className="mx-auto max-w-6xl">
					<div className="mb-16 text-center">
						<h2 className="mb-4 text-3xl font-bold tracking-tight text-gray-900 dark:text-white sm:text-4xl">
							Why choose FrenCircle?
						</h2>
						<p className="text-xl text-gray-600 dark:text-gray-300">
							Modern SaaS features to help you grow and succeed
						</p>
					</div>
					<div className="grid gap-8 md:grid-cols-2 lg:grid-cols-4">
						<Card className="w-full max-w-sm mx-auto">
							<CardHeader>
								<CardTitle>Developer Friendly</CardTitle>
								<CardDescription>
									Built with modern APIs, docs, and integrations for easy development.
								</CardDescription>
							</CardHeader>
							<CardContent>
								<ul className="text-sm text-gray-600 dark:text-gray-300 space-y-2">
									<li>• REST & GraphQL APIs</li>
									<li>• Webhooks</li>
									<li>• SDKs for popular languages</li>
								</ul>
							</CardContent>
							<CardFooter>
								<Button variant="link">View Docs</Button>
							</CardFooter>
						</Card>
						<Card className="w-full max-w-sm mx-auto">
							<CardHeader>
								<CardTitle>Free Forever</CardTitle>
								<CardDescription>
									Start for free, no credit card required. Upgrade anytime.
								</CardDescription>
							</CardHeader>
							<CardContent>
								<ul className="text-sm text-gray-600 dark:text-gray-300 space-y-2">
									<li>• Generous free tier</li>
									<li>• No hidden fees</li>
									<li>• Cancel anytime</li>
								</ul>
							</CardContent>
							<CardFooter>
								<Button variant="outline">Get Started</Button>
							</CardFooter>
						</Card>
						<Card className="w-full max-w-sm mx-auto">
							<CardHeader>
								<CardTitle>Scalable & Reliable</CardTitle>
								<CardDescription>
									Enterprise-grade infrastructure for teams of any size.
								</CardDescription>
							</CardHeader>
							<CardContent>
								<ul className="text-sm text-gray-600 dark:text-gray-300 space-y-2">
									<li>• 99.99% uptime SLA</li>
									<li>• Auto-scaling</li>
									<li>• Global CDN</li>
								</ul>
							</CardContent>
							<CardFooter>
								<Button variant="link">Learn More</Button>
							</CardFooter>
						</Card>
						<Card className="w-full max-w-sm mx-auto">
							<CardHeader>
								<CardTitle>Secure by Design</CardTitle>
								<CardDescription>
									Your data is protected with best-in-class security features.
								</CardDescription>
							</CardHeader>
							<CardContent>
								<ul className="text-sm text-gray-600 dark:text-gray-300 space-y-2">
									<li>• End-to-end encryption</li>
									<li>• GDPR & SOC2 compliant</li>
									<li>• SSO & 2FA support</li>
								</ul>
							</CardContent>
							<CardFooter>
								<Button variant="outline">Security Details</Button>
							</CardFooter>
						</Card>
					</div>
				</div>
			</section>

		</div>
	);
}
