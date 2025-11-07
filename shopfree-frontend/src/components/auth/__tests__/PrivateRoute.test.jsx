import { describe, it, expect, vi } from 'vitest'
import { render, screen } from '@testing-library/react'
import { BrowserRouter } from 'react-router-dom'
import PrivateRoute from '../PrivateRoute'
import { AuthProvider } from '../../../context/AuthContext'

// Mock useAuth
vi.mock('../../../context/AuthContext', async () => {
  const actual = await vi.importActual('../../../context/AuthContext')
  return {
    ...actual,
    useAuth: vi.fn(),
  }
})

const renderWithRouter = (component, { isAuthenticated = false } = {}) => {
  const mockUseAuth = (await import('../../../context/AuthContext')).useAuth
  mockUseAuth.mockReturnValue({
    isAuthenticated,
    loading: false,
    user: isAuthenticated ? { id: 1, email: 'test@example.com' } : null,
  })

  return render(
    <BrowserRouter>
      <AuthProvider>
        {component}
      </AuthProvider>
    </BrowserRouter>
  )
}

describe('PrivateRoute', () => {
  it('should render children when authenticated', () => {
    renderWithRouter(
      <PrivateRoute>
        <div>Protected Content</div>
      </PrivateRoute>,
      { isAuthenticated: true }
    )

    expect(screen.getByText('Protected Content')).toBeInTheDocument()
  })

  it('should redirect when not authenticated', () => {
    renderWithRouter(
      <PrivateRoute>
        <div>Protected Content</div>
      </PrivateRoute>,
      { isAuthenticated: false }
    )

    expect(screen.queryByText('Protected Content')).not.toBeInTheDocument()
  })
})

