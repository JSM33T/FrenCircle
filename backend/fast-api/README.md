# FrenCircle FastAPI Backend

A simple, well-structured FastAPI backend application for the FrenCircle social networking platform.

## Project Structure

```
fast-api/
├── main.py                     # Application entry point
├── requirements.txt            # Python dependencies
├── .env.development           # Development environment variables
├── .env.production           # Production environment variables
├── core/
│   ├── __init__.py
│   └── config.py             # Configuration management
├── middlewares/
│   ├── __init__.py
│   └── logging_middleware.py # Configurable request/response logging
├── api/
│   ├── __init__.py
│   └── v1/                   # API version 1
│       ├── __init__.py
│       └── routes.py         # Simple API endpoints
└── logs/                    # Application logs (created at runtime)
```

## Features

- **Clean Architecture**: Simple, organized structure with minimal complexity
- **Environment Management**: Separate configuration files for development and production
- **Configurable Logging**: Middleware with verbose/simple logging options
- **API Versioning**: Organized API structure with version support
- **Health Check**: Simple health endpoint for monitoring

## Configuration Options

### Logging Configuration

The application supports configurable logging through environment variables:

- `LOG_LEVEL`: Set to DEBUG, INFO, WARNING, ERROR (default: INFO)
- `VERBOSE_LOGGING`: Set to True for detailed logs, False for simple logs (default: False)
- `LOG_FILE`: Optional file path for logging to file

**Verbose Logging (VERBOSE_LOGGING=True):**
- Includes detailed request/response information
- Shows headers, query parameters, and file/line numbers
- Detailed timestamps and emojis for better readability

**Simple Logging (VERBOSE_LOGGING=False):**
- Clean, minimal log output
- Shows only essential information (method, path, status, timing)
- Production-friendly format

## Quick Start

### 1. Install Dependencies

```bash
pip install -r requirements.txt
```

### 2. Environment Setup

Copy the appropriate environment file:

For development:
```bash
cp .env.development .env
```

For production:
```bash
cp .env.production .env
```

### 3. Run the Application

Development mode:
```bash
python main.py
```

Or using uvicorn directly:
```bash
uvicorn main:app --reload --host 0.0.0.0 --port 8000
```

### 4. Access the Application

- **API Documentation**: http://localhost:8000/docs
- **Health Check**: http://localhost:8000/api/v1/health

## API Endpoints

### Health Check
- `GET /api/v1/health` - Check if the API is running

Returns:
```json
{
  "status": "healthy",
  "message": "FrenCircle API is running smoothly",
  "environment": "development",
  "version": "1.0.0",
  "debug": true
}
```

## Configuration

The application supports different environments through environment files:

- `.env.development` - Development settings with debug mode and verbose logging
- `.env.production` - Production settings with optimized logging

Key configuration areas:
- **Application**: Basic app info and debug settings
- **Server**: Host and port configuration
- **CORS**: Cross-origin resource sharing settings
- **Logging**: Configurable logging levels and output options

## Development Guidelines

### Adding New API Endpoints

1. Add new routes to `api/v1/routes.py` or create new version folders (e.g., `api/v2/`)
2. Register new routers in `main.py`

### Customizing Logging

Modify the logging configuration in `middlewares/logging_middleware.py`:
- Adjust log formats
- Add custom log levels
- Modify what information is logged

## Environment Variables

### Development (.env.development)
```
PROJECT_NAME=FrenCircle API
ENVIRONMENT=development
DEBUG=True
HOST=0.0.0.0
PORT=8000
LOG_LEVEL=DEBUG
VERBOSE_LOGGING=True
LOG_FILE=logs/dev.log
```

### Production (.env.production)
```
PROJECT_NAME=FrenCircle API
ENVIRONMENT=production
DEBUG=False
HOST=0.0.0.0
PORT=8000
LOG_LEVEL=INFO
VERBOSE_LOGGING=False
LOG_FILE=logs/prod.log
```

## License

This project is licensed under the MIT License.
