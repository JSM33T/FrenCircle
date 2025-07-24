"use client"

import * as React from "react"
import { Check, Palette, Radius, Type, Eye, Settings2 } from "lucide-react"
import { useTheme } from "next-themes"

import { Button } from "@/components/ui/button"
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card"
import { Label } from "@/components/ui/label"
import { Switch } from "@/components/ui/switch"
import { Popover, PopoverContent, PopoverTrigger } from "@/components/ui/popover"
import {
    useThemeSettings,
    AccentColor,
    BorderRadius,
    FontFamily
} from "@/contexts/theme-context"

const accentColors: { name: AccentColor; label: string; color: string }[] = [
    { name: "blue", label: "Blue", color: "rgb(59 130 246)" },
    { name: "green", label: "Green", color: "rgb(34 197 94)" },
    { name: "purple", label: "Purple", color: "rgb(147 51 234)" },
    { name: "red", label: "Red", color: "rgb(239 68 68)" },
    { name: "orange", label: "Orange", color: "rgb(249 115 22)" },
    { name: "yellow", label: "Yellow", color: "rgb(234 179 8)" },
    { name: "pink", label: "Pink", color: "rgb(236 72 153)" },
    { name: "emerald", label: "Emerald", color: "rgb(16 185 129)" },
    { name: "cyan", label: "Cyan", color: "rgb(6 182 212)" },
    { name: "violet", label: "Violet", color: "rgb(139 92 246)" },
]

const borderRadiusOptions: { name: BorderRadius; label: string; value: string }[] = [
    { name: "none", label: "None", value: "0px" },
    { name: "sm", label: "Small", value: "4px" },
    { name: "md", label: "Medium", value: "8px" },
    { name: "lg", label: "Large", value: "12px" },
    { name: "xl", label: "Extra Large", value: "16px" },
    { name: "full", label: "Full", value: "9999px" },
]

const fontFamilyOptions: { name: FontFamily; label: string; className: string }[] = [
    { name: "inter", label: "Inter", className: "font-sans" },
    { name: "system", label: "System", className: "font-system" },
    { name: "mono", label: "Monospace", className: "font-mono" },
    { name: "montserrat", label: "Montserrat", className: "font-montserrat" },
]

