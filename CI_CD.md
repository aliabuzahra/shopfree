# CI/CD Pipeline Documentation

دليل شامل لـ CI/CD Pipeline في منصة ShopFree.

## نظرة عامة

المشروع يستخدم GitHub Actions لـ CI/CD Pipeline الكامل الذي يشمل:
- ✅ Build Backend و Frontend
- ✅ Run جميع الاختبارات
- ✅ Code quality checks
- ✅ Docker image builds
- ✅ Artifact uploads
- ✅ Coverage reports

## Workflows

### 1. CI - Full Pipeline (`ci.yml`)

Pipeline شامل يجمع جميع الخطوات:

**Triggers:**
- Push إلى `main`, `develop`, أو `feature/**`
- Pull Request إلى `main` أو `develop`

**Jobs:**
- `build-backend`: Build Backend API
- `build-frontend`: Build Frontend
- `test-backend`: Run Backend tests
- `test-frontend`: Run Frontend tests
- `ci-status`: Combined status check

**Artifacts:**
- Backend build artifacts
- Frontend build artifacts
- Test results
- Coverage reports

### 2. CI - Tests (`tests.yml`)

اختبارات منفصلة للـ Backend و Frontend:

**Triggers:**
- Push إلى `main`, `develop`, أو `feature/**`
- Pull Request إلى `main` أو `develop`

**Jobs:**
- `backend-tests`: Backend Unit و Integration Tests
- `frontend-tests`: Frontend Unit و E2E Tests

**Services:**
- SQL Server container للـ Integration Tests

**Outputs:**
- Test results (TRX format)
- Coverage reports (Codecov)

### 3. CI - Build (`build.yml`)

Build checks فقط:

**Triggers:**
- Push إلى `main`, `develop`, أو `feature/**`
- Pull Request إلى `main` أو `develop`

**Jobs:**
- `backend-build`: Build Backend
- `frontend-build`: Build Frontend

### 4. CI - Lint (`lint.yml`)

Code quality checks:

**Triggers:**
- Push إلى `main`, `develop`, أو `feature/**`
- Pull Request إلى `main` أو `develop`

**Jobs:**
- `backend-lint`: .NET format check
- `frontend-lint`: ESLint check (if configured)

### 5. CI - Docker Build (`docker-build.yml`)

Build و push Docker images:

**Triggers:**
- Push إلى `main` أو `develop`
- Tags مثل `v1.0.0`
- Pull Request إلى `main` أو `develop` (build only, no push)

**Jobs:**
- `build-and-push`: Build و push API و Frontend images

**Registry:**
- GitHub Container Registry (`ghcr.io`)

**Tags:**
- Branch name
- PR number
- Semantic version (from tags)
- SHA commit

## كيفية الاستخدام

### تشغيل CI محلياً

```bash
# استخدام act (GitHub Actions locally)
act -j build-backend
act -j test-backend
```

### التحقق من CI Status

1. اذهب إلى GitHub repository
2. انقر على "Actions" tab
3. اختر workflow من القائمة
4. شاهد logs و results

### Debugging Failed CI

1. **Check logs**: اقرأ logs في GitHub Actions
2. **Run locally**: شغل نفس الأوامر محلياً
3. **Check dependencies**: تأكد من أن جميع الحزم محدثة
4. **Check secrets**: تأكد من أن Secrets صحيحة (للـ Docker build)

## Secrets و Environment Variables

### GitHub Secrets

لا حاجة لـ secrets إضافية - `GITHUB_TOKEN` متاح تلقائياً.

### Environment Variables في Workflows

```yaml
env:
  DOTNET_VERSION: '9.0.x'
  NODE_VERSION: '18'
  REGISTRY: ghcr.io
  IMAGE_NAME: ${{ github.repository }}
```

## Artifacts

### Backend Artifacts

- `backend-build`: DLL files من Release build
- `backend-test-results`: Test results (TRX format)
- `backend-coverage`: Coverage reports

### Frontend Artifacts

- `frontend-build`: Built files من `dist/`
- `frontend-test-results`: Test results (JSON format)

### Download Artifacts

1. اذهب إلى workflow run
2. انقر على "Artifacts" في sidebar
3. Download الملفات المطلوبة

## Coverage Reports

### Codecov

Coverage reports تُرفع تلقائياً إلى Codecov:
- Backend coverage
- Frontend coverage (if configured)

### View Coverage

1. اذهب إلى Codecov dashboard
2. اختر repository
3. شاهد coverage reports

## Docker Images

### Pull Images

```bash
# Pull latest image
docker pull ghcr.io/your-username/shopfree-api:main
docker pull ghcr.io/your-username/shopfree-frontend:main

# Pull specific version
docker pull ghcr.io/your-username/shopfree-api:v1.0.0
```

### Use Images

```bash
# Run API
docker run -p 5001:80 ghcr.io/your-username/shopfree-api:main

# Run Frontend
docker run -p 3000:80 ghcr.io/your-username/shopfree-frontend:main
```

## Best Practices

1. **Always run tests locally** قبل Push
2. **Check CI status** قبل Merge
3. **Review artifacts** عند فشل CI
4. **Keep dependencies updated**
5. **Use meaningful commit messages**

## Troubleshooting

### CI Fails on Build

```bash
# Run locally
dotnet build --configuration Release
cd shopfree-frontend && npm run build
```

### CI Fails on Tests

```bash
# Run tests locally
dotnet test
cd shopfree-frontend && npm run test
```

### Docker Build Fails

```bash
# Build locally
docker build -t shopfree-api ./ShopFree.API
docker build -t shopfree-frontend ./shopfree-frontend
```

## Status Badges

أضف هذه badges إلى README:

```markdown
![CI](https://github.com/your-username/shopfree/workflows/CI%20-%20Full%20Pipeline/badge.svg)
![Tests](https://github.com/your-username/shopfree/workflows/CI%20-%20Tests/badge.svg)
![Build](https://github.com/your-username/shopfree/workflows/CI%20-%20Build/badge.svg)
```

## المزيد من المعلومات

- [GitHub Actions Documentation](https://docs.github.com/en/actions)
- [Codecov Documentation](https://docs.codecov.com/)
- [Docker Documentation](https://docs.docker.com/)

