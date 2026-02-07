# 🔧 Troubleshooting Guide

Common issues encountered during development and their solutions.

## 1. "Sync Trigger Failed / Malformed Content"
**Symptoms:** Deployment succeeds, but Function App shows no functions in Azure Portal.

**Cause:**
1. **Wrong Runtime:** Terraform configured the App Service for "In-Process" .NET, but code is ".NET Isolated".
2. **Zip Structure:** The deployment zip contained a nested folder (e.g., `zip/folder/host.json`) instead of `host.json` at the root.

**Solution:**
- Update Terraform to use `use_dotnet_isolated_runtime = true` (if using an isolated worker).
- Update `deploy.yml` to publish the `./published` folder directly and ensure zipping occurs at that folder root.

## 2. 401 Unauthorized on Curl
**Symptoms:** `curl` command returns `401 Unauthorized`.

**Cause:** The Function App is secure by default (`AuthorizationLevel.Function`).

**Solution:**
- **Option A:** Include the Function Key in the URL: `?code=<YOUR_KEY>`.
- **Option B:** Change the Function attribute to `AuthorizationLevel.Anonymous` for public access (only for demos).

## 3. Terraform Quota Error (401)
**Symptoms:** `CreateOrUpdate: unexpected status 401 ... Current Limit (Dynamic VMs): 0`.

**Cause:** Free/Student subscriptions often have zero quota for "Consumption" plans in certain regions.

**Solution:**
- Move region to `East US 2` or `North Central US` in Terraform variables.

## 4. "Directory does not contain a project or solution file"
**Symptoms:** GitHub Action fails at `dotnet build`.

**Cause:** The `dotnet` command was running in `./src` (root), but the `.csproj` file was in `./src/DataOrchestrator`.

**Solution:**
- Update `deploy.yml` environment variable `PROJECT_PATH` to `./src/DataOrchestrator` or set `working-directory` for `dotnet` steps.

---

If you hit an issue not listed here, run `az functionapp log tail --name <APP_NAME> --resource-group <RG>` to stream logs during invocation and paste the relevant error traces into an issue.