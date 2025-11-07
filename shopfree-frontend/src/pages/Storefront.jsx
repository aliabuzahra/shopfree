import { useState, useEffect } from 'react'
import { useParams } from 'react-router-dom'
import {
  Box,
  Container,
  Typography,
  Grid,
  Card,
  CardContent,
  CardMedia,
  Button,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  TextField,
  FormControl,
  InputLabel,
  Select,
  MenuItem,
  Alert,
  Badge,
  IconButton,
} from '@mui/material'
import { ShoppingCart as ShoppingCartIcon } from '@mui/icons-material'
import { productsAPI, ordersAPI, storesAPI } from '../services/api'

const Storefront = () => {
  const { subdomain } = useParams()
  const [store, setStore] = useState(null)
  const [products, setProducts] = useState([])
  const [cart, setCart] = useState([])
  const [open, setOpen] = useState(false)
  const [loading, setLoading] = useState(true)
  const [formData, setFormData] = useState({
    customerName: '',
    customerEmail: '',
    customerPhone: '',
    shippingAddress: '',
    paymentMethodType: 1,
    paymentDetails: '',
    notes: '',
  })
  const [error, setError] = useState('')
  const [submitting, setSubmitting] = useState(false)

  useEffect(() => {
    loadStore()
  }, [subdomain])

  useEffect(() => {
    if (store) {
      loadProducts()
    }
  }, [store])

  const loadStore = async () => {
    try {
      // In a real app, you'd have an API endpoint to get store by subdomain
      // For now, we'll use a workaround
      const response = await storesAPI.getAll()
      const foundStore = response.data.find(s => s.subdomain === subdomain)
      if (foundStore) {
        setStore(foundStore)
      }
    } catch (error) {
      console.error('Error loading store:', error)
    } finally {
      setLoading(false)
    }
  }

  const loadProducts = async () => {
    if (!store) return

    try {
      const response = await productsAPI.getByStore(store.id, true) // Active only
      setProducts(response.data)
    } catch (error) {
      console.error('Error loading products:', error)
    }
  }

  const addToCart = (product) => {
    const existingItem = cart.find(item => item.productId === product.id)
    
    if (existingItem) {
      setCart(cart.map(item =>
        item.productId === product.id
          ? { ...item, quantity: item.quantity + 1 }
          : item
      ))
    } else {
      setCart([...cart, {
        productId: product.id,
        productName: product.name,
        productPrice: product.price,
        quantity: 1,
      }])
    }
  }

  const removeFromCart = (productId) => {
    setCart(cart.filter(item => item.productId !== productId))
  }

  const updateCartQuantity = (productId, quantity) => {
    if (quantity <= 0) {
      removeFromCart(productId)
    } else {
      setCart(cart.map(item =>
        item.productId === productId
          ? { ...item, quantity }
          : item
      ))
    }
  }

  const getCartTotal = () => {
    return cart.reduce((total, item) => total + (item.productPrice * item.quantity), 0)
  }

  const handleCheckout = () => {
    if (cart.length === 0) {
      setError('السلة فارغة')
      return
    }
    setOpen(true)
    setError('')
  }

  const handleChange = (e) => {
    setFormData({
      ...formData,
      [e.target.name]: e.target.value,
    })
  }

  const handleSubmit = async () => {
    if (!store) return

    try {
      setError('')
      setSubmitting(true)

      const orderData = {
        storeId: store.id,
        customerName: formData.customerName,
        customerEmail: formData.customerEmail,
        customerPhone: formData.customerPhone,
        shippingAddress: formData.shippingAddress,
        paymentMethodType: parseInt(formData.paymentMethodType),
        paymentDetails: formData.paymentDetails,
        notes: formData.notes,
        items: cart.map(item => ({
          productId: item.productId,
          quantity: item.quantity,
        })),
      }

      await ordersAPI.create(orderData)
      
      // Clear cart and close dialog
      setCart([])
      setOpen(false)
      setFormData({
        customerName: '',
        customerEmail: '',
        customerPhone: '',
        shippingAddress: '',
        paymentMethodType: 1,
        paymentDetails: '',
        notes: '',
      })
      
      alert('تم إنشاء الطلب بنجاح!')
    } catch (error) {
      setError(error.response?.data?.message || 'فشل إنشاء الطلب')
    } finally {
      setSubmitting(false)
    }
  }

  if (loading) {
    return (
      <Container>
        <Typography>جاري التحميل...</Typography>
      </Container>
    )
  }

  if (!store) {
    return (
      <Container>
        <Typography variant="h4" color="error">
          المتجر غير موجود
        </Typography>
      </Container>
    )
  }

  return (
    <Container maxWidth="lg">
      <Box sx={{ my: 4 }}>
        <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 4 }}>
          <Box>
            <Typography variant="h3">{store.name}</Typography>
            {store.description && (
              <Typography variant="body1" color="text.secondary" sx={{ mt: 1 }}>
                {store.description}
              </Typography>
            )}
          </Box>
          <IconButton onClick={handleCheckout} color="primary" size="large">
            <Badge badgeContent={cart.length} color="error">
              <ShoppingCartIcon />
            </Badge>
          </IconButton>
        </Box>

        <Grid container spacing={3}>
          {products.map((product) => (
            <Grid item xs={12} sm={6} md={4} key={product.id}>
              <Card>
                {product.imageUrl && (
                  <CardMedia
                    component="img"
                    height="200"
                    image={product.imageUrl}
                    alt={product.name}
                  />
                )}
                <CardContent>
                  <Typography variant="h6">{product.name}</Typography>
                  {product.description && (
                    <Typography variant="body2" color="text.secondary" sx={{ mt: 1 }}>
                      {product.description}
                    </Typography>
                  )}
                  <Typography variant="h6" color="primary" sx={{ mt: 2 }}>
                    {product.price} ر.س
                  </Typography>
                  <Typography variant="body2" color="text.secondary" sx={{ mt: 1 }}>
                    المخزون: {product.stock}
                  </Typography>
                  <Button
                    variant="contained"
                    fullWidth
                    sx={{ mt: 2 }}
                    onClick={() => addToCart(product)}
                    disabled={product.stock === 0}
                  >
                    {product.stock === 0 ? 'نفد المخزون' : 'إضافة إلى السلة'}
                  </Button>
                </CardContent>
              </Card>
            </Grid>
          ))}
        </Grid>

        {products.length === 0 && (
          <Card sx={{ mt: 4 }}>
            <CardContent>
              <Typography align="center" color="text.secondary">
                لا توجد منتجات متاحة حالياً
              </Typography>
            </CardContent>
          </Card>
        )}
      </Box>

      <Dialog open={open} onClose={() => setOpen(false)} maxWidth="sm" fullWidth>
        <DialogTitle>إتمام الطلب</DialogTitle>
        <DialogContent>
          {error && (
            <Alert severity="error" sx={{ mb: 2 }}>
              {error}
            </Alert>
          )}

          <Box sx={{ mb: 2 }}>
            <Typography variant="h6" gutterBottom>
              العناصر في السلة:
            </Typography>
            {cart.map((item) => (
              <Box key={item.productId} sx={{ display: 'flex', justifyContent: 'space-between', mb: 1 }}>
                <Typography variant="body2">
                  {item.productName} x {item.quantity}
                </Typography>
                <Typography variant="body2">
                  {item.productPrice * item.quantity} ر.س
                </Typography>
              </Box>
            ))}
            <Typography variant="h6" sx={{ mt: 2 }}>
              الإجمالي: {getCartTotal()} ر.س
            </Typography>
          </Box>

          <TextField
            fullWidth
            label="الاسم الكامل"
            name="customerName"
            value={formData.customerName}
            onChange={handleChange}
            required
            margin="normal"
          />
          <TextField
            fullWidth
            label="البريد الإلكتروني"
            name="customerEmail"
            type="email"
            value={formData.customerEmail}
            onChange={handleChange}
            required
            margin="normal"
          />
          <TextField
            fullWidth
            label="رقم الهاتف"
            name="customerPhone"
            value={formData.customerPhone}
            onChange={handleChange}
            required
            margin="normal"
          />
          <TextField
            fullWidth
            label="عنوان الشحن"
            name="shippingAddress"
            value={formData.shippingAddress}
            onChange={handleChange}
            required
            multiline
            rows={3}
            margin="normal"
          />
          <FormControl fullWidth margin="normal">
            <InputLabel>طريقة الدفع</InputLabel>
            <Select
              name="paymentMethodType"
              value={formData.paymentMethodType}
              onChange={handleChange}
              label="طريقة الدفع"
            >
              <MenuItem value={1}>التحويل البنكي</MenuItem>
              <MenuItem value={2}>الدفع عند الاستلام</MenuItem>
              <MenuItem value={3}>المحفظة الإلكترونية</MenuItem>
            </Select>
          </FormControl>
          {formData.paymentMethodType == 1 && (
            <TextField
              fullWidth
              label="تفاصيل التحويل البنكي"
              name="paymentDetails"
              value={formData.paymentDetails}
              onChange={handleChange}
              multiline
              rows={2}
              margin="normal"
              helperText="مثال: رقم الحساب، اسم البنك"
            />
          )}
          {formData.paymentMethodType == 3 && (
            <TextField
              fullWidth
              label="تفاصيل المحفظة الإلكترونية"
              name="paymentDetails"
              value={formData.paymentDetails}
              onChange={handleChange}
              multiline
              rows={2}
              margin="normal"
              helperText="مثال: رقم المحفظة، نوع المحفظة"
            />
          )}
          <TextField
            fullWidth
            label="ملاحظات (اختياري)"
            name="notes"
            value={formData.notes}
            onChange={handleChange}
            multiline
            rows={2}
            margin="normal"
          />
        </DialogContent>
        <DialogActions>
          <Button onClick={() => setOpen(false)}>إلغاء</Button>
          <Button onClick={handleSubmit} variant="contained" disabled={submitting}>
            {submitting ? 'جاري المعالجة...' : 'تأكيد الطلب'}
          </Button>
        </DialogActions>
      </Dialog>
    </Container>
  )
}

export default Storefront

