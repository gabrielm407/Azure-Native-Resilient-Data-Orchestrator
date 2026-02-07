# 🚀 Deployment & CI/CD Pipelines

This project uses **GitHub Actions** for all deployment tasks, separating Infrastructure (Terraform) from Application Code (.NET).

## 1. Terraform Pipeline (`terraform-ci.yml`)
Manages the Azure resources.
* **Trigger:** Push to `main` (Apply) or Pull Request (Plan).
* **State Management:** Remote state stored in **Terraform Cloud**.
* **Authentication:** Uses Azure Service Principal with `Contributor` role (or OIDC where possible).
* **Steps:**
    1. `terraform init`: Initialize providers.
    2. `terraform plan`: Preview changes.
    3. `terraform apply`: (Main branch only) Provision resources.

## 2. Application Pipeline (`deploy.yml`)
Builds and deploys the C# code.
* **Trigger:** Push to `main`.
* **Build Process:**
    1. `dotnet restore` & `dotnet test`: Runs unit tests.
    2. `dotnet publish`: Compiles code to `./published` folder.
* **Deployment:**
    * Uses `azure/login` to authenticate via RBAC.
    * Uses `Azure/functions-action` to zip and deploy the `./published` folder to the Function App created by Terraform.

## 🔧 Environment Secrets Required

| Secret Name | Description |
|---|---|
| `AZURE_CLIENT_ID` | Service Principal App ID |
| `AZURE_CLIENT_SECRET` | Service Principal Password |
| `AZURE_SUBSCRIPTION_ID` | Azure Subscription ID |
| `AZURE_TENANT_ID` | Azure Tenant (Directory) ID |
| `TF_API_TOKEN` | Terraform Cloud User Token |

## Notes & Best Practices
- Prefer OIDC-based workload identity (GitHub Actions `azure/login`) when possible to avoid long-lived SP secrets.
- Keep `PROJECT_PATH` or working-directory set to `./src/DataOrchestrator` for `dotnet` commands.
- Ensure the zip produced by the action contains `host.json` at the root (publish folder only).