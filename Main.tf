terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "=3.0.0"
    }
  }
}

provider "azurerm" {
  features {}
}

resource "azurerm_resource_group" "dbs" {
  name     = "dbs-pp"
  location = "Norway East"
}

resource "azurerm_postgresql_server" "psql" {
  name                = "psqlserver-pp"
  location            = azurerm_resource_group.dbs.location
  resource_group_name = azurerm_resource_group.dbs.name

  administrator_login          = "psqladmin"
  administrator_login_password = ""

  sku_name   = "B_Gen5_1"
  version    = "11"
  storage_mb = 5120

  backup_retention_days        = 7
  geo_redundant_backup_enabled = false
  auto_grow_enabled            = false

  public_network_access_enabled    = true
  ssl_enforcement_enabled          = true
  ssl_minimal_tls_version_enforced = "TLS1_2"
}

resource "azurerm_redis_cache" "redis" {
  name                = "redis-pp"
  location            = azurerm_resource_group.dbs.location
  resource_group_name = azurerm_resource_group.dbs.name
  capacity            = 1
  family              = "C"
  sku_name            = "Basic"
  enable_non_ssl_port = true
  minimum_tls_version = "1.2"

  redis_configuration {
  }
}
