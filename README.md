FynosAPI

A backend API for the Fynos e-commerce application built with ASP.NET Core, Entity Framework Core, and PostgreSQL. The API is containerized with Docker for easy setup and development.

Features:

CRUD operations for products
PostgreSQL database integration via EF Core
Dockerized for local development
Swagger documentation for testing API endpoints

Tech Stack:

Backend: ASP.NET Core 8
ORM: Entity Framework Core
Database: PostgreSQL 16
Containerization: Docker, Docker Compose
API Docs: Swagger

Getting Started:

Clone the repo:
git clone https://github.com/<your-username>/<repo-name>.git
cd backend

Copy the environment template:
cp .env.template .env   # Windows: copy .env.template .env

Edit .env if you want to change database credentials or ASP.NET environment:

POSTGRES_DB=fynosdb
POSTGRES_USER=postgres
POSTGRES_PASSWORD=yourpassword
ASPNETCORE_ENVIRONMENT=Development

Running with Docker

Build and start the containers:

docker compose up -d --build


Stop the containers:

docker compose down


The API will be accessible at: http://localhost:8080

Swagger UI available at: http://localhost:8080/swagger/index.html

Database

PostgreSQL container runs on port 5432

Default credentials are in .env

Database schema is managed via Entity Framework Core Migrations

API Endpoints
Method	Endpoint	Description
GET	/api/products	Get all products
GET	/api/products/{id}	Get product by ID
POST	/api/products	Create a new product
PUT	/api/products/{id}	Update an existing product
DELETE	/api/products/{id}	Delete a product

Swagger UI provides an interactive interface to test endpoints.

Contributing

Fork the repo

Create a branch (git checkout -b feature/my-feature)

Commit your changes (git commit -m 'Add new feature')

Push to the branch (git push origin feature/my-feature)

Open a Pull Request
