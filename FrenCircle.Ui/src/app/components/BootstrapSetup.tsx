'use client'
import { useEffect } from 'react'
import { useMounted } from '../helpers/useMounted'

export default function BootstrapSetup() {
    const mounted = useMounted()

    useEffect(() => {

    }, [])

    if (!mounted) return null
}