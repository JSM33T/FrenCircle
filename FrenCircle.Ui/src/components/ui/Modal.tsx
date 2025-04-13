
'use client'
import {
    forwardRef,
    useImperativeHandle,
    useRef,
    useState,
    useEffect,
} from 'react'

export enum ModalType {
    Success = 'primary',
    Error = 'danger',
    Warning = 'warning',
}

export type ModalRef = {
    show: (type: ModalType, title: string, message: string) => void
    hide: () => void
}

const Modal = forwardRef<ModalRef>((_, ref) => {
    const modalElementRef = useRef<HTMLDivElement>(null)
    const instanceRef = useRef<any>(null)

    const [modalType, setModalType] = useState<ModalType>(ModalType.Success)
    const [title, setTitle] = useState('')
    const [message, setMessage] = useState('')

    useImperativeHandle(ref, () => ({
        show: (type, t, msg) => {
            setModalType(type)
            setTitle(t)
            setMessage(msg)
            instanceRef.current?.show()
        },
        hide: () => {
            instanceRef.current?.hide()
        },
    }))

    useEffect(() => {
        if (!modalElementRef.current) return

        import('bootstrap').then(({ Modal }) => {
            instanceRef.current = new Modal(modalElementRef.current!)
        })
    }, [])

    return (
        <div
            ref={modalElementRef}
            className="modal fade"
            tabIndex={-1}
            aria-hidden="true"
        >
            <div className="modal-dialog">
                <div className={`modal-content border-${modalType}`}>
                    <div className={`modal-header bg-${modalType} text-white`}>
                        <h5 className="modal-title">{title}</h5>
                        <button
                            type="button"
                            className="btn-close"
                            data-bs-dismiss="modal"
                        />
                    </div>
                    <div className="modal-body">
                        <p>{message}</p>
                    </div>
                </div>
            </div>
        </div>
    )
})

Modal.displayName = 'Modal'
export default Modal
