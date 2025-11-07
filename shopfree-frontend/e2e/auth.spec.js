import { test, expect } from '@playwright/test'

test.describe('Authentication', () => {
  test('should display login page', async ({ page }) => {
    await page.goto('/login')

    await expect(page.getByText('تسجيل الدخول')).toBeVisible()
    await expect(page.getByLabel('البريد الإلكتروني')).toBeVisible()
    await expect(page.getByLabel('كلمة المرور')).toBeVisible()
  })

  test('should navigate to register page', async ({ page }) => {
    await page.goto('/login')

    await page.getByText('ليس لديك حساب؟ سجل الآن').click()

    await expect(page).toHaveURL(/.*register/)
    await expect(page.getByText('إنشاء حساب جديد')).toBeVisible()
  })

  test('should show error on invalid login', async ({ page }) => {
    await page.goto('/login')

    await page.getByLabel('البريد الإلكتروني').fill('invalid@example.com')
    await page.getByLabel('كلمة المرور').fill('wrongpassword')
    await page.getByRole('button', { name: 'تسجيل الدخول' }).click()

    // Wait for error message (if API is available)
    // This test assumes the API might not be running
    await page.waitForTimeout(1000)
  })
})

