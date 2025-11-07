# دليل الاختبارات الآلية

دليل شامل لاختبارات منصة ShopFree.

## البنية

### Backend Tests

#### Unit Tests (`ShopFree.UnitTests`)
- اختبارات الوحدات للمكونات الفردية
- استخدام Moq للـ Mocking
- FluentAssertions للـ Assertions
- اختبارات Domain Entities
- اختبارات Command/Query Handlers

#### Integration Tests (`ShopFree.IntegrationTests`)
- اختبارات التكامل للـ API
- استخدام In-Memory Database
- اختبارات Controllers
- اختبارات End-to-End للـ API

### Frontend Tests

#### Unit Tests (Vitest)
- اختبارات المكونات React
- استخدام React Testing Library
- اختبارات الصفحات والـ Context

#### E2E Tests (Playwright)
- اختبارات End-to-End
- اختبارات سيناريوهات المستخدم الكاملة
- اختبارات عبر المتصفحات

## تشغيل الاختبارات

### Backend Tests

```bash
# تشغيل جميع الاختبارات
dotnet test

# تشغيل Unit Tests فقط
dotnet test ShopFree.UnitTests/ShopFree.UnitTests.csproj

# تشغيل Integration Tests فقط
dotnet test ShopFree.IntegrationTests/ShopFree.IntegrationTests.csproj

# مع تغطية الكود
dotnet test --collect:"XPlat Code Coverage"
```

### Frontend Tests

```bash
cd shopfree-frontend

# تشغيل Unit Tests
npm run test

# تشغيل Unit Tests مع UI
npm run test:ui

# تشغيل Unit Tests مع تغطية الكود
npm run test:coverage

# تشغيل E2E Tests
npm run test:e2e
```

## CI/CD

تم إعداد GitHub Actions لتشغيل الاختبارات تلقائياً عند:
- Push إلى main أو develop
- Pull Request

راجع `.github/workflows/tests.yml` للتفاصيل.

## تغطية الكود

### Backend
- استخدام Coverlet لتغطية الكود
- التقارير متاحة في `coverage.cobertura.xml`

### Frontend
- استخدام Vitest Coverage
- التقارير متاحة في `coverage/`

## كتابة اختبارات جديدة

### Unit Test Example (Backend)

```csharp
public class MyCommandHandlerTests
{
    [Fact]
    public async Task Handle_WithValidInput_ShouldReturnSuccess()
    {
        // Arrange
        var mockRepo = new Mock<IRepository>();
        var handler = new MyCommandHandler(mockRepo.Object);
        
        // Act
        var result = await handler.Handle(new MyCommand(), CancellationToken.None);
        
        // Assert
        result.Should().NotBeNull();
    }
}
```

### Integration Test Example (Backend)

```csharp
public class MyControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    
    public MyControllerTests(CustomWebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }
    
    [Fact]
    public async Task GetEndpoint_ShouldReturnOk()
    {
        var response = await _client.GetAsync("/api/endpoint");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
```

### Unit Test Example (Frontend)

```javascript
import { describe, it, expect } from 'vitest'
import { render, screen } from '@testing-library/react'
import MyComponent from './MyComponent'

describe('MyComponent', () => {
  it('should render correctly', () => {
    render(<MyComponent />)
    expect(screen.getByText('Hello')).toBeInTheDocument()
  })
})
```

### E2E Test Example (Frontend)

```javascript
import { test, expect } from '@playwright/test'

test('should complete user flow', async ({ page }) => {
  await page.goto('/')
  await page.getByText('Login').click()
  await expect(page).toHaveURL(/.*login/)
})
```

## أفضل الممارسات

1. **Arrange-Act-Assert Pattern**: استخدم هذا النمط في جميع الاختبارات
2. **Test Isolation**: كل اختبار يجب أن يكون مستقلاً
3. **Meaningful Names**: استخدم أسماء واضحة للاختبارات
4. **One Assertion Per Test**: ركز كل اختبار على حالة واحدة
5. **Mock External Dependencies**: استخدم Mocking للتبعيات الخارجية
6. **Test Edge Cases**: اختبر الحالات الحدية والأخطاء

## التغطية المستهدفة

- **Backend**: 80%+ تغطية
- **Frontend**: 70%+ تغطية
- **Critical Paths**: 100% تغطية

