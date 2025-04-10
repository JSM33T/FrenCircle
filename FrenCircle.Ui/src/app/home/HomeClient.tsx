'use client'

import { useRef } from "react"
import Modal, { ModalRef, ModalType } from "../components/ui/Modal"

export default function HomeClient() {
    const modalRef = useRef<ModalRef>(null)

    const showModal = () => {
        modalRef.current?.show(ModalType.Success, 'Heads up!', 'Something needs your attention.')
    }

    return (
        <div className="container py-4">
            <h1>Home</h1>
            <p>Home page for FrenCircle.</p>
            <button className="btn btn-sm btn-primary" onClick={showModal}>Sample Modal</button>
            <Modal ref={modalRef} />
        </div>
    )
}
