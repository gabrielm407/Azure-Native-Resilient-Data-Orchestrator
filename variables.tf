variable "ARM_CLIENT_SECRET" {
  description = "The client secret for the Azure service principal"
  type        = string
  sensitive   = true
}

variable "ARM_SUBSCRIPTION_ID" {
  description = "The Azure Subscription ID"
  type        = string
}

variable "ARM_TENANT_ID" {
  description = "The tentant ID for the Azure service principal"
  type        = string
}

variable "ARM_CLIENT_ID" {
  description = "The client ID for the Azure service principal"
  type        = string
}