'use client'
import Link from 'next/link'
import { usePathname } from 'next/navigation'

export default function Navbar() {
    const pathname = usePathname()

    const isActive = (href: string) => pathname === href ? 'active' : ''

    return (
        <nav className="navbar navbar-expand-lg navbar-dark bg-dark px-3 sticky-top">
            <Link className="navbar-brand" href="/">FrenCircle</Link>
            <button
                className="navbar-toggler"
                type="button"
                data-bs-toggle="collapse"
                data-bs-target="#navbarContent"
                aria-controls="navbarContent"
                aria-expanded="false"
                aria-label="Toggle navigation"
            >
                <span className="navbar-toggler-icon" />
            </button>

            <div className="collapse navbar-collapse mx-2" id="navbarContent">
                <ul className="navbar-nav ms-auto">
                    <li className="nav-item">
                        <Link className={`nav-link ${isActive('/')}`} href="/">Home</Link>
                    </li>
                    <li className="nav-item">
                        <Link className={`nav-link ${isActive('/about')}`} href="/about">About</Link>
                    </li>

                    {/* <li className="nav-item dropdown">
                        <a
                            className="nav-link dropdown-toggle"
                            href="#"
                            role="button"
                            data-bs-toggle="dropdown"
                            aria-expanded="false"
                        >
                            Services
                        </a>
                        <ul className="dropdown-menu dropdown-menu-dark">
                            <li>
                                <Link className="dropdown-item disabled" href="/services/design">Design</Link>
                            </li>
                        </ul>
                    </li> */}
                </ul>
            </div>
        </nav>
    )
}
