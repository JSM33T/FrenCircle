"use client"

import { Badge } from "@/components/ui/badge"
import { Button } from "@/components/ui/button"
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { Switch } from "@/components/ui/switch"
import { useThemeSettings } from "@/contexts/theme-context"
import {
    Palette,
    Type,
    Radius,
    Eye,
    Settings,
    Star,
    Heart,
    MessageCircle,
    Share2
} from "lucide-react"

export default function ThemeShowcase() {
    const { settings } = useThemeSettings()

    return (
        <div className="min-h-screen bg-background text-foreground p-8">
            <div className="max-w-7xl mx-auto space-y-8">
                {/* Header */}
                <div className="text-center space-y-4">
                    <h1 className="text-4xl font-bold">Theme Showcase</h1>
                    <p className="text-lg text-muted-foreground">
                        Explore the customizable theming system with accent colors, border radius, fonts, and accessibility options.
                    </p>
                    <div className="flex flex-wrap gap-2 justify-center">
                        <Badge variant="secondary">
                            <Palette className="h-3 w-3 mr-1" />
                            {settings.accentColor}
                        </Badge>
                        <Badge variant="secondary">
                            <Radius className="h-3 w-3 mr-1" />
                            {settings.borderRadius}
                        </Badge>
                        <Badge variant="secondary">
                            <Type className="h-3 w-3 mr-1" />
                            {settings.fontFamily}
                        </Badge>
                    </div>
                </div>

                {/* Color Showcase */}
                <Card>
                    <CardHeader>
                        <CardTitle className="flex items-center gap-2">
                            <Palette className="h-5 w-5" />
                            Color System
                        </CardTitle>
                        <CardDescription>
                            Primary, secondary, accent, and other semantic colors
                        </CardDescription>
                    </CardHeader>
                    <CardContent className="space-y-6">
                        {/* Color Swatches */}
                        <div className="grid grid-cols-2 md:grid-cols-4 gap-4">
                            <div className="space-y-2">
                                <div className="h-16 bg-primary rounded-lg border"></div>
                                <p className="text-sm font-medium">Primary</p>
                            </div>
                            <div className="space-y-2">
                                <div className="h-16 bg-secondary rounded-lg border"></div>
                                <p className="text-sm font-medium">Secondary</p>
                            </div>
                            <div className="space-y-2">
                                <div className="h-16 bg-accent rounded-lg border"></div>
                                <p className="text-sm font-medium">Accent</p>
                            </div>
                            <div className="space-y-2">
                                <div className="h-16 bg-destructive rounded-lg border"></div>
                                <p className="text-sm font-medium">Destructive</p>
                            </div>
                        </div>

                        {/* Buttons */}
                        <div className="flex flex-wrap gap-2">
                            <Button>Primary Button</Button>
                            <Button variant="secondary">Secondary</Button>
                            <Button variant="outline">Outline</Button>
                            <Button variant="ghost">Ghost</Button>
                            <Button variant="destructive">Destructive</Button>
                        </div>
                    </CardContent>
                </Card>

                {/* Typography Showcase */}
                <Card>
                    <CardHeader>
                        <CardTitle className="flex items-center gap-2">
                            <Type className="h-5 w-5" />
                            Typography
                        </CardTitle>
                        <CardDescription>
                            Current font family: {settings.fontFamily}
                        </CardDescription>
                    </CardHeader>
                    <CardContent className="space-y-4">
                        <div className="space-y-2">
                            <h1 className="text-4xl font-bold">Heading 1</h1>
                            <h2 className="text-3xl font-semibold">Heading 2</h2>
                            <h3 className="text-2xl font-medium">Heading 3</h3>
                            <h4 className="text-xl">Heading 4</h4>
                            <p className="text-base">
                                Regular paragraph text with <strong>bold</strong> and <em>italic</em> styles.
                                This demonstrates how the selected font family affects all text elements.
                            </p>
                            <p className="text-sm text-muted-foreground">
                                Small text and muted colors for secondary information.
                            </p>
                        </div>
                    </CardContent>
                </Card>

                {/* Border Radius Showcase */}
                <Card>
                    <CardHeader>
                        <CardTitle className="flex items-center gap-2">
                            <Radius className="h-5 w-5" />
                            Border Radius
                        </CardTitle>
                        <CardDescription>
                            Current setting: {settings.borderRadius}
                        </CardDescription>
                    </CardHeader>
                    <CardContent>
                        <div className="grid grid-cols-2 md:grid-cols-3 gap-4">
                            <Card className="p-4">
                                <h4 className="font-medium mb-2">Card Element</h4>
                                <p className="text-sm text-muted-foreground">
                                    Cards inherit the global border radius setting
                                </p>
                            </Card>
                            <div className="space-y-2">
                                <Button className="w-full">Button</Button>
                                <Input placeholder="Input field" />
                            </div>
                            <div className="space-y-2">
                                <Badge>Badge</Badge>
                                <div className="h-12 bg-muted rounded-lg border flex items-center justify-center">
                                    <span className="text-sm">Custom Element</span>
                                </div>
                            </div>
                        </div>
                    </CardContent>
                </Card>

                {/* Interactive Elements */}
                <Card>
                    <CardHeader>
                        <CardTitle className="flex items-center gap-2">
                            <Settings className="h-5 w-5" />
                            Interactive Elements
                        </CardTitle>
                        <CardDescription>
                            Form controls and interactive components
                        </CardDescription>
                    </CardHeader>
                    <CardContent className="space-y-6">
                        <div className="grid md:grid-cols-2 gap-6">
                            <div className="space-y-4">
                                <div className="space-y-2">
                                    <Label htmlFor="email">Email</Label>
                                    <Input id="email" type="email" placeholder="Enter your email" />
                                </div>

                                <div className="flex items-center space-x-2">
                                    <Switch id="notifications" />
                                    <Label htmlFor="notifications">Enable notifications</Label>
                                </div>

                                <div className="space-y-2">
                                    <Label>Preferences</Label>
                                    <div className="flex gap-2">
                                        <Button variant="outline" size="sm">
                                            <Star className="h-4 w-4 mr-1" />
                                            Favorite
                                        </Button>
                                        <Button variant="outline" size="sm">
                                            <Heart className="h-4 w-4 mr-1" />
                                            Like
                                        </Button>
                                        <Button variant="outline" size="sm">
                                            <Share2 className="h-4 w-4 mr-1" />
                                            Share
                                        </Button>
                                    </div>
                                </div>
                            </div>

                            <Card className="p-4">
                                <div className="space-y-3">
                                    <div className="flex items-center gap-3">
                                        <div className="h-10 w-10 bg-primary rounded-full flex items-center justify-center">
                                            <MessageCircle className="h-5 w-5 text-primary-foreground" />
                                        </div>
                                        <div>
                                            <p className="font-medium">Sample Message</p>
                                            <p className="text-sm text-muted-foreground">2 minutes ago</p>
                                        </div>
                                    </div>
                                    <p className="text-sm">
                                        This is a sample message card that demonstrates how the theme
                                        affects nested components and content areas.
                                    </p>
                                </div>
                            </Card>
                        </div>
                    </CardContent>
                </Card>

                {/* Accessibility Info */}
                <Card>
                    <CardHeader>
                        <CardTitle className="flex items-center gap-2">
                            <Eye className="h-5 w-5" />
                            Accessibility Settings
                        </CardTitle>
                        <CardDescription>
                            Current accessibility configurations
                        </CardDescription>
                    </CardHeader>
                    <CardContent>
                        <div className="grid md:grid-cols-3 gap-4">
                            <div className="flex items-center justify-between p-3 border rounded-lg">
                                <span className="text-sm font-medium">Reduced Motion</span>
                                <Badge variant={settings.reducedMotion ? "default" : "secondary"}>
                                    {settings.reducedMotion ? "Enabled" : "Disabled"}
                                </Badge>
                            </div>
                            <div className="flex items-center justify-between p-3 border rounded-lg">
                                <span className="text-sm font-medium">High Contrast</span>
                                <Badge variant={settings.highContrast ? "default" : "secondary"}>
                                    {settings.highContrast ? "Enabled" : "Disabled"}
                                </Badge>
                            </div>
                            <div className="flex items-center justify-between p-3 border rounded-lg">
                                <span className="text-sm font-medium">Colorblind Friendly</span>
                                <Badge variant={settings.colorBlindFriendly ? "default" : "secondary"}>
                                    {settings.colorBlindFriendly ? "Enabled" : "Disabled"}
                                </Badge>
                            </div>
                        </div>
                    </CardContent>
                </Card>

                {/* Footer */}
                <div className="text-center py-8">
                    <p className="text-muted-foreground">
                        Use the theme customizer (⚙️ icon) in the navigation to modify these settings in real-time.
                    </p>
                </div>
            </div>
        </div>
    )
}
