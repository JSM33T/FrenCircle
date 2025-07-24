"use client"

import { createContext, useContext, useEffect, useState } from "react"

export type AccentColor =
    | "blue"
    | "green"
    | "purple"
    | "red"
    | "orange"
    | "yellow"
    | "pink"
    | "emerald"
    | "cyan"
    | "violet"

export type BorderRadius = "none" | "sm" | "md" | "lg" | "xl" | "full"

export type FontFamily = "inter" | "system" | "mono" | "montserrat"

export interface ThemeSettings {
    accentColor: AccentColor
    borderRadius: BorderRadius
    fontFamily: FontFamily
    reducedMotion: boolean
    highContrast: boolean
    colorBlindFriendly: boolean
}

interface ThemeSettingsContextType {
    settings: ThemeSettings
    updateSetting: <K extends keyof ThemeSettings>(key: K, value: ThemeSettings[K]) => void
    resetSettings: () => void
}

const defaultSettings: ThemeSettings = {
    accentColor: "blue",
    borderRadius: "md",
    fontFamily: "inter",
    reducedMotion: false,
    highContrast: false,
    colorBlindFriendly: false,
}

const ThemeSettingsContext = createContext<ThemeSettingsContextType | undefined>(undefined)

export function ThemeSettingsProvider({ children }: { children: React.ReactNode }) {
    const [settings, setSettings] = useState<ThemeSettings>(defaultSettings)

    // Load settings from localStorage on mount
    useEffect(() => {
        const saved = localStorage.getItem("theme-settings")
        if (saved) {
            try {
                const parsed = JSON.parse(saved)
                setSettings({ ...defaultSettings, ...parsed })
            } catch {
                // If parsing fails, use defaults
            }
        }
    }, [])

    // Save settings to localStorage whenever they change
    useEffect(() => {
        localStorage.setItem("theme-settings", JSON.stringify(settings))

        // Apply CSS custom properties
        const root = document.documentElement

        // Apply accent color
        root.setAttribute("data-accent", settings.accentColor)

        // Apply border radius
        root.setAttribute("data-radius", settings.borderRadius)

        // Apply font family
        root.setAttribute("data-font", settings.fontFamily)

        // Apply accessibility settings
        root.setAttribute("data-reduced-motion", settings.reducedMotion.toString())
        root.setAttribute("data-high-contrast", settings.highContrast.toString())
        root.setAttribute("data-colorblind-friendly", settings.colorBlindFriendly.toString())

    }, [settings])

    const updateSetting = <K extends keyof ThemeSettings>(
        key: K,
        value: ThemeSettings[K]
    ) => {
        setSettings(prev => ({ ...prev, [key]: value }))
    }

    const resetSettings = () => {
        setSettings(defaultSettings)
    }

    return (
        <ThemeSettingsContext.Provider value={{ settings, updateSetting, resetSettings }}>
            {children}
        </ThemeSettingsContext.Provider>
    )
}

export function useThemeSettings() {
    const context = useContext(ThemeSettingsContext)
    if (context === undefined) {
        throw new Error("useThemeSettings must be used within a ThemeSettingsProvider")
    }
    return context
}
