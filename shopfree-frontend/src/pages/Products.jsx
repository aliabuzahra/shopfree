import { useState, useEffect } from 'react'
import {
  Box,
  Button,
  Card,
  CardContent,
  Typography,
  Grid,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  TextField,
  Alert,
  IconButton,
  Menu,
  MenuItem,
} from '@mui/material'
import { Add as AddIcon, MoreVert as MoreVertIcon } from '@mui/icons-material'
import { productsAPI } from '../services/api'

const Products = ({ selectedStore }) => {
  const [products, setProducts] = useState([])
  const [loading, setLoading] = useState(true)
  const [open, setOpen] = useState(false)
  const [editingProduct, setEditingProduct] = useState(null)
  const [formData, setFormData] = useState({
    name: '',
    description: '',
    price: '',
    imageUrl: '',
    stock: '0',
  })
  const [error, setError] = useState('')
  const [anchorEl, setAnchorEl] = useState(null)
  const [selectedProduct, setSelectedProduct] = useState(null)

  useEffect(() => {
    if (selectedStore) {
      loadProducts()
    }
  }, [selectedStore])

  const loadProducts = async () => {
    if (!selectedStore) return
    
    try {
      setLoading(true)
      const response = await productsAPI.getByStore(selectedStore.id)
      setProducts(response.data)
    } catch (error) {
      console.error('Error loading products:', error)
    } finally {
      setLoading(false)
    }
  }

  const handleOpen = (product = null) => {
    if (product) {
      setEditingProduct(product)
      setFormData({
        name: product.name,
        description: product.description || '',
        price: product.price.toString(),
        imageUrl: product.imageUrl || '',
        stock: product.stock.toString(),
      })
    } else {
      setEditingProduct(null)
      setFormData({
        name: '',
        description: '',
        price: '',
        imageUrl: '',
        stock: '0',
      })
    }
    setOpen(true)
    setError('')
  }

  const handleClose = () => {
    setOpen(false)
    setEditingProduct(null)
    setFormData({
      name: '',
      description: '',
      price: '',
      imageUrl: '',
      stock: '0',
    })
    setError('')
  }

  const handleChange = (e) => {
    setFormData({
      ...formData,
      [e.target.name]: e.target.value,
    })
  }

  const handleSubmit = async () => {
    if (!selectedStore) return

    try {
      setError('')
      const data = {
        ...formData,
        storeId: selectedStore.id,
        price: parseFloat(formData.price),
        stock: parseInt(formData.stock),
      }

      if (editingProduct) {
        await productsAPI.update(editingProduct.id, data)
      } else {
        await productsAPI.create(data)
      }
      
      handleClose()
      loadProducts()
    } catch (error) {
      setError(error.response?.data?.message || 'فشل حفظ المنتج')
    }
  }

  const handleDelete = async (productId) => {
    if (!window.confirm('هل أنت متأكد من حذف هذا المنتج؟')) return

    try {
      await productsAPI.delete(productId)
      setAnchorEl(null)
      loadProducts()
    } catch (error) {
      console.error('Error deleting product:', error)
    }
  }

  if (!selectedStore) {
    return (
      <Box>
        <Typography variant="h6" color="text.secondary">
          يرجى اختيار متجر أولاً
        </Typography>
      </Box>
    )
  }

  return (
    <Box>
      <Box sx={{ display: 'flex', justifyContent: 'space-between', mb: 3 }}>
        <Typography variant="h4">المنتجات - {selectedStore.name}</Typography>
        <Button
          variant="contained"
          startIcon={<AddIcon />}
          onClick={() => handleOpen()}
        >
          إضافة منتج جديد
        </Button>
      </Box>

      {loading ? (
        <Typography>جاري التحميل...</Typography>
      ) : products.length === 0 ? (
        <Card>
          <CardContent>
            <Typography align="center" color="text.secondary">
              لا توجد منتجات. ابدأ بإضافة منتج جديد!
            </Typography>
          </CardContent>
        </Card>
      ) : (
        <Grid container spacing={3}>
          {products.map((product) => (
            <Grid item xs={12} sm={6} md={4} key={product.id}>
              <Card>
                {product.imageUrl && (
                  <Box
                    component="img"
                    src={product.imageUrl}
                    alt={product.name}
                    sx={{
                      width: '100%',
                      height: 200,
                      objectFit: 'cover',
                    }}
                  />
                )}
                <CardContent>
                  <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'start' }}>
                    <Typography variant="h6">{product.name}</Typography>
                    <IconButton
                      size="small"
                      onClick={(e) => {
                        setAnchorEl(e.currentTarget)
                        setSelectedProduct(product)
                      }}
                    >
                      <MoreVertIcon />
                    </IconButton>
                  </Box>
                  <Typography variant="body2" color="text.secondary" sx={{ mt: 1 }}>
                    {product.description || 'لا يوجد وصف'}
                  </Typography>
                  <Typography variant="h6" color="primary" sx={{ mt: 2 }}>
                    {product.price} ر.س
                  </Typography>
                  <Typography variant="body2" color="text.secondary">
                    المخزون: {product.stock}
                  </Typography>
                  <Typography
                    variant="caption"
                    color={product.isActive ? 'success.main' : 'error.main'}
                    sx={{ display: 'block', mt: 1 }}
                  >
                    {product.isActive ? 'نشط' : 'غير نشط'}
                  </Typography>
                </CardContent>
              </Card>
            </Grid>
          ))}
        </Grid>
      )}

      <Menu
        anchorEl={anchorEl}
        open={Boolean(anchorEl)}
        onClose={() => setAnchorEl(null)}
      >
        <MenuItem onClick={() => {
          handleOpen(selectedProduct)
          setAnchorEl(null)
        }}>
          تعديل
        </MenuItem>
        <MenuItem onClick={() => handleDelete(selectedProduct?.id)}>
          حذف
        </MenuItem>
      </Menu>

      <Dialog open={open} onClose={handleClose} maxWidth="sm" fullWidth>
        <DialogTitle>
          {editingProduct ? 'تعديل منتج' : 'إضافة منتج جديد'}
        </DialogTitle>
        <DialogContent>
          {error && (
            <Alert severity="error" sx={{ mb: 2 }}>
              {error}
            </Alert>
          )}
          <TextField
            fullWidth
            label="اسم المنتج"
            name="name"
            value={formData.name}
            onChange={handleChange}
            required
            margin="normal"
          />
          <TextField
            fullWidth
            label="الوصف"
            name="description"
            value={formData.description}
            onChange={handleChange}
            multiline
            rows={3}
            margin="normal"
          />
          <TextField
            fullWidth
            label="السعر"
            name="price"
            type="number"
            value={formData.price}
            onChange={handleChange}
            required
            margin="normal"
            inputProps={{ min: 0, step: 0.01 }}
          />
          <TextField
            fullWidth
            label="رابط الصورة"
            name="imageUrl"
            value={formData.imageUrl}
            onChange={handleChange}
            margin="normal"
          />
          <TextField
            fullWidth
            label="المخزون"
            name="stock"
            type="number"
            value={formData.stock}
            onChange={handleChange}
            required
            margin="normal"
            inputProps={{ min: 0 }}
          />
        </DialogContent>
        <DialogActions>
          <Button onClick={handleClose}>إلغاء</Button>
          <Button onClick={handleSubmit} variant="contained">
            {editingProduct ? 'حفظ' : 'إنشاء'}
          </Button>
        </DialogActions>
      </Dialog>
    </Box>
  )
}

export default Products

