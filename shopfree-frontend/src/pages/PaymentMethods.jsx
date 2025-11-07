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
  FormControl,
  InputLabel,
  Select,
  MenuItem,
  Switch,
  FormControlLabel,
} from '@mui/material'
import { Add as AddIcon } from '@mui/icons-material'
import { paymentMethodsAPI } from '../services/api'

const PaymentMethods = ({ selectedStore }) => {
  const [paymentMethods, setPaymentMethods] = useState([])
  const [loading, setLoading] = useState(true)
  const [open, setOpen] = useState(false)
  const [editingMethod, setEditingMethod] = useState(null)
  const [formData, setFormData] = useState({
    type: 1,
    title: '',
    details: '',
    isActive: true,
  })
  const [error, setError] = useState('')

  useEffect(() => {
    if (selectedStore) {
      loadPaymentMethods()
    }
  }, [selectedStore])

  const loadPaymentMethods = async () => {
    if (!selectedStore) return

    try {
      setLoading(true)
      const response = await paymentMethodsAPI.getByStore(selectedStore.id)
      setPaymentMethods(response.data)
    } catch (error) {
      console.error('Error loading payment methods:', error)
    } finally {
      setLoading(false)
    }
  }

  const handleOpen = (method = null) => {
    if (method) {
      setEditingMethod(method)
      setFormData({
        type: method.type,
        title: method.title || '',
        details: method.details || '',
        isActive: method.isActive,
      })
    } else {
      setEditingMethod(null)
      setFormData({
        type: 1,
        title: '',
        details: '',
        isActive: true,
      })
    }
    setOpen(true)
    setError('')
  }

  const handleClose = () => {
    setOpen(false)
    setEditingMethod(null)
    setFormData({
      type: 1,
      title: '',
      details: '',
      isActive: true,
    })
    setError('')
  }

  const handleChange = (e) => {
    const value = e.target.type === 'checkbox' ? e.target.checked : e.target.value
    setFormData({
      ...formData,
      [e.target.name]: value,
    })
  }

  const handleSubmit = async () => {
    if (!selectedStore) return

    try {
      setError('')
      const data = {
        ...formData,
        storeId: selectedStore.id,
      }

      if (editingMethod) {
        await paymentMethodsAPI.update(editingMethod.id, data)
      } else {
        await paymentMethodsAPI.create(data)
      }

      handleClose()
      loadPaymentMethods()
    } catch (error) {
      setError(error.response?.data?.message || 'فشل حفظ طريقة الدفع')
    }
  }

  const getTypeText = (type) => {
    const types = {
      1: 'التحويل البنكي',
      2: 'الدفع عند الاستلام',
      3: 'المحفظة الإلكترونية',
    }
    return types[type] || 'غير معروف'
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
        <Typography variant="h4">طرق الدفع - {selectedStore.name}</Typography>
        <Button
          variant="contained"
          startIcon={<AddIcon />}
          onClick={() => handleOpen()}
        >
          إضافة طريقة دفع
        </Button>
      </Box>

      {loading ? (
        <Typography>جاري التحميل...</Typography>
      ) : paymentMethods.length === 0 ? (
        <Card>
          <CardContent>
            <Typography align="center" color="text.secondary">
              لا توجد طرق دفع. ابدأ بإضافة طريقة دفع جديدة!
            </Typography>
          </CardContent>
        </Card>
      ) : (
        <Grid container spacing={3}>
          {paymentMethods.map((method) => (
            <Grid item xs={12} sm={6} md={4} key={method.id}>
              <Card>
                <CardContent>
                  <Typography variant="h6">{getTypeText(method.type)}</Typography>
                  {method.title && (
                    <Typography variant="subtitle2" color="text.secondary" sx={{ mt: 1 }}>
                      {method.title}
                    </Typography>
                  )}
                  {method.details && (
                    <Typography variant="body2" color="text.secondary" sx={{ mt: 1 }}>
                      {method.details}
                    </Typography>
                  )}
                  <Typography
                    variant="caption"
                    color={method.isActive ? 'success.main' : 'error.main'}
                    sx={{ display: 'block', mt: 1 }}
                  >
                    {method.isActive ? 'نشط' : 'غير نشط'}
                  </Typography>
                  <Button
                    size="small"
                    onClick={() => handleOpen(method)}
                    sx={{ mt: 2 }}
                  >
                    تعديل
                  </Button>
                </CardContent>
              </Card>
            </Grid>
          ))}
        </Grid>
      )}

      <Dialog open={open} onClose={handleClose} maxWidth="sm" fullWidth>
        <DialogTitle>
          {editingMethod ? 'تعديل طريقة الدفع' : 'إضافة طريقة دفع جديدة'}
        </DialogTitle>
        <DialogContent>
          {error && (
            <Alert severity="error" sx={{ mb: 2 }}>
              {error}
            </Alert>
          )}
          <FormControl fullWidth margin="normal">
            <InputLabel>نوع طريقة الدفع</InputLabel>
            <Select
              name="type"
              value={formData.type}
              onChange={handleChange}
              label="نوع طريقة الدفع"
            >
              <MenuItem value={1}>التحويل البنكي</MenuItem>
              <MenuItem value={2}>الدفع عند الاستلام</MenuItem>
              <MenuItem value={3}>المحفظة الإلكترونية</MenuItem>
            </Select>
          </FormControl>
          <TextField
            fullWidth
            label="العنوان (اختياري)"
            name="title"
            value={formData.title}
            onChange={handleChange}
            margin="normal"
          />
          <TextField
            fullWidth
            label="التفاصيل"
            name="details"
            value={formData.details}
            onChange={handleChange}
            multiline
            rows={4}
            margin="normal"
            helperText="مثال: رقم الحساب البنكي، معلومات المحفظة الإلكترونية، إلخ"
          />
          <FormControlLabel
            control={
              <Switch
                checked={formData.isActive}
                onChange={handleChange}
                name="isActive"
              />
            }
            label="نشط"
            sx={{ mt: 2 }}
          />
        </DialogContent>
        <DialogActions>
          <Button onClick={handleClose}>إلغاء</Button>
          <Button onClick={handleSubmit} variant="contained">
            {editingMethod ? 'حفظ' : 'إنشاء'}
          </Button>
        </DialogActions>
      </Dialog>
    </Box>
  )
}

export default PaymentMethods

