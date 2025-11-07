# Docker Setup Guide

دليل إعداد وتشغيل منصة ShopFree باستخدام Docker.

## المتطلبات

- Docker Desktop (أو Docker Engine + Docker Compose)
- Docker Compose v3.8+

## التشغيل السريع

### 1. تشغيل جميع الخدمات (Production-like)

```bash
docker-compose up -d
```

سيتم تشغيل:
- SQL Server على المنفذ 1433
- Backend API على المنفذ 5000
- Frontend على المنفذ 3000

### 2. تشغيل في وضع التطوير (Development)

```bash
docker-compose -f docker-compose.dev.yml up
```

هذا الوضع يدعم:
- Hot reload للـ Backend
- Hot reload للـ Frontend
- Mounting للكود المصدري

## الأوامر المفيدة

### عرض السجلات (Logs)

```bash
# جميع الخدمات
docker-compose logs -f

# خدمة محددة
docker-compose logs -f api
docker-compose logs -f frontend
docker-compose logs -f sqlserver
```

### إيقاف الخدمات

```bash
docker-compose down
```

### إيقاف وحذف البيانات

```bash
docker-compose down -v
```

### إعادة بناء الصور

```bash
docker-compose build --no-cache
```

### تشغيل أوامر داخل الحاوية

```bash
# Backend
docker-compose exec api bash

# Frontend
docker-compose exec frontend sh

# Database
docker-compose exec sqlserver /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P YourStrong@Passw0rd
```

## إعداد قاعدة البيانات

عند أول تشغيل، سيتم إنشاء قاعدة البيانات تلقائياً. إذا كنت تريد إنشاء Migration يدوياً:

```bash
# داخل حاوية Backend
docker-compose exec api dotnet ef migrations add InitialCreate --project ShopFree.Infrastructure --startup-project ShopFree.API

# تطبيق Migrations
docker-compose exec api dotnet ef database update --project ShopFree.Infrastructure --startup-project ShopFree.API
```

## الوصول إلى الخدمات

- **Frontend**: http://localhost:3000
- **Backend API**: http://localhost:5001
- **Swagger UI**: http://localhost:5001/swagger
- **SQL Server**: localhost:1433
  - Username: `sa`
  - Password: `YourStrong@Passw0rd`

## استكشاف الأخطاء

### مشكلة الاتصال بقاعدة البيانات

تأكد من أن SQL Server جاهز:

```bash
docker-compose ps
```

انتظر حتى يكون status `healthy` لخدمة sqlserver.

### مشكلة في المنافذ

إذا كانت المنافذ مستخدمة، يمكنك تغييرها في `docker-compose.yml`:

```yaml
ports:
  - "5002:80"  # بدلاً من 5001
```

### إعادة بناء الصور

إذا قمت بتغيير الكود، قد تحتاج لإعادة بناء الصور:

```bash
docker-compose build --no-cache
docker-compose up -d
```

## متغيرات البيئة

يمكنك تعديل المتغيرات البيئية في `docker-compose.yml`:

- `SA_PASSWORD`: كلمة مرور SQL Server
- `Jwt__Key`: مفتاح JWT (يجب أن يكون 32 حرف على الأقل)
- `ConnectionStrings__DefaultConnection`: سلسلة الاتصال بقاعدة البيانات

## ملاحظات الأمان

⚠️ **تحذير**: الإعدادات الحالية مخصصة للتطوير المحلي فقط. لا تستخدمها في الإنتاج!

للإنتاج:
1. استخدم كلمات مرور قوية
2. استخدم متغيرات بيئة آمنة
3. قم بتفعيل HTTPS
4. استخدم قاعدة بيانات منفصلة وآمنة

