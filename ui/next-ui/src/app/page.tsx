import { Badge } from "@/components/ui/badge";
import { Button } from "@/components/ui/button";

import { ArrowRight } from "lucide-react";
import Link from "next/link";

export default function Home() {
	return (
		<div className="min-h-screen bg-gradient-to-br from-gray-50 via-white to-gray-50 dark:from-gray-950 dark:via-gray-900 dark:to-gray-950">
			{/* Hero Section */}
			{/* <section className="container mx-auto px-4 py-20 text-center"> */}
			<section className="container mx-auto py-20 text-center">
				<div className="mx-auto max-w-4xl">
					<Badge className="mb-6" variant="secondary">
						FrenCircle
					</Badge>
					<h1 className="mb-6 text-4xl font-bold tracking-tight text-gray-900 dark:text-white sm:text-6xl">
						Connect, Collaborate,{" "}
						<span className="bg-gradient-to-r from-blue-600 to-purple-600 bg-clip-text text-transparent">
							Create Together
						</span>
					</h1>
					<p className="mb-8 text-xl text-gray-600 dark:text-gray-300 sm:text-2xl">
						The ultimate platform for building meaningful connections and driving collaboration across teams and communities.
					</p>
					<div className="flex flex-col gap-4 sm:flex-row sm:justify-center">
						<Button size="lg" className="text-md">
							Get Started Free
							<ArrowRight className="ml-2 h-5 w-5" />
						</Button>

						<Link href="/about" legacyBehavior>
							<Button size="lg" variant="outline" className="text-md">
								About
							</Button>
						</Link>
					</div>
				</div>
			</section>
		</div>
	);
}
