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
} from '@mui/material'
import { Add as AddIcon } from '@mui/icons-material'
import { storesAPI } from '../services/api'

const Stores = ({ onStoreSelect }) => {
  const [stores, setStores] = useState([])
  const [loading, setLoading] = useState(true)
  const [open, setOpen] = useState(false)
  const [formData, setFormData] = useState({
    name: '',
    description: '',
    subdomain: '',
    logoUrl: '',
  })
  const [error, setError] = useState('')

  useEffect(() => {
    loadStores()
  }, [])

  const loadStores = async () => {
    try {
      setLoading(true)
      const response = await storesAPI.getAll()
      setStores(response.data)
      if (response.data.length > 0 && onStoreSelect) {
        onStoreSelect(response.data[0])
      }
    } catch (error) {
      console.error('Error loading stores:', error)
    } finally {
      setLoading(false)
    }
  }

  const handleOpen = () => setOpen(true)
  const handleClose = () => {
    setOpen(false)
    setFormData({ name: '', description: '', subdomain: '', logoUrl: '' })
    setError('')
  }

  const handleChange = (e) => {
    setFormData({
      ...formData,
      [e.target.name]: e.target.value,
    })
  }

  const handleSubmit = async () => {
    try {
      setError('')
      await storesAPI.create(formData)
      handleClose()
      loadStores()
    } catch (error) {
      setError(error.response?.data?.message || 'فشل إنشاء المتجر')
    }
  }

  return (
    <Box>
      <Box sx={{ display: 'flex', justifyContent: 'space-between', mb: 3 }}>
        <Typography variant="h4">المتاجر</Typography>
        <Button
          variant="contained"
          startIcon={<AddIcon />}
          onClick={handleOpen}
        >
          إضافة متجر جديد
        </Button>
      </Box>

      {loading ? (
        <Typography>جاري التحميل...</Typography>
      ) : stores.length === 0 ? (
        <Card>
          <CardContent>
            <Typography align="center" color="text.secondary">
              لا توجد متاجر. ابدأ بإنشاء متجر جديد!
            </Typography>
          </CardContent>
        </Card>
      ) : (
        <Grid container spacing={3}>
          {stores.map((store) => (
            <Grid item xs={12} sm={6} md={4} key={store.id}>
              <Card
                sx={{ cursor: 'pointer' }}
                onClick={() => onStoreSelect && onStoreSelect(store)}
              >
                <CardContent>
                  <Typography variant="h6">{store.name}</Typography>
                  <Typography variant="body2" color="text.secondary" sx={{ mt: 1 }}>
                    {store.description || 'لا يوجد وصف'}
                  </Typography>
                  {store.subdomain && (
                    <Typography variant="caption" color="primary" sx={{ mt: 1, display: 'block' }}>
                      {store.subdomain}.shopfree.com
                    </Typography>
                  )}
                </CardContent>
              </Card>
            </Grid>
          ))}
        </Grid>
      )}

      <Dialog open={open} onClose={handleClose} maxWidth="sm" fullWidth>
        <DialogTitle>إضافة متجر جديد</DialogTitle>
        <DialogContent>
          {error && (
            <Alert severity="error" sx={{ mb: 2 }}>
              {error}
            </Alert>
          )}
          <TextField
            fullWidth
            label="اسم المتجر"
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
            label="النطاق الفرعي (Subdomain)"
            name="subdomain"
            value={formData.subdomain}
            onChange={handleChange}
            margin="normal"
            helperText="مثال: store1 (سيصبح store1.shopfree.com)"
          />
          <TextField
            fullWidth
            label="رابط الشعار"
            name="logoUrl"
            value={formData.logoUrl}
            onChange={handleChange}
            margin="normal"
          />
        </DialogContent>
        <DialogActions>
          <Button onClick={handleClose}>إلغاء</Button>
          <Button onClick={handleSubmit} variant="contained">
            إنشاء
          </Button>
        </DialogActions>
      </Dialog>
    </Box>
  )
}

export default Stores

