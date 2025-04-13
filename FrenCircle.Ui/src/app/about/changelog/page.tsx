/* eslint-disable @typescript-eslint/no-explicit-any */
import { Metadata } from "next";
import { defaultMetadata } from "../../lib/defaultMetadata";
import { httpService } from "@/app/services/httpService";

export const generateMetadata = async (): Promise<Metadata> => {
    return {
        ...defaultMetadata,
        title: "Changelog - FrenCircle",
        description: "Learn more about MyApp and our mission.",
    };
};

export default async function ChangelogPage() {
    const res = await httpService.get("/changelog/grouped");

    return (
        <div className="container py-4">
            <h1 className="text-2xl font-bold mb-4">Changelog</h1>
            {res.data.map((entry: any) => (
                <div key={entry.version} className="mb-6">
                    <h2 className="text-xl font-semibold mb-2">Version {entry.version}</h2>
                    {entry.changes.map((change: any) => (
                        <div key={change.id} className="border-b pb-3 mb-3">
                            <h3 className="text-lg font-medium">{change.title}</h3>
                            <p className="text-sm text-gray-600">{change.description}</p>
                            <p className="text-xs mt-1">
                                <strong>Type:</strong> {change.changeType} |{" "}
                                <strong>By:</strong> {change.contributors} |{" "}
                                <strong>Date:</strong> {new Date(change.changedAt).toLocaleDateString()}
                            </p>
                        </div>
                    ))}
                </div>
            ))}
        </div>
    );
}
