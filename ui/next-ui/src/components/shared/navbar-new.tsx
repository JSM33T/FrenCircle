"use client"

import * as React from "react"
import Link from "next/link"
import { ChevronDown, Menu, User, Search, ShoppingCart } from "lucide-react"

import { Button } from "@/components/ui/button"
import {
	DropdownMenu,
	DropdownMenuContent,
	DropdownMenuItem,
	DropdownMenuLabel,
	DropdownMenuSeparator,
	DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu"
import {
	NavigationMenu,
	NavigationMenuContent,
	NavigationMenuItem,
	NavigationMenuLink,
	NavigationMenuList,
	NavigationMenuTrigger,
	navigationMenuTriggerStyle,
} from "@/components/ui/navigation-menu"
import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar"
import { ThemeToggle } from "@/components/theme-toggle"
import { cn } from "@/lib/utils"
import navigationData from "@/data/navigation.json"

interface NavItem {
	title: string
	href: string
	description?: string
	items?: NavItem[]
}

const ListItem = React.forwardRef<
	React.ElementRef<"a">,
	React.ComponentPropsWithoutRef<"a"> & { title: string; children: React.ReactNode }
>(({ className, title, children, ...props }, ref) => {
	return (
		<li>
			<NavigationMenuLink asChild>
				<a
					ref={ref}
					className={cn(
						"block select-none space-y-1 rounded-md p-3 leading-none no-underline outline-none transition-colors hover:bg-accent hover:text-accent-foreground focus:bg-accent focus:text-accent-foreground",
						className
					)}
					{...props}
				>
					<div className="text-sm font-medium leading-none">{title}</div>
					<p className="line-clamp-2 text-sm leading-snug text-muted-foreground">
						{children}
					</p>
				</a>
			</NavigationMenuLink>
		</li>
	)
})
ListItem.displayName = "ListItem"

export function Navbar() {
	const [isOpen, setIsOpen] = React.useState(false)

	return (
		<header className="sticky top-0 z-50 w-full border-b border-border/20 bg-background/80 backdrop-blur-xl backdrop-saturate-150 supports-[backdrop-filter]:bg-background/20 dark:border-border/20 dark:bg-background/40">
			<div className="container flex h-16 max-w-screen-2xl items-center">
				{/* Brand Logo */}
				<div className="mr-4 hidden md:flex">
					<Link href={navigationData.brand.href} className="mr-6 flex items-center space-x-2">
						<div className="h-8 w-8 rounded-lg bg-gradient-to-br from-blue-600 to-purple-600 flex items-center justify-center shadow-lg">
							<span className="text-sm font-bold text-white">FC</span>
						</div>
						<span className="hidden font-bold sm:inline-block">
							{navigationData.brand.name}
						</span>
					</Link>
				</div>

				{/* Mobile menu button */}
				<Button
					variant="ghost"
					className="mr-2 px-0 text-base hover:bg-transparent focus-visible:bg-transparent focus-visible:ring-0 focus-visible:ring-offset-0 md:hidden"
					onClick={() => setIsOpen(!isOpen)}
				>
					<Menu className="h-6 w-6" />
					<span className="sr-only">Toggle Menu</span>
				</Button>

				{/* Desktop Navigation */}
				<div className="hidden md:flex flex-1">
					<NavigationMenu>
						<NavigationMenuList>
							{navigationData.mainNav.map((item: NavItem) => (
								<NavigationMenuItem key={item.title}>
									{item.items ? (
										<>
											<NavigationMenuTrigger className="h-10">
												{item.title}
											</NavigationMenuTrigger>
											<NavigationMenuContent>
												<ul className="grid w-[400px] gap-3 p-4 md:w-[500px] md:grid-cols-2 lg:w-[600px]">
													{item.items.map((subItem) => (
														<ListItem
															key={subItem.title}
															title={subItem.title}
															href={subItem.href}
														>
															{subItem.description}
														</ListItem>
													))}
												</ul>
											</NavigationMenuContent>
										</>
									) : (
										<Link href={item.href} passHref>
											<NavigationMenuLink className={navigationMenuTriggerStyle()}>
												{item.title}
											</NavigationMenuLink>
										</Link>
									)}
								</NavigationMenuItem>
							))}
						</NavigationMenuList>
					</NavigationMenu>
				</div>

				{/* Right side actions */}
				<div className="flex flex-1 items-center justify-between space-x-2 md:justify-end">
					<div className="w-full flex-1 md:w-auto md:flex-none">
						{/* Search can be added here */}
					</div>
					<nav className="flex items-center space-x-2">
						{/* Search Button */}
						<Button variant="ghost" size="icon" className="h-9 w-9">
							<Search className="h-4 w-4" />
							<span className="sr-only">Search</span>
						</Button>

						{/* Shopping Cart */}
						<Button variant="ghost" size="icon" className="h-9 w-9">
							<ShoppingCart className="h-4 w-4" />
							<span className="sr-only">Shopping Cart</span>
						</Button>

						{/* Theme Toggle */}
						<ThemeToggle />

						{/* User Menu */}
						<DropdownMenu>
							<DropdownMenuTrigger asChild>
								<Button variant="ghost" className="relative h-9 w-9 rounded-full">
									<Avatar className="h-8 w-8">
										<AvatarImage src="/placeholder-avatar.jpg" alt="@user" />
										<AvatarFallback>FC</AvatarFallback>
									</Avatar>
								</Button>
							</DropdownMenuTrigger>
							<DropdownMenuContent className="w-56" align="end" forceMount>
								<DropdownMenuLabel className="font-normal">
									<div className="flex flex-col space-y-1">
										<p className="text-sm font-medium leading-none">John Doe</p>
										<p className="text-xs leading-none text-muted-foreground">
											john.doe@example.com
										</p>
									</div>
								</DropdownMenuLabel>
								<DropdownMenuSeparator />
								{navigationData.userMenu.map((item) => (
									<DropdownMenuItem key={item.title} asChild>
										<Link href={item.href}>{item.title}</Link>
									</DropdownMenuItem>
								))}
							</DropdownMenuContent>
						</DropdownMenu>
					</nav>
				</div>

				{/* Mobile Navigation */}
				{isOpen && (
					<div className="absolute top-16 left-0 right-0 z-50 border-b border-border/20 bg-background/90 backdrop-blur-xl p-4 shadow-lg md:hidden">
						<nav className="flex flex-col space-y-3">
							{navigationData.mainNav.map((item: NavItem) => (
								<div key={item.title}>
									{item.items ? (
										<DropdownMenu>
											<DropdownMenuTrigger asChild>
												<Button
													variant="ghost"
													className="w-full justify-between px-0 py-2 h-auto font-medium"
												>
													{item.title}
													<ChevronDown className="h-4 w-4" />
												</Button>
											</DropdownMenuTrigger>
											<DropdownMenuContent className="w-56">
												{item.items.map((subItem) => (
													<DropdownMenuItem key={subItem.title} asChild>
														<Link href={subItem.href} className="w-full">
															<div>
																<div className="font-medium">{subItem.title}</div>
																{subItem.description && (
																	<div className="text-sm text-muted-foreground">
																		{subItem.description}
																	</div>
																)}
															</div>
														</Link>
													</DropdownMenuItem>
												))}
											</DropdownMenuContent>
										</DropdownMenu>
									) : (
										<Link
											href={item.href}
											className="block px-0 py-2 text-sm font-medium transition-colors hover:text-foreground/80"
											onClick={() => setIsOpen(false)}
										>
											{item.title}
										</Link>
									)}
								</div>
							))}
							{/* Mobile Theme Toggle */}
							<div className="flex items-center justify-between pt-2 border-t border-border/20">
								<span className="text-sm font-medium">Theme</span>
								<ThemeToggle />
							</div>
						</nav>
					</div>
				)}
			</div>
		</header>
	)
}
