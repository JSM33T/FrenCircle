'use client'

import Modal, { ModalRef, ModalType } from "@/components/ui/Modal"
import { useRef } from "react"


export default function HomeClient() {
    const modalRef = useRef<ModalRef>(null)

    const showModal = () => {
        modalRef.current?.show(ModalType.Success, 'Heads up!', 'Something needs your attention.')
    }

    return (
        <div className="container py-4">
            <h1>Home</h1>
            <p>Home page for FrenCircle.</p>
            <button className="btn btn-lg btn-primary" onClick={showModal}>Sample Modal</button>
            <Modal ref={modalRef} />
        </div>
    )
}
