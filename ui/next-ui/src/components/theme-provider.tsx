"use client"

import * as React from "react"
import { ThemeProvider as NextThemesProvider } from "next-themes"
import { ThemeSettingsProvider } from "@/contexts/theme-context"

interface ThemeProviderProps {
	children: React.ReactNode
	attribute?: "class" | "data-theme"
	defaultTheme?: string
	enableSystem?: boolean
	disableTransitionOnChange?: boolean
}

export function ThemeProvider({
	children,
	attribute = "class",
	defaultTheme = "system",
	enableSystem = true,
	disableTransitionOnChange = true,
	...props
}: ThemeProviderProps) {
	return (
		<NextThemesProvider
			attribute={attribute}
			defaultTheme={defaultTheme}
			enableSystem={enableSystem}
			disableTransitionOnChange={disableTransitionOnChange}
			{...props}
		>
			<ThemeSettingsProvider>
				{children}
			</ThemeSettingsProvider>
		</NextThemesProvider>
	)
}
