# 🛒 Mini E-Commerce Microservices Architecture

![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?style=flat-square&logo=dotnet&logoColor=white)
![Kafka](https://img.shields.io/badge/Apache_Kafka-KRaft_Mode-231F20?style=flat-square&logo=apachekafka&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-Compose-2496ED?style=flat-square&logo=docker&logoColor=white)
![SQLite](https://img.shields.io/badge/SQLite-Database-003B57?style=flat-square&logo=sqlite&logoColor=white)

A highly scalable, decoupled **Event-Driven Microservices** application simulating a Mini E-Commerce platform. Built using **.NET 10 Minimal APIs**, **Entity Framework Core**, and **Apache Kafka**, fully containerized with **Docker Compose**.

---

## 🏗️ System Architecture

This project implements the **Choreography Saga Pattern** to ensure eventual consistency without a central orchestrator.

- **Clean Architecture** — Organized into `Brokers`, `Foundations (Services)`, and `Exposers (Controllers)`.
- **Asynchronous Messaging** — Services communicate via `Confluent.Kafka` using strongly-typed events.
- **Hybrid Communication** — Uses `IHttpClientFactory` for synchronous data fetching (Pricing) and Kafka for async processing (Stock/Balance deductions).
- **Decentralized Data Management** — Database-Per-Service pattern utilizing SQLite, with persistent Docker volumes.

---

## 📦 Services Breakdown

### 👥 User Service — Port `5001`
- Manages user profiles and wallet balances.
- **Kafka Consumer:** Listens to `order-events` to securely deduct the total price from the user's wallet.

### 📦 Product Service — Port `5002`
- Manages inventory and product pricing.
- **Kafka Consumer:** Listens to `order-events` to deduct purchased quantities from stock.

### 🧾 Order Service — Port `5003`
- The entry point for purchases.
- Synchronously queries the Product Service for the latest price.
- **Kafka Producer:** Publishes `OrderCreatedEvent` to trigger the Saga workflow.

### 🔗 Shared Library
- Centralized class library for Kafka configurations and shared Event Models (e.g., `OrderCreatedEvent`) to enforce contract consistency.

---

## 🚀 Getting Started

### Prerequisites

- [Docker Desktop](https://www.docker.com/products/docker-desktop) installed and running.
- .NET 10 SDK *(only required if running locally without Docker)*.

### Running the Cluster

Build and spin up the entire cluster — Kafka (KRaft, no Zookeeper) + 3 Microservices:

```bash
docker compose up --build -d
```

### Stopping the Cluster

To bring down all running containers:

```bash
docker compose down
```

---

## 🌐 API Documentation (Scalar UI)

This project uses **Scalar** instead of Swagger for a modern, interactive API documentation experience.

Once the containers are running, access the docs at:

| Service     | URL                                                      |
|-------------|----------------------------------------------------------|
| User API    | [http://localhost:5001/scalar/v1](http://localhost:5001/scalar/v1) |
| Product API | [http://localhost:5002/scalar/v1](http://localhost:5002/scalar/v1) |
| Order API   | [http://localhost:5003/scalar/v1](http://localhost:5003/scalar/v1) |

---

## 🎬 Testing the Saga Workflow (End-to-End)

**1. Seed Data**

- `POST /api/users` — Create a user with an initial wallet balance (e.g., `1000`). Save the `userId`.
- `POST /api/products` — Create a product with a price and stock quantity (e.g., `50` items). Save the `productId`.

**2. Place an Order**

- `POST /api/orders` — Submit an order using the `userId` and `productId` from above.

**3. Verify Eventual Consistency**

- **Product Service** — Stock should be automatically deducted.
- **User Service** — Total price (`Quantity × Price`) should be deducted from the wallet balance.

---

## 🛠️ Tech Stack Highlights

| Feature                   | Detail                                                    |
|---------------------------|-----------------------------------------------------------|
| Central Package Management | `Directory.Packages.props` (CPM)                         |
| GUID Strategy             | GUID v7 — time-ordered for better DB index performance    |
| Exception Handling        | Custom domain exceptions mapped to HTTP status codes      |
| Persistent Storage        | Data survives restarts via Docker volumes (`./DockerData`) |

---

> Built with ❤️ and Architectural Best Practices.