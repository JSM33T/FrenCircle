import { Metadata } from "next"
import { defaultMetadata } from "../lib/defaultMetadata"

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
        </div>
    )
}
