"use client"

import * as React from "react"
import { Moon, Sun } from "lucide-react"
import { useTheme } from "next-themes"

import { Button } from "@/components/ui/button"
import { ThemeCustomizer } from "@/components/theme-customizer"

export function ThemeToggle() {
	const { theme, setTheme } = useTheme()
	const [mounted, setMounted] = React.useState(false)

	React.useEffect(() => {
		setMounted(true)
	}, [])

	if (!mounted) {
		return (
			<div className="flex items-center gap-2">
				<Button variant="ghost" size="icon" className="h-9 w-9">
					<div className="h-4 w-4" />
					<span className="sr-only">Toggle theme</span>
				</Button>
				<Button variant="ghost" size="icon" className="h-9 w-9">
					<div className="h-4 w-4" />
					<span className="sr-only">Theme settings</span>
				</Button>
			</div>
		)
	}

	return (
		<div className="flex items-center gap-2">
			<Button
				variant="ghost"
				size="icon"
				className="h-9 w-9"
				onClick={() => setTheme(theme === "dark" ? "light" : "dark")}
			>
				{theme === "dark" ? (
					<Sun className="h-4 w-4 transition-all" />
				) : (
					<Moon className="h-4 w-4 transition-all" />
				)}
				<span className="sr-only">Toggle theme</span>
			</Button>
			<ThemeCustomizer />
		</div>
	)
}