export function ThemeCustomizer() {
    const { theme, setTheme } = useTheme()
    const { settings, updateSetting, resetSettings } = useThemeSettings()
    const [mounted, setMounted] = React.useState(false)

    React.useEffect(() => {
        setMounted(true)
    }, [])

    if (!mounted) {
        return (
            <Button variant="ghost" size="icon" className="h-9 w-9">
                <Settings2 className="h-4 w-4" />
                <span className="sr-only">Theme settings</span>
            </Button>
        )
    }

    return (
        <Popover>
            <PopoverTrigger asChild>
                <Button variant="ghost" size="icon" className="h-9 w-9">
                    <Settings2 className="h-4 w-4" />
                    <span className="sr-only">Theme settings</span>
                </Button>
            </PopoverTrigger>
            <PopoverContent className="w-96 p-0" align="end">
                <div className="max-h-[80vh] overflow-y-auto">
                    <CardHeader className="pb-4">
                        <CardTitle className="flex items-center gap-2">
                            <Palette className="h-5 w-5" />
                            Theme Customizer
                        </CardTitle>
                        <CardDescription>
                            Customize the appearance and feel of the interface
                        </CardDescription>
                    </CardHeader>

                    <CardContent className="space-y-6">
                        {/* Theme Mode */}
                        <div className="space-y-3">
                            <Label className="text-sm font-medium">Color Mode</Label>
                            <div className="grid grid-cols-3 gap-2">
                                {["light", "dark", "system"].map((mode) => (
                                    <Button
                                        key={mode}
                                        variant={theme === mode ? "default" : "outline"}
                                        size="sm"
                                        onClick={() => setTheme(mode)}
                                        className="capitalize"
                                    >
                                        {mode === theme && <Check className="h-3 w-3 mr-1" />}
                                        {mode}
                                    </Button>
                                ))}
                            </div>
                        </div>

                        {/* Accent Color */}
                        <div className="space-y-3">
                            <Label className="text-sm font-medium flex items-center gap-2">
                                <Palette className="h-4 w-4" />
                                Accent Color
                            </Label>
                            <div className="grid grid-cols-5 gap-2">
                                {accentColors.map((accent) => (
                                    <Button
                                        key={accent.name}
                                        variant="ghost"
                                        size="sm"
                                        onClick={() => updateSetting("accentColor", accent.name)}
                                        className={`h-12 p-1 ${settings.accentColor === accent.name
                                            ? "ring-2 ring-ring ring-offset-2"
                                            : ""
                                            }`}
                                    >
                                        <div
                                            className="w-full h-full rounded-sm flex items-center justify-center"
                                            style={{ backgroundColor: accent.color }}
                                        >
                                            {settings.accentColor === accent.name && (
                                                <Check className="h-3 w-3 text-white" />
                                            )}
                                        </div>
                                    </Button>
                                ))}
                            </div>
                        </div>

                        {/* Border Radius */}
                        <div className="space-y-3">
                            <Label className="text-sm font-medium flex items-center gap-2">
                                <Radius className="h-4 w-4" />
                                Border Radius
                            </Label>
                            <div className="grid grid-cols-2 gap-2">
                                {borderRadiusOptions.map((radius) => (
                                    <Button
                                        key={radius.name}
                                        variant={settings.borderRadius === radius.name ? "default" : "outline"}
                                        size="sm"
                                        onClick={() => updateSetting("borderRadius", radius.name)}
                                        className="justify-start"
                                    >
                                        <div
                                            className="w-4 h-4 bg-foreground mr-2"
                                            style={{ borderRadius: radius.value }}
                                        />
                                        {radius.label}
                                    </Button>
                                ))}
                            </div>
                        </div>

                        {/* Font Family */}
                        <div className="space-y-3">
                            <Label className="text-sm font-medium flex items-center gap-2">
                                <Type className="h-4 w-4" />
                                Font Family
                            </Label>
                            <div className="space-y-2">
                                {fontFamilyOptions.map((font) => (
                                    <Button
                                        key={font.name}
                                        variant={settings.fontFamily === font.name ? "default" : "outline"}
                                        size="sm"
                                        onClick={() => updateSetting("fontFamily", font.name)}
                                        className={`w-full justify-start ${font.className}`}
                                    >
                                        {font.label}
                                    </Button>
                                ))}
                            </div>
                        </div>

                        {/* Accessibility Options */}
                        <Card>
                            <CardHeader className="pb-3">
                                <CardTitle className="text-sm flex items-center gap-2">
                                    <Eye className="h-4 w-4" />
                                    Accessibility
                                </CardTitle>
                            </CardHeader>
                            <CardContent className="space-y-4">
                                <div className="flex items-center justify-between">
                                    <Label htmlFor="reduced-motion" className="text-sm">
                                        Reduce Motion
                                    </Label>
                                    <Switch
                                        id="reduced-motion"
                                        checked={settings.reducedMotion}
                                        onCheckedChange={(checked) => updateSetting("reducedMotion", checked)}
                                    />
                                </div>

                                <div className="flex items-center justify-between">
                                    <Label htmlFor="high-contrast" className="text-sm">
                                        High Contrast
                                    </Label>
                                    <Switch
                                        id="high-contrast"
                                        checked={settings.highContrast}
                                        onCheckedChange={(checked) => updateSetting("highContrast", checked)}
                                    />
                                </div>

                                <div className="flex items-center justify-between">
                                    <Label htmlFor="colorblind-friendly" className="text-sm">
                                        Colorblind Friendly
                                    </Label>
                                    <Switch
                                        id="colorblind-friendly"
                                        checked={settings.colorBlindFriendly}
                                        onCheckedChange={(checked) => updateSetting("colorBlindFriendly", checked)}
                                    />
                                </div>
                            </CardContent>
                        </Card>

                        {/* Reset Button */}
                        <Button
                            variant="outline"
                            onClick={resetSettings}
                            className="w-full"
                        >
                            Reset to Defaults
                        </Button>
                    </CardContent>
                </div>
            </PopoverContent>
        </Popover>
    )
}
