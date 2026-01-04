# TradesCompany

## Overview

**TradesCompany** is a comprehensive web application designed to bridge the gap between customers and service professionals. It facilitates the discovery, booking, and management of various trade services. The platform is built using **ASP.NET Core MVC** and follows the **Clean Architecture** principles to ensure a scalable, maintainable, and testable codebase.

> **Note:** This is a **learning practice project** created to explore and demonstrate advanced software development concepts, architectural patterns, and full-stack .NET development.

## Features

The application includes a wide range of features modeled after real-world service marketplaces:

*   **User Management**: Distinct roles for **Customers** and **Service Men**, managed via a secure authentication system.
*   **Service & Scheduling**:
    *   **Service Listings**: Categorized services offering detailed descriptions.
    *   **Bookings**: Users can book services based on availability.
    *   **Scheduling**: Service men can manage their schedules and availability.
*   **Commerce & Transactions**:
    *   **Quotations**: Service men can send quotations for specific jobs, including `QuotationForms`.
    *   **Billing**: Generation of bills for completed services.
*   **Communication**:
    *   **Messaging System**: Integrated chat functionality enabling communication between users and service providers (Channels, Messages).
    *   **Notifications**: Real-time updates for booking statuses and messages.
*   **Feedback**:
    *   **Ratings**: Users can rate services and providers.

## Architecture

This project is structured using **Clean Architecture**, separating concerns into distinct layers:

1.  **TradesCompany.Domain**: Contains the core business logic, entities (e.g., `Booking`, `ServiceMan`, `Bill`), and enums. It has no dependencies on other layers.
2.  **TradesCompany.Application**: Defines interfaces and business rules/use cases.
3.  **TradesCompany.Infrastructure**: Implements interfaces defined in the Application layer (e.g., data access, external services).
4.  **TradesCompany.Web**: The presentation layer, built with ASP.NET Core MVC (Controllers, Views, ViewModels).
5.  **TradesCompany.Shared**: Shared resources and DTOs.

## Technology Stack

*   **Framework**: .NET / ASP.NET Core
*   **Pattern**: MVC (Model-View-Controller)
*   **Language**: C#
*   **Frontend**: Razor Views (HTML/CSS/JS)
*   **Database ORM**: Entity Framework Core (implied)

## Getting Started

To run this project locally:

1.  **Prerequisites**: Ensure you have the [.NET SDK](https://dotnet.microsoft.com/download) installed.
2.  **Clone**: Clone the repository to your local machine.
3.  **Restore**: Run `dotnet restore` to install dependencies.
4.  **Build**: Run `dotnet build` to compile the solution.
5.  **Run**: Navigate to the `TradesCompany.Web` directory and run `dotnet run`.
    ```bash
    cd TradesCompany.Web
    dotnet run
    ```
6.  **Access**: Open your browser and navigate to the local URL (usually `https://localhost:5001` or `http://localhost:5000`).

---
*This project is for educational purposes.*
