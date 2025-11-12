# 💻 Tokenized Asset Tracker (Serverless)

This project simulates a highly scalable, event-driven system for tracking and processing tokenized asset transfers originating from a blockchain (or similar external event source). It uses a clean **Producer-Consumer** architecture implemented entirely on **Azure Functions**.

## 🚀 Architecture Overview

The system is decoupled into two primary components to ensure fast ingestion and resilient processing:

1.  **Producer (`BlockchainEventIngestor`):** Receives blockchain event notifications via a public HTTP endpoint. It validates the input and immediately queues the event.
2.  **Consumer (`AssetDataProcessor`):** Asynchronously pulls events from the queue and performs complex, time-consuming business logic (e.g., updating a ledger database, notifying users).



---

## 🛠️ Tech Stack & Concepts Demonstrated

| Component | Technology | Concept Demonstrated |
| :--- | :--- | :--- |
| **Runtime** | C# / .NET | Azure Functions (Isolated Worker Model) |
| **Ingestion** | HTTP Trigger | Public API endpoint for receiving POST data |
| **Messaging** | Azure Storage Queue SDK | Output Binding / Queue Client for decoupling |
| **Processing** | Queue Trigger | Asynchronous, auto-scaling event consumption |
| **Code Quality** | Dependency Injection (DI) | Clean separation of concerns (SRP) in the Service Layer |

---

## ⚙️ Getting Started

### Prerequisites

* **Visual Studio** or **VS Code** with C# and Azure Functions extensions
* **.NET SDK** (v6 or higher)
* **Azure Functions Core Tools** (`func`)
* **Azure Storage Account** connection string configured in `local.settings.json` (as `AzureWebJobsStorage`).
* **Postman** or similar tool for testing the API.

### Running the Project

1.  Clone the repository:
    ```bash
    git clone [YOUR_REPO_URL]
    cd Tokenized-Asset-Tracker-Serverless
    ```
2.  Start the Azure Functions host from the root directory:
    ```bash
    func start
    ```

---

## 🧪 Testing (Part 1: The Producer)

Use Postman to test the ingestion API. This validates the HTTP Trigger and the service's ability to publish the message to the queue.

| Detail | Value |
| :--- | :--- |
| **Method** | `POST` |
| **URL** | `http://localhost:7071/api/BlockchainEventIngestorFunction` |
| **Header** | `Content-Type: application/json` |

#### Request Body Structure:

```json
{
  "AssetId": "2c5e884e-5e9c-4a3f-8c7a-5d6e2b1f8c7d",
  "TxHash": "0xABC123DEF4567890...",
  "NewOwnerAddress": "0xOwnerAddressXYZ...",
  "EventType": "TRANSFER_ASSET"
}
