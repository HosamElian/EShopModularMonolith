# 🧱 EShop Modular Monolith

A robust, scalable, and maintainable backend application built using modern .NET technologies and architectural patterns. This project implements a **Modular Monolith (Modulith)** with clear **Vertical Slice Architecture (VSA)**, powered by **Domain-Driven Design (DDD)**, **CQRS**, and **event-driven communication** using RabbitMQ. It provides an end-to-end showcase of building production-grade backend systems with secure, clean, and modular code.

---

## 🔍 Overview

This project demonstrates how to structure large applications using **feature-based modular architecture** with boundaries enforced by public APIs and asynchronous messaging. Each module is independently developed, tested, and maintained—allowing for eventual migration to microservices with minimal effort using patterns like **Strangler Fig**.

---

## 🚀 Tech Stack & Architecture

- **.NET 8** with **C# 12**
- **ASP.NET Core Minimal APIs**
- **MediatR** for implementing CQRS & request pipelines
- **Entity Framework Core** with **PostgreSQL**
- **Redis** for distributed caching
- **RabbitMQ + MassTransit** for asynchronous messaging
- **Keycloak** for identity and access management
- **Docker & Docker Compose** for service orchestration
- **Serilog** for structured logging

---

## 🏗️ Key Architectural Concepts

- **Modular Monolith (Modulith)**: Independent modules with clear boundaries.
- **Vertical Slice Architecture (VSA)**: Features organized per use case rather than by layer (controllers, services, etc.).
- **Domain-Driven Design (DDD)**: Modules modeled around business domains.
- **CQRS (Command Query Responsibility Segregation)**: Separation of read and write logic for scalability and maintainability.
- **Outbox Pattern**: Ensures reliable messaging for distributed workflows.
- **Event-driven Communication**: Internal module events published via RabbitMQ for decoupled interactions.
- **Secure APIs**: OAuth2 + OpenID Connect using Keycloak.

---

## 📦 Modules & Features

### 📁 Catalog Module
- Implements **Vertical Slice Architecture** using feature folders.
- Applies **DDD** and **CQRS** with MediatR best practices.
- Exposes endpoints using **Carter** for clean Minimal API design.
- Uses **EF Core Code-First** approach with PostgreSQL migrations.
- Handles **Domain Events** like `UpdatePriceChanged`.
- Implements cross-cutting concerns: logging, validation, pagination, exception handling.

### 🧺 Basket Module
- Mirrors the architecture of the Catalog Module.
- Integrates **Redis** for distributed caching.
- Applies **Cache-aside**, **Proxy**, and **Decorator** patterns.
- Implements **Outbox Pattern** to publish `BasketCheckoutEvent` to RabbitMQ.
- Consumes events in downstream modules for asynchronous workflows.

### 🛂 Identity Module
- Uses **Keycloak** for centralized authentication and authorization.
- Configured with **OpenID Connect** and **JwtBearer tokens**.
- Runs in a **Dockerized environment** as a backing service.
- Secures all APIs with Bearer tokens from Keycloak.

### 📦 Ordering Module
- Processes orders received via events from the Basket module.
- Implements DDD, CQRS, and Vertical Slice Architecture.
- Relies on the **Outbox Pattern** to ensure consistency and reliable messaging.

---

## 🔄 Module Communication

- **Synchronous (In-Process)**: Catalog ↔ Basket via Public APIs.
- **Asynchronous (Message-based)**: Catalog → Basket & Basket → Ordering via RabbitMQ + MassTransit.

---

## 🧩 Patterns & Practices

- **Outbox Pattern** for resilient messaging.
- **Cache-aside Strategy** to optimize read-heavy operations.
- **Proxy & Decorator Patterns** for enhancing behaviors (e.g., logging, caching).
- **Serilog** for structured and contextual logging.
- **MediatR Behaviors** for validation, logging, and exception handling pipelines.

---

## 🛠️ Development & Setup

1. Clone the repository  
   ```bash
   git clone https://github.com/your-username/eshop-modular-monolith.git
   cd eshop-modular-monolith
   ```

2. Build and run the solution using Docker Compose:  
   ```bash
   docker-compose up --build
   ```

3. Access Keycloak at `http://localhost:8080` to manage users and clients.

---

## 🌱 Migration to Microservices

This solution is designed to facilitate gradual migration to microservices using the **Strangler Fig Pattern**. Each module operates independently and can be extracted into a separate service with minimal refactoring.

---

## 📄 License

This project is licensed under the [MIT License](LICENSE).
