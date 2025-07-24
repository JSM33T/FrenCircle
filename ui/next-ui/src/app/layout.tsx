import type { Metadata } from "next";
import { Inter, JetBrains_Mono, Montserrat } from "next/font/google";
import "./globals.css";
import { Navbar } from "@/components/shared/navbar";
import { ThemeProvider } from "@/components/theme-provider";
import { LanguageProvider } from "@/contexts/language-context";
import { TranslationGuard } from "@/components/translation-guard";
import OffcanvasSidebar from "@/components/ui/offcanvas-sidebar";

const inter = Inter({
	variable: "--font-inter",
	subsets: ["latin"],
	display: "swap",
	preload: true,
});

const montserrat = Montserrat({
	variable: "--font-montserrat",
	subsets: ["latin"],
	display: "swap",
	preload: true,
});

const jetbrainsMono = JetBrains_Mono({
	variable: "--font-jetbrains-mono",
	subsets: ["latin"],
	display: "swap",
	preload: true,
});

export const metadata: Metadata = {
	title: "FrenCircle - Modern Web Application",
	description: "A modern web application built with Next.js and shadcn/ui",
};

export default function RootLayout({
	children,
}: Readonly<{
	children: React.ReactNode;
}>) {
	return (
		<html lang="en" suppressHydrationWarning>
			<head>
				<link rel="preconnect" href="https://fonts.googleapis.com" />
				<link rel="preconnect" href="https://fonts.gstatic.com" crossOrigin="anonymous" />
			</head>
			<body
				className={`${inter.variable} ${montserrat.variable} ${jetbrainsMono.variable} antialiased`}
			>
				<ThemeProvider>
					<LanguageProvider>
						<TranslationGuard>
							<Navbar />
							<main className="min-h-screen">{children}</main>
							<OffcanvasSidebar />
						</TranslationGuard>
					</LanguageProvider>
				</ThemeProvider>
			</body>
		</html>
	);
}
