# Project Architecture Overview

This project is designed with a modern, scalable architecture leveraging multiple technologies for different components:

## Backend APIs

1. **.NET API**  
   - Serves as the main backend for core business logic and data management.
   - Handles authentication, authorization, and main application workflows.

2. **FastAPI (Python)**  
   - Dedicated to utility endpoints and AI/ML services.
   - Provides fast, asynchronous APIs for machine learning inference and data processing.

## Frontend

- **Angular**  
  - Single Page Application (SPA) for user interface.
  - Communicates with backend APIs via RESTful endpoints.

## Supporting Services

- **CDN Server (C#/.net)**  
  - Delivers static assets (images, scripts, styles) efficiently.
  - Built with .NET for performance and integration.

- **Redis Cache**  
  - Used for caching frequently accessed data.
  - Improves response times and reduces load on databases.

- **Qdrant (Vector Database)**  
  - Stores and searches vector embeddings for AI/ML features.
  - Enables semantic search and recommendation functionalities.

- **PostgreSQL**  
  - Main relational database for persistent storage.
  - Stores user data, application state, and transactional records.




## Deployment & Hosting

- **Docker**
  - Services and components will be containerized if they do not natively support deployment on Linux environments. This ensures compatibility and consistent deployment where needed.

- **Vercel**
  - The Angular frontend will be deployed and hosted on Vercel for fast, global delivery and easy CI/CD integration.

---
