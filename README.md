# ShopFree Platform

منصة إلكترونية لإنشاء متاجر إلكترونية احترافية في دقائق دون الحاجة إلى بوابة دفع إلكترونية أو حساب بنكي.

## البنية المعمارية

المشروع مبني باستخدام **Clean Architecture** مع تطبيق مبادئ **DDD (Domain-Driven Design)** و **CQRS (Command Query Responsibility Segregation)** و **SOLID**.

### الطبقات (Layers)

1. **Domain Layer** (`ShopFree.Domain`)
   - Entities (User, Store, Product, Order, PaymentMethod)
   - Value Objects (CustomerInfo, Address)
   - Domain Interfaces (Repositories, Services)
   - Enums (OrderStatus, PaymentMethodType)
   - Domain Logic

2. **Application Layer** (`ShopFree.Application`)
   - Commands (Write operations)
   - Queries (Read operations)
   - Command/Query Handlers
   - DTOs
   - AutoMapper Profiles
   - FluentValidation Validators
   - MediatR for CQRS

3. **Infrastructure Layer** (`ShopFree.Infrastructure`)
   - Entity Framework Core DbContext
   - Repository Implementations
   - Unit of Work Pattern
   - External Services (JWT Service)
   - Database Configurations

4. **Presentation Layer** (`ShopFree.API`)
   - Controllers
   - API Endpoints
   - Middleware
   - Dependency Injection Configuration

## الميزات

- ✅ نظام المصادقة (JWT)
- ✅ إدارة المتاجر (Multi-tenant)
- ✅ إدارة المنتجات
- ✅ إدارة الطلبات
- ✅ طرق الدفع البديلة (التحويل البنكي، الدفع عند الاستلام، المحفظة الإلكترونية)
- ✅ واجهة المتجر العامة

## التقنيات المستخدمة

### Backend
- .NET 9.0
- ASP.NET Core Web API
- Entity Framework Core
- MediatR (CQRS)
- AutoMapper
- FluentValidation
- JWT Authentication
- BCrypt.Net

### Frontend (قيد التطوير)
- React
- React Router
- Axios

## إعداد المشروع

### الطريقة 1: استخدام Docker (موصى به) 🐳

#### المتطلبات
- Docker Desktop (أو Docker Engine + Docker Compose)

#### خطوات التشغيل

1. استنساخ المشروع:
```bash
git clone <repository-url>
cd shopfree
```

2. تشغيل جميع الخدمات:
```bash
# Production-like
docker-compose up -d

# أو Development mode (مع hot reload)
docker-compose -f docker-compose.dev.yml up
```

3. الوصول إلى الخدمات:
- **Frontend**: http://localhost:3000
- **Backend API**: http://localhost:5001
- **Swagger UI**: http://localhost:5001/swagger
- **SQL Server**: localhost:1433 (sa/YourStrong@Passw0rd)

للمزيد من التفاصيل، راجع [DOCKER.md](DOCKER.md)

### الطريقة 2: التشغيل المحلي

#### المتطلبات
- .NET 9.0 SDK
- Node.js 18+
- SQL Server (أو LocalDB)

#### خطوات الإعداد

1. استنساخ المشروع:
```bash
git clone <repository-url>
cd shopfree
```

2. إعداد Backend:
```bash
# استعادة الحزم
dotnet restore

# تحديث Connection String في appsettings.json
# تحديث JWT Key في appsettings.json

# تشغيل Backend
cd ShopFree.API
dotnet run
```

3. إعداد Frontend:
```bash
cd shopfree-frontend
npm install
npm run dev
```

4. تحديث Connection String في `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=ShopFreeDb;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

5. تحديث JWT Key في `appsettings.json`:
```json
{
  "Jwt": {
    "Key": "YourSuperSecretKeyThatIsAtLeast32CharactersLong!",
    "Issuer": "ShopFree",
    "Audience": "ShopFree"
  }
}
```

## API Endpoints

### Authentication
- `POST /api/auth/register` - تسجيل مستخدم جديد
- `POST /api/auth/login` - تسجيل الدخول

### Stores
- `POST /api/stores` - إنشاء متجر جديد (محمي)

## البنية المعمارية

```
ShopFree.API (Presentation)
    ↓
ShopFree.Application (Application - CQRS)
    ↓
