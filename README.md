# 💻 Tokenized Asset Tracker (Serverless)

This project simulates a highly scalable, event-driven system for tracking and processing tokenized asset transfers originating from a blockchain (or similar external event source). It uses a clean **Producer-Consumer architecture** implemented entirely on Azure Functions (Isolated Worker Model) for optimal performance and resilience.

## 🚀 Architecture Overview 🧱

The system is decoupled into two primary components to ensure fast ingestion and resilient processing. 

| Component | Role | Azure Function Type |
| :--- | :--- | :--- |
| **Producer (BlockchainEventIngestor)** | Receives blockchain event notifications via a public HTTP endpoint. It validates the input and immediately queues the event to guarantee fast acknowledgment and queue-based reliability. | HTTP Trigger |
| **Consumer (AssetDataProcessor)** | Asynchronously pulls events from the queue and performs complex, time-consuming business logic, including persisting the final transaction state to a Cosmos DB ledger via a dedicated repository service. | Queue Trigger |

## 🛠️ Tech Stack & Concepts Demonstrated

| Component | Technology | Concept Demonstrated |
| :--- | :--- | :--- |
| **Runtime** | C# / .NET 9.0 | Azure Functions (Isolated Worker Model) |
| **Ingestion** | HTTP Trigger | Public API endpoint for receiving POST data |
| **Messaging** | Azure Storage Queue SDK | Output Binding / Queue Client for decoupling and reliability |
| **Persistence** | Azure Cosmos DB | NoSQL Persistence Layer using the official `Microsoft.Azure.Cosmos` Client |
| **Configuration** | `IOptions<T>` Pattern | Strongly-typed, externalized configuration management (DB/Container names) |
| **Startup Logic** | `ICosmosDbProvisioner` | Automated infrastructure provisioning on host startup (`CreateIfNotExistsAsync`) |
| **Code Quality** | Dependency Injection (DI) | Clean separation of concerns (SRP) for services and repository layers |

## ⚙️ Getting Started

### Prerequisites

* Visual Studio or VS Code with C# and Azure Functions extensions
* .NET SDK (v9.0 or higher)
* Azure Functions Core Tools (`func`)
* **Azurite** (local storage emulator) running for Queue/Storage access.
* An Azure Cosmos DB account (used for persistence).
* Postman or similar tool for testing the API.

### Configuration

You must configure your local settings for Azure Storage and Cosmos DB.

1.  **Azure Storage**: The Azurite connection string must be configured for the function host.
2.  **Cosmos DB**: Update your `local.settings.json` with your real Cosmos DB endpoint and configuration structure:

```json
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet-isolated",
    "CosmosDbConnectionString": "AccountEndpoint=[https://your-cosmos-db-name.documents.azure.com:443/;AccountKey=YOUR_SECRET_KEY_HERE](https://your-cosmos-db-name.documents.azure.com:443/;AccountKey=YOUR_SECRET_KEY_HERE)" 
  },
  "CosmosDbSettings": { 
    "DatabaseName": "AssetTrackerDb",
    "ContainerName": "transactions"
  }
}
```

## 🏃 Running the Project

### Clone the repository
```
git clone [YOUR_REPO_URL]
cd TokenizedAssetTracker-Serverless
```

### Start the Azure Functions host

From the root directory, run:
`func start`

Note: On startup, the Azure Functions host automatically resolves and executes the provisioning service (`ICosmosDbProvisioner`) to create the required database and container (`AssetTrackerDb/transactions`) in your Cosmos DB account if they do not already exist.

---

## 🧪 Testing (End-to-End Pipeline)

Use Postman or a similar API testing tool to send an event to the ingestion endpoint.  
A successful request confirms the entire pipeline is working:

HTTP Ingestion → Queue Message → Cosmos DB Persistence

### API Request Details

| Detail | Value |
|---------|-------|
| Method | POST |
| URL | `http://localhost:7071/api/BlockchainEventIngestorFunction` |
| Header | `Content-Type: application/json` |

#### Request Body Structure
```json
{
"AssetId": "2c5e884e-5e9c-4a3f-8c7a-5d6e2b1f8c7d",
"TxHash": "0xABC123DEF4567890...",
"NewOwnerAddress": "0xOwnerAddressXYZ...",
"EventType": "TRANSFER_ASSET"
}
```

---

## ✅ Verification Steps

1. **Check Console Log:**  
   The log should display that `AssetDataProcessorFunction` successfully processed and persisted the data.  
   Example output:  
   `Asset ID: ... successfully processed and persisted.`

2. **Check Cosmos DB:**  
   Use either Azure Portal or Azure Storage Explorer to confirm a new JSON document in the `AssetTrackerDb/transactions` container, matching the `AssetId` and `TxHash` from your request.

---

## 📘 Documentation Links

- [Azure Functions Documentation](https://learn.microsoft.com/azure/azure-functions/)
- [Azure Cosmos DB Documentation](https://learn.microsoft.com/azure/cosmos-db/)
- [Azure Storage Explorer](https://azure.microsoft.com/features/storage-explorer/)

---

## 🧩 License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.



