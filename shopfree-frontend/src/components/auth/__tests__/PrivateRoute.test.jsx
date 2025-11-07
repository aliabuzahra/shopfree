import { describe, it, expect, vi, beforeEach } from 'vitest'
import { render, screen } from '@testing-library/react'
import { BrowserRouter } from 'react-router-dom'
import PrivateRoute from '../PrivateRoute'
import { AuthProvider, useAuth } from '../../../context/AuthContext'

// Mock useAuth
vi.mock('../../../context/AuthContext', async () => {
  const actual = await vi.importActual('../../../context/AuthContext')
  return {
    ...actual,
    useAuth: vi.fn(),
  }
})

describe('PrivateRoute', () => {
  beforeEach(() => {
    vi.clearAllMocks()
  })

  it('should render children when authenticated', () => {
    // Mock authenticated state
    vi.mocked(useAuth).mockReturnValue({
      isAuthenticated: true,
      loading: false,
      user: { id: 1, email: 'test@example.com' },
      login: vi.fn(),
      logout: vi.fn(),
      register: vi.fn(),
    })

    render(
      <BrowserRouter>
        <AuthProvider>
          <PrivateRoute>
            <div>Protected Content</div>
          </PrivateRoute>
        </AuthProvider>
      </BrowserRouter>
    )

    expect(screen.getByText('Protected Content')).toBeInTheDocument()
  })

  it('should redirect when not authenticated', () => {
    // Mock unauthenticated state
    vi.mocked(useAuth).mockReturnValue({
      isAuthenticated: false,
      loading: false,
      user: null,
      login: vi.fn(),
      logout: vi.fn(),
      register: vi.fn(),
    })

    render(
      <BrowserRouter>
        <AuthProvider>
          <PrivateRoute>
            <div>Protected Content</div>
          </PrivateRoute>
        </AuthProvider>
      </BrowserRouter>
    )

    expect(screen.queryByText('Protected Content')).not.toBeInTheDocument()
  })
})

