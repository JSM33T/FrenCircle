"use client";
import React, { useState } from "react";
import { Card } from "@/components/ui/card";

const exampleCards = [
	{ title: "Card 1", description: "This is the first card." },
	{ title: "Card 2", description: "This is the second card." },
	{ title: "Card 3", description: "This is the third card." },
];

export default function OffcanvasSidebar() {
	const [open, setOpen] = useState(false);

	return (
		<>
			{/* Floating Button */}
			<button
				className="fixed right-6 bottom-6 z-50 rounded-full shadow-lg p-4 transition
				bg-primary text-primary-foreground hover:bg-primary/80 focus:outline-none focus:ring-2 focus:ring-primary"
				onClick={() => setOpen(true)}
				aria-label="Open sidebar"
			>
				&#9776;
			</button>

			{/* Offcanvas Sidebar */}
			<div
				className={`fixed top-0 right-0 h-full w-80 bg-background shadow-xl z-[100] transition-transform duration-300 ${open ? "translate-x-0" : "translate-x-full"}`}
				role="dialog"
				aria-modal="true"
			>
				<div className="flex justify-between items-center p-4 border-b">
					<h2 className="text-lg font-semibold">Sidebar</h2>
					<button
						className="text-xl font-bold hover:text-primary"
						onClick={() => setOpen(false)}
						aria-label="Close sidebar"
					>
						&times;
					</button>
				</div>
				<div className="p-4 space-y-4 overflow-y-auto h-[calc(100%-56px)]">
					{exampleCards.map((card, idx) => (
						<Card key={idx} className="p-4">
							<h3 className="font-bold text-base mb-2">{card.title}</h3>
							<p className="text-sm text-muted-foreground">{card.description}</p>
						</Card>
					))}
				</div>
			</div>

			{/* Overlay */}
			{open && (
				<div
					className="fixed inset-0 bg-black/30 z-[99]"
					onClick={() => setOpen(false)}
					aria-label="Close sidebar overlay"
				/>
			)}
		</>
	);
}
