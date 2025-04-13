'use client'

import { createContext, useContext, useState } from 'react';

type ToastType = 'success' | 'error' | 'info';

interface Toast {
    title: string;
    message: string;
    type: ToastType;
}

const ToastContext = createContext<(toast: Toast) => void>(() => { });

export const useToast = () => useContext(ToastContext);

export function ToastProvider({ children }: { children: React.ReactNode }) {
    const [toast, setToast] = useState<Toast | null>(null);

    return (
        <ToastContext.Provider value={setToast}>
            {children}
            {toast && (
                <div className="toast-container position-fixed top-0 end-0 p-3" style={{ zIndex: 1050 }}>
                    <div className={`toast show text-bg-${toast.type}`}>
                        <div className="toast-header">
                            <strong className="me-auto">{toast.title}</strong>
                            <button
                                type="button"
                                className="btn-close"
                                onClick={() => setToast(null)}
                            ></button>
                        </div>
                        <div className="toast-body">{toast.message}</div>
                    </div>
                </div>
            )}
        </ToastContext.Provider>
    );
}
