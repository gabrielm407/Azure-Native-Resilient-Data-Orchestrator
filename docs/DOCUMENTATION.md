# Documentation Index

This folder contains supporting documentation for the Azure-Native Resilient Data Orchestrator repository.

- **ARCHITECTURE.md** — Deep dive into system architecture, components, and data flow diagrams.
- **DEPLOYMENT.md** — CI/CD and Terraform pipeline details, required secrets, and best practices.
- **TROUBLESHOOTING.md** — Common issues, symptoms, root causes, and remediation steps.
- **QUICK_REFERENCE.md** — Cheatsheet: curl, Azure CLI, and Kusto queries for quick ops.

## Repo Structure

- `infrastructure/` — Terraform code that provisions Azure resources (Function App, Service Bus, Storage, App Insights).
- `src/` — C# Function App source code (`DataOrchestrator`).

## How to Contribute
1. Open a PR with a clear title and description.
2. Include link to architecture diagram or steps to reproduce any bug.
3. For infra changes, run `terraform plan` and attach the plan output to the PR.

---

If you want, I can add a `CHANGELOG.md`, `CONTRIBUTING.md`, or export the mermaid diagrams as images and store them in `docs/images/`.