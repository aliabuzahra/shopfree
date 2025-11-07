import axios from 'axios'

const API_BASE_URL = import.meta.env.VITE_API_URL || 'http://localhost:5000/api'

const api = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
})

// Request interceptor to add token
api.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem('token')
    if (token) {
      config.headers.Authorization = `Bearer ${token}`
    }
    return config
  },
  (error) => {
    return Promise.reject(error)
  }
)

// Response interceptor to handle errors
api.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response?.status === 401) {
      localStorage.removeItem('token')
      localStorage.removeItem('user')
      window.location.href = '/login'
    }
    return Promise.reject(error)
  }
)

// Auth API
export const authAPI = {
  register: (data) => api.post('/auth/register', data),
  login: (data) => api.post('/auth/login', data),
}

// Stores API
export const storesAPI = {
  getAll: () => api.get('/stores/my-stores'),
  getById: (id) => api.get(`/stores/${id}`),
  create: (data) => api.post('/stores', data),
  update: (id, data) => api.put(`/stores/${id}`, data),
}

// Products API
export const productsAPI = {
  getByStore: (storeId, activeOnly = false) =>
    api.get(`/products/store/${storeId}`, { params: { activeOnly } }),
  getById: (id) => api.get(`/products/${id}`),
  create: (data) => api.post('/products', data),
  update: (id, data) => api.put(`/products/${id}`, data),
  delete: (id) => api.delete(`/products/${id}`),
}

// Orders API
export const ordersAPI = {
  getByStore: (storeId) => api.get(`/orders/store/${storeId}`),
  getById: (id) => api.get(`/orders/${id}`),
  getByOrderNumber: (orderNumber) => api.get(`/orders/number/${orderNumber}`),
  create: (data) => api.post('/orders', data),
  updateStatus: (id, status) => api.put(`/orders/${id}/status`, { status }),
}

// Payment Methods API
export const paymentMethodsAPI = {
  getByStore: (storeId, activeOnly = false) =>
    api.get(`/paymentmethods/store/${storeId}`, { params: { activeOnly } }),
  create: (data) => api.post('/paymentmethods', data),
  update: (id, data) => api.put(`/paymentmethods/${id}`, data),
}

export default api

