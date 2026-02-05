# ============================================================================
# Terraform Providers and Backend Configuration
# ============================================================================

terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~> 4.0"
    }
    random = {
      source  = "hashicorp/random"
      version = "~> 3.0"
    }
  }

  backend "remote" {
    hostname     = "app.terraform.io"
    organization = "Azure-Native-Resilient-Data-Orchestrator"

    workspaces {
      name = "Azure-Native-Resilient-Data-Orchestrator"
    }
  }
}

provider "azurerm" {
  features {}
  use_cli = false
}