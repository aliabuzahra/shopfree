import { describe, it, expect, vi, beforeEach } from 'vitest'
import { render, screen, fireEvent, waitFor } from '@testing-library/react'
import { BrowserRouter } from 'react-router-dom'
import Login from '../Login'
import { AuthProvider } from '../../context/AuthContext'
import * as authAPI from '../../services/api'

vi.mock('../../services/api')
vi.mock('react-router-dom', async () => {
  const actual = await vi.importActual('react-router-dom')
  return {
    ...actual,
    useNavigate: () => vi.fn(),
  }
})

const renderLogin = () => {
  return render(
    <BrowserRouter>
      <AuthProvider>
        <Login />
      </AuthProvider>
    </BrowserRouter>
  )
}

describe('Login', () => {
  beforeEach(() => {
    vi.clearAllMocks()
  })

  it('should render login form', () => {
    renderLogin()

    expect(screen.getByLabelText(/البريد الإلكتروني/i)).toBeInTheDocument()
    expect(screen.getByLabelText(/كلمة المرور/i)).toBeInTheDocument()
    expect(screen.getByRole('button', { name: /تسجيل الدخول/i })).toBeInTheDocument()
  })

  it('should show error message on failed login', async () => {
    authAPI.authAPI.login = vi.fn().mockRejectedValue({
      response: { data: { message: 'Invalid credentials' } },
    })

    renderLogin()

    const emailInput = screen.getByLabelText(/البريد الإلكتروني/i)
    const passwordInput = screen.getByLabelText(/كلمة المرور/i)
    const submitButton = screen.getByRole('button', { name: /تسجيل الدخول/i })

    fireEvent.change(emailInput, { target: { value: 'test@example.com' } })
    fireEvent.change(passwordInput, { target: { value: 'wrongpassword' } })
    fireEvent.click(submitButton)

    await waitFor(() => {
      expect(screen.getByText(/Invalid credentials/i)).toBeInTheDocument()
    })
  })
})

