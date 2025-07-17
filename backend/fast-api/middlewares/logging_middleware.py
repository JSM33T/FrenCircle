"""
Logging Middleware
=================

This middleware logs incoming requests and outgoing responses with configurable verbosity.
"""

import time
import logging
from fastapi import Request
from starlette.middleware.base import BaseHTTPMiddleware
from starlette.types import ASGIApp

from core.config import settings


class LoggingMiddleware(BaseHTTPMiddleware):
    """
    Middleware to log HTTP requests and responses with configurable logging levels.
    """
    
    def __init__(self, app: ASGIApp):
        super().__init__(app)
        
        # Configure logging based on settings
        log_level = getattr(logging, settings.LOG_LEVEL.upper(), logging.INFO)
        
        # Create formatter based on verbose setting
        if settings.VERBOSE_LOGGING:
            formatter = logging.Formatter(
                '%(asctime)s - %(name)s - %(levelname)s - %(message)s - [%(filename)s:%(lineno)d]'
            )
        else:
            formatter = logging.Formatter('%(asctime)s - %(levelname)s - %(message)s')
        
        # Setup logger
        self.logger = logging.getLogger(__name__)
        self.logger.setLevel(log_level)
        
        # Remove existing handlers to avoid duplicates
        if self.logger.handlers:
            self.logger.handlers.clear()
        
        # Console handler
        console_handler = logging.StreamHandler()
        console_handler.setFormatter(formatter)
        self.logger.addHandler(console_handler)
        
        # File handler if log file is specified
        if settings.LOG_FILE:
            try:
                file_handler = logging.FileHandler(settings.LOG_FILE)
                file_handler.setFormatter(formatter)
                self.logger.addHandler(file_handler)
            except Exception as e:
                self.logger.warning(f"Could not create log file {settings.LOG_FILE}: {e}")
    
    async def dispatch(self, request: Request, call_next):
        """
        Log request and response information.
        """
        # Start timer
        start_time = time.time()
        
        # Get client info
        client_ip = request.client.host if request.client else "unknown"
        user_agent = request.headers.get('user-agent', 'unknown')
        
        # Log request based on verbosity
        if settings.VERBOSE_LOGGING:
            self.logger.info(
                f"📥 REQUEST: {request.method} {request.url.path} "
                f"from {client_ip} - User-Agent: {user_agent} - "
                f"Query: {dict(request.query_params)} - "
                f"Headers: {dict(request.headers)}"
            )
        else:
            self.logger.info(f"📥 {request.method} {request.url.path} from {client_ip}")
        
        # Process request
        response = await call_next(request)
        
        # Calculate processing time
        process_time = time.time() - start_time
        
        # Log response based on verbosity
        if settings.VERBOSE_LOGGING:
            self.logger.info(
                f"📤 RESPONSE: {response.status_code} for {request.method} {request.url.path} "
                f"in {process_time:.4f}s - Headers: {dict(response.headers)}"
            )
        else:
            status_emoji = "✅" if response.status_code < 400 else "❌"
            self.logger.info(
                f"📤 {status_emoji} {response.status_code} - {request.method} {request.url.path} "
                f"({process_time:.3f}s)"
            )
        
        # Add processing time to response headers
        response.headers["X-Process-Time"] = str(process_time)
        
        return response
