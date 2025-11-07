.PHONY: help build up down restart logs clean test test-unit test-integration test-frontend test-e2e

help: ## عرض قائمة الأوامر المتاحة
	@echo "أوامر ShopFree Docker:"
	@grep -E '^[a-zA-Z_-]+:.*?## .*$$' $(MAKEFILE_LIST) | awk 'BEGIN {FS = ":.*?## "}; {printf "\033[36m%-20s\033[0m %s\n", $$1, $$2}'

build: ## بناء جميع الصور
	docker-compose build

up: ## تشغيل جميع الخدمات
	docker-compose up -d

down: ## إيقاف جميع الخدمات
	docker-compose down

restart: ## إعادة تشغيل جميع الخدمات
	docker-compose restart

logs: ## عرض سجلات جميع الخدمات
	docker-compose logs -f

logs-api: ## عرض سجلات Backend API
	docker-compose logs -f api

logs-frontend: ## عرض سجلات Frontend
	docker-compose logs -f frontend

logs-db: ## عرض سجلات قاعدة البيانات
	docker-compose logs -f sqlserver

dev: ## تشغيل في وضع التطوير
	docker-compose -f docker-compose.dev.yml up

dev-down: ## إيقاف وضع التطوير
	docker-compose -f docker-compose.dev.yml down

clean: ## حذف جميع الحاويات والصور والحجم
	docker-compose down -v --rmi all

rebuild: ## إعادة بناء وتشغيل
	docker-compose down
	docker-compose build --no-cache
	docker-compose up -d

shell-api: ## فتح shell في حاوية API
	docker-compose exec api bash

shell-frontend: ## فتح shell في حاوية Frontend
	docker-compose exec frontend sh

shell-db: ## فتح SQL Server shell
	docker-compose exec sqlserver /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P YourStrong@Passw0rd

status: ## عرض حالة جميع الخدمات
	docker-compose ps

test: ## تشغيل جميع الاختبارات (Backend + Frontend)
	@echo "Running Backend Tests..."
	dotnet test
	@echo "Running Frontend Tests..."
	cd shopfree-frontend && npm run test -- --run

test-unit: ## تشغيل Unit Tests للـ Backend
	dotnet test ShopFree.UnitTests/ShopFree.UnitTests.csproj

test-integration: ## تشغيل Integration Tests للـ Backend
	dotnet test ShopFree.IntegrationTests/ShopFree.IntegrationTests.csproj

test-frontend: ## تشغيل Unit Tests للـ Frontend
	cd shopfree-frontend && npm run test

test-e2e: ## تشغيل E2E Tests للـ Frontend
	cd shopfree-frontend && npm run test:e2e

test-coverage: ## تشغيل الاختبارات مع تغطية الكود
	dotnet test --collect:"XPlat Code Coverage"
	cd shopfree-frontend && npm run test:coverage
