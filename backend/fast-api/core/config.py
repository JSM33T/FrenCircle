"""
Configuration Management
=======================

This module handles application configuration using environment variables
and provides a centralized settings object.
"""

import os
from typing import List
from pydantic_settings import BaseSettings
from pydantic import Field


class Settings(BaseSettings):
    """
    Application settings loaded from environment variables.
    """
    
    # Application Settings
    PROJECT_NAME: str = Field(default="FrenCircle API")
    PROJECT_DESCRIPTION: str = Field(default="FrenCircle Backend API")
    VERSION: str = Field(default="1.0.0")
    ENVIRONMENT: str = Field(default="development")
    DEBUG: bool = Field(default=True)
    
    # Server Configuration
    HOST: str = Field(default="0.0.0.0")
    PORT: int = Field(default=8000)
    
    # CORS Settings
    ALLOWED_HOSTS: List[str] = Field(default=["*"])
    
    # Logging Configuration
    LOG_LEVEL: str = Field(default="INFO")
    LOG_FILE: str = Field(default="")
    VERBOSE_LOGGING: bool = Field(default=False)
    
    class Config:
        env_file = f".env.{os.getenv('ENVIRONMENT', 'development')}"
        env_file_encoding = 'utf-8'
        case_sensitive = True


# Create settings instance
settings = Settings()
