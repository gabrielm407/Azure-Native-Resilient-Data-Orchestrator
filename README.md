# ☁️ Azure-Native Resilient Data Orchestrator

![Build Status](https://img.shields.io/github/actions/workflow/status/YOUR_GITHUB_USERNAME/YOUR_REPO_NAME/deploy.yml?style=for-the-badge)
![Azure](https://img.shields.io/badge/azure-%230072C6.svg?style=for-the-badge&logo=microsoftazure&logoColor=white)
![.NET](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white)
![Terraform](https://img.shields.io/badge/terraform-%235835CC.svg?style=for-the-badge&logo=terraform&logoColor=white)

## 🎯 Overview
**"Distributed systems are hard. This project makes them observable."**

This repository demonstrates a **Serverless Event-Driven Architecture** built on Azure. It is designed to simulate a high-scale data ingestion pipeline similar to those used in **Azure Databricks** environments.

It focuses on three core engineering pillars:
1.  **Resilience** (Self-healing via retries & Dead Letter Queues)
2.  **Observability** (Deep telemetry & distributed tracing)
3.  **Automation** (Infrastructure as Code & CI/CD)

---

## 🏗️ Architecture
The system uses the **Competing Consumers** pattern to decouple ingestion from processing.

`[Client]` 📡 _HTTP POST_
   ⬇️
**[Ingest Function]** (Azure Function)
   ⬇️ _Async Handoff_
**[Job Queue]** (Azure Service Bus)
   ⬇️ _Trigger_
**[Worker Function]** (Azure Function) 🔄 _(Retries on Failure)_
   ⬇️
**[Telemetry Store]** (Application Insights)

---

## 🚀 Key Features

### 1. 🛡️ Chaos Engineering & Resilience
To prove the system works in the real world, the Worker intentionally simulates "transient failures" (random crashes).
* **Mechanism:** ~20% of jobs randomly throw an exception.
* **Safety Net:** Azure Service Bus automatically retries these messages using an **Exponential Backoff** policy.
* **Zero Data Loss:** If a message fails repeatedly, it is moved to a **Dead Letter Queue (DLQ)** for manual inspection.

### 2. 📊 Deep Telemetry
The system emits custom metrics to **Azure Application Insights** to track performance.
* `JobProcessingLatencyMs`: Measures exact time spent in the worker.
* `JobIngestionStart`: Tracks throughput of incoming requests.

*(Insert screenshot of your Azure App Insights Application Map here)*

### 3. 🤖 Automated DevOps
* **Infrastructure as Code:** All Azure resources (Service Bus, Functions, App Insights) are provisioned via **Terraform**.
* **CI/CD:** GitHub Actions pipeline automatically tests, builds, and deploys the code on every push to `main`.

---

## 🛠️ Tech Stack

| Component | Technology | Why? |
| :--- | :--- | :--- |
| **Compute** | Azure Functions (Isolated Worker) | Serverless scaling based on queue depth. |
| **Language** | C# / .NET 8 | Performance and strong typing for backend logic. |
| **Messaging** | Azure Service Bus | Enterprise-grade message broker for decoupling. |
| **IaC** | Terraform | Repeatable, state-managed infrastructure. |
| **Observability** | Application Insights | Distributed tracing and performance monitoring. |

---

## 🏃 Getting Started

### Prerequisites
* [Azure CLI](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli)
* [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
* [Terraform](https://www.terraform.io/downloads.html)

### 1. 🌩️ Provision Infrastructure
```bash
cd infrastructure
terraform init
terraform apply