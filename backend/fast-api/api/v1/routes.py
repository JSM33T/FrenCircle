"""
API v1 Routes
============

Simple API routes for version 1.
"""

from fastapi import APIRouter
from core.config import settings


# Initialize router
router = APIRouter()


@router.get("/health")
async def health_check():
    """
    Health check endpoint to verify the API is running.
    """
    return {
        "status": "healthy",
        "message": "FrenCircle API is running smoothly",
        "environment": settings.ENVIRONMENT,
        "version": settings.VERSION,
        "debug": settings.DEBUG
    }