ShopFree.Domain (Domain - Entities, Value Objects, Interfaces)
    ↑
ShopFree.Infrastructure (Infrastructure - Data Access, External Services)
```

## المبادئ المطبقة

- **Clean Architecture**: فصل الطبقات مع Dependency Inversion
- **DDD**: Domain Entities مع Value Objects و Domain Logic
- **CQRS**: فصل Commands (Write) عن Queries (Read)
- **SOLID**: Single Responsibility, Open/Closed, Liskov Substitution, Interface Segregation, Dependency Inversion
- **Repository Pattern**: فصل Data Access Logic
- **Unit of Work**: إدارة Transactions

## Docker Support 🐳

المشروع يدعم Docker بالكامل مع:
- ✅ Docker Compose لتشغيل جميع الخدمات
- ✅ Multi-stage builds للأداء الأمثل
- ✅ Development mode مع hot reload
- ✅ Production-ready configurations

راجع [DOCKER.md](DOCKER.md) للتفاصيل الكاملة.

## الاختبارات الآلية 🧪

المشروع يحتوي على اختبارات آلية شاملة:

### Backend Tests

- **Unit Tests** (`ShopFree.UnitTests`): اختبارات الوحدات للمكونات الفردية
  - Domain Entities
  - Command/Query Handlers
  - استخدام Moq و FluentAssertions

- **Integration Tests** (`ShopFree.IntegrationTests`): اختبارات التكامل للـ API
  - Controllers
  - End-to-End API Tests
  - استخدام In-Memory Database

### Frontend Tests

- **Unit Tests** (Vitest): اختبارات المكونات React
  - Components
  - Pages
  - Context

- **E2E Tests** (Playwright): اختبارات End-to-End
  - User Flows
  - Cross-browser Testing

### تشغيل الاختبارات

```bash
# جميع الاختبارات (Backend + Frontend)
make test

# Backend Unit Tests
make test-unit

# Backend Integration Tests
make test-integration

# Frontend Unit Tests
cd shopfree-frontend && npm run test

# Frontend E2E Tests
cd shopfree-frontend && npm run test:e2e

# مع تغطية الكود
make test-coverage
```

راجع [TESTING.md](TESTING.md) للدليل الكامل.

## CI/CD Pipeline 🚀

المشروع يحتوي على CI/CD Pipeline كامل على GitHub Actions:

### Workflows المتاحة

1. **CI - Full Pipeline** (`.github/workflows/ci.yml`)
   - Build Backend و Frontend
   - Run جميع الاختبارات
   - Upload artifacts و coverage reports
   - Status check شامل

2. **CI - Tests** (`.github/workflows/tests.yml`)
   - Backend Unit و Integration Tests
   - Frontend Unit و E2E Tests
   - Coverage reports

3. **CI - Build** (`.github/workflows/build.yml`)
   - Build Backend و Frontend
   - التحقق من أخطاء البناء

4. **CI - Lint** (`.github/workflows/lint.yml`)
   - Code formatting checks
   - Linting checks

5. **CI - Docker Build** (`.github/workflows/docker-build.yml`)
   - Build Docker images
   - Push to GitHub Container Registry

### متى يتم تشغيل CI؟

- ✅ عند Push إلى `main`, `develop`, أو `feature/**`
- ✅ عند فتح Pull Request إلى `main` أو `develop`
- ✅ عند إنشاء Tag جديد (لـ Docker build)

### Status Badges

يمكنك إضافة status badges إلى README:

```markdown
![CI](https://github.com/aliabuzahra/shopfree/workflows/CI%20-%20Full%20Pipeline/badge.svg)
![Tests](https://github.com/aliabuzahra/shopfree/workflows/CI%20-%20Tests/badge.svg)
![Build](https://github.com/aliabuzahra/shopfree/workflows/CI%20-%20Build/badge.svg)
```

### GitHub Actions Secrets

للـ Docker build، تأكد من:
- `GITHUB_TOKEN` متاح تلقائياً (لا حاجة لإعداده)

## التطوير المستقبلي

- [ ] Microservices Architecture
- [ ] Event Sourcing
- [ ] API Gateway
- [ ] Message Queue (RabbitMQ/Azure Service Bus)
- [ ] Caching (Redis)
- [ ] Logging (Serilog)
- [ ] Monitoring (Application Insights)

## الترخيص

MIT License

