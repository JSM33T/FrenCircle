import { defaultMetadata } from "@/lib/defaultMetadata"
import { Metadata } from "next"
import Link from "next/link"

export const generateMetadata = async (): Promise<Metadata> => {
    return {
        ...defaultMetadata,
        title: 'About Us - FrenCircle',
        description: 'Learn more about MyApp and our mission.',
    }
}

export default function AboutPage() {

    return (
        <div className="container py-4">
            <h1>About Us</h1>
            <p>This is the about page of MyApp.</p>

            <Link className={`nav-link`} href="/about/changelog">Changelog</Link>
        </div>
    )
}
