
'use client'

import { httpService } from '@/services/httpService';
import { useEffect, useState } from 'react';

export default function ChangelogClient() {
    const [loading, setLoading] = useState(true);
    const [data, setData] = useState<any[]>([]);

    useEffect(() => {
        (async () => {
            try {
                const res = await httpService.get('/changelog/grouped');
                setData(res.data);
            } catch {
                // toast is already handled by httpService
            } finally {
                setLoading(false);
            }
        })();
    }, []);

    return (
        <div className="container py-4">
            <h1 className="text-2xl nav-link font-bold mb-4"><pre>Changelog</pre></h1>

            {loading ? (
                <div>
                    {[...Array(3)].map((_, i) => (
                        <div key={i} className="mb-5">
                            <div className="placeholder-glow mb-2">
                                <span className="placeholder col-3"></span>
                            </div>
                            <div className="placeholder-glow mb-1">
                                <span className="placeholder col-6"></span>
                            </div>
                            <div className="placeholder-glow mb-1">
                                <span className="placeholder col-8"></span>
                            </div>
                        </div>
                    ))}
                </div>
            ) : (
                data.map((entry: any) => (
                    <div key={entry.version} className="mb-6">
                        <h2 className="text-xl fw-semibold mb-2">Version {entry.version}</h2>
                        {entry.changes.map((change: any) => (
                            <pre key={change.id} className="border-bottom pb-3 mb-3">
                                <h3 className="text-lg fw-medium">{change.title}</h3>
                                <p className="text-sm text-muted">{change.description}</p>
                                <p className="text-xs mt-1">
                                    <strong>Type:</strong> {change.changeType} |{' '}
                                    <strong>By:</strong> {change.contributors} |{' '}
                                    <strong>Date:</strong> {new Date(change.changedAt).toLocaleDateString()}
                                </p>
                            </pre>
                        ))}
                    </div>
                ))
            )}
        </div>
    );
}
