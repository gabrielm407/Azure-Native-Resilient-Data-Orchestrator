resource "azurerm_servicebus_namespace" "sb" {
  name                = "sb-databricks-demo-${random_string.suffix.result}"
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  sku                 = "Standard"
}

resource "azurerm_servicebus_queue" "sb_queue" {
  name         = "job-queue"
  namespace_id = azurerm_servicebus_namespace.sb.id
  partitioning_enabled = true # Optimizes for scale
}