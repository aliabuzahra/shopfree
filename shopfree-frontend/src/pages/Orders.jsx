import { useState, useEffect } from 'react'
import {
  Box,
  Typography,
  Card,
  CardContent,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Paper,
  Chip,
  Button,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  Select,
  MenuItem,
  FormControl,
  InputLabel,
} from '@mui/material'
import { ordersAPI } from '../services/api'

const Orders = ({ selectedStore }) => {
  const [orders, setOrders] = useState([])
  const [loading, setLoading] = useState(true)
  const [selectedOrder, setSelectedOrder] = useState(null)
  const [open, setOpen] = useState(false)

  useEffect(() => {
    if (selectedStore) {
      loadOrders()
    }
  }, [selectedStore])

  const loadOrders = async () => {
    if (!selectedStore) return

    try {
      setLoading(true)
      const response = await ordersAPI.getByStore(selectedStore.id)
      setOrders(response.data)
    } catch (error) {
      console.error('Error loading orders:', error)
    } finally {
      setLoading(false)
    }
  }

  const handleViewOrder = (order) => {
    setSelectedOrder(order)
    setOpen(true)
  }

  const handleUpdateStatus = async (orderId, newStatus) => {
    try {
      await ordersAPI.updateStatus(orderId, newStatus)
      loadOrders()
      if (selectedOrder?.id === orderId) {
        setSelectedOrder({ ...selectedOrder, status: newStatus })
      }
    } catch (error) {
      console.error('Error updating order status:', error)
    }
  }

  const getStatusColor = (status) => {
    const colors = {
      0: 'default', // Pending
      1: 'info', // Confirmed
      2: 'warning', // Processing
      3: 'primary', // Shipped
      4: 'success', // Delivered
      5: 'error', // Cancelled
    }
    return colors[status] || 'default'
  }

  const getStatusText = (status) => {
    const texts = {
      0: 'قيد الانتظار',
      1: 'مؤكد',
      2: 'قيد المعالجة',
      3: 'تم الشحن',
      4: 'تم التسليم',
      5: 'ملغي',
    }
    return texts[status] || 'غير معروف'
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
      <Typography variant="h4" sx={{ mb: 3 }}>
        الطلبات - {selectedStore.name}
      </Typography>

      {loading ? (
        <Typography>جاري التحميل...</Typography>
      ) : orders.length === 0 ? (
        <Card>
          <CardContent>
            <Typography align="center" color="text.secondary">
              لا توجد طلبات
            </Typography>
          </CardContent>
        </Card>
      ) : (
        <TableContainer component={Paper}>
          <Table>
            <TableHead>
              <TableRow>
                <TableCell>رقم الطلب</TableCell>
                <TableCell>اسم العميل</TableCell>
                <TableCell>البريد الإلكتروني</TableCell>
                <TableCell>المبلغ الإجمالي</TableCell>
                <TableCell>الحالة</TableCell>
                <TableCell>تاريخ الإنشاء</TableCell>
                <TableCell>الإجراءات</TableCell>
              </TableRow>
            </TableHead>
            <TableBody>
              {orders.map((order) => (
                <TableRow key={order.id}>
                  <TableCell>{order.orderNumber}</TableCell>
                  <TableCell>{order.customerName}</TableCell>
                  <TableCell>{order.customerEmail}</TableCell>
                  <TableCell>{order.totalAmount} ر.س</TableCell>
                  <TableCell>
                    <Chip
                      label={getStatusText(order.status)}
                      color={getStatusColor(order.status)}
                      size="small"
                    />
                  </TableCell>
                  <TableCell>
                    {new Date(order.createdAt).toLocaleDateString('ar-SA')}
                  </TableCell>
                  <TableCell>
                    <Button
                      size="small"
                      onClick={() => handleViewOrder(order)}
                    >
                      عرض التفاصيل
                    </Button>
                  </TableCell>
                </TableRow>
              ))}
            </TableBody>
          </Table>
        </TableContainer>
      )}

      <Dialog open={open} onClose={() => setOpen(false)} maxWidth="md" fullWidth>
        <DialogTitle>تفاصيل الطلب</DialogTitle>
        <DialogContent>
          {selectedOrder && (
            <Box>
              <Typography variant="h6" gutterBottom>
                رقم الطلب: {selectedOrder.orderNumber}
              </Typography>
              <Typography variant="body2" color="text.secondary" gutterBottom>
                العميل: {selectedOrder.customerName}
              </Typography>
              <Typography variant="body2" color="text.secondary" gutterBottom>
                البريد: {selectedOrder.customerEmail}
              </Typography>
              <Typography variant="body2" color="text.secondary" gutterBottom>
                الهاتف: {selectedOrder.customerPhone}
              </Typography>
              <Typography variant="body2" color="text.secondary" gutterBottom>
                العنوان: {selectedOrder.shippingAddress}
              </Typography>
              <Typography variant="h6" sx={{ mt: 2 }}>
                العناصر:
              </Typography>
              {selectedOrder.items?.map((item) => (
                <Box key={item.id} sx={{ mt: 1 }}>
                  <Typography variant="body2">
                    {item.productName} - الكمية: {item.quantity} - السعر: {item.unitPrice} ر.س
                  </Typography>
                </Box>
              ))}
              <Typography variant="h6" sx={{ mt: 2 }}>
                المبلغ الإجمالي: {selectedOrder.totalAmount} ر.س
              </Typography>
              <FormControl fullWidth sx={{ mt: 2 }}>
                <InputLabel>تحديث الحالة</InputLabel>
                <Select
                  value={selectedOrder.status}
                  onChange={(e) => handleUpdateStatus(selectedOrder.id, e.target.value)}
                  label="تحديث الحالة"
                >
                  <MenuItem value={0}>قيد الانتظار</MenuItem>
                  <MenuItem value={1}>مؤكد</MenuItem>
                  <MenuItem value={2}>قيد المعالجة</MenuItem>
                  <MenuItem value={3}>تم الشحن</MenuItem>
                  <MenuItem value={4}>تم التسليم</MenuItem>
                  <MenuItem value={5}>ملغي</MenuItem>
                </Select>
              </FormControl>
            </Box>
          )}
        </DialogContent>
        <DialogActions>
          <Button onClick={() => setOpen(false)}>إغلاق</Button>
        </DialogActions>
      </Dialog>
    </Box>
  )
}

export default Orders

