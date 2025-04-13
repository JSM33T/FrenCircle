import { defaultMetadata } from "@/lib/defaultMetadata"
import HomeClient from "./home/HomeClient"

import { Metadata } from "next"


export const generateMetadata = async (): Promise<Metadata> => {
    return {
        ...defaultMetadata,
        title: "Home - FrenCircle",
        description: "Learn more about MyApp and our mission.",
    }
}

export default function HomePage() {
    return <HomeClient />
}
