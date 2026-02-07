# ⚡ Quick Reference

## Trigger Ingest Job (curl)

```bash
curl -X POST <YOUR_URL>/api/IngestJob \
     -H "Content-Type: application/json" \
     -d '{"JobId": "test-01", "OperationType": "ETL", "DataSizeKB": 100}'
```

## Get Function Key (Azure CLI)

```bash
az functionapp function keys list --function-name IngestJob --resource-group rg-databricks-demo-dev --name <APP_NAME>
```

## Kusto Queries (KQL)
Run these in Application Insights Logs to see activity.

### See Recent Logs

```kql
union traces, exceptions, requests
| where timestamp > ago(1h)
| order by timestamp desc
```

### Count Failures vs Success

```kql
requests
| where success == false
| summarize count() by name
```

## Useful Links
- Azure Portal: https://portal.azure.com
- Terraform Cloud: https://app.terraform.io
- GitHub Actions Runs: https://github.com/<OWNER>/<REPO>/actions

---

If you need additional commands (Service Bus inspection, Storage Explorer quick links), tell me which resource and I can add sample commands.