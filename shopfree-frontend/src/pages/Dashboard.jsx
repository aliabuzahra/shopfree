import { useState, useEffect } from 'react'
import { Routes, Route, useNavigate, useLocation } from 'react-router-dom'
import {
  Box,
  Drawer,
  AppBar,
  Toolbar,
  List,
  Typography,
  Divider,
  ListItem,
  ListItemButton,
  ListItemIcon,
  ListItemText,
  Container,
} from '@mui/material'
import {
  Dashboard as DashboardIcon,
  Store,
  Inventory,
  ShoppingCart,
  Payment,
  Logout,
} from '@mui/icons-material'
import { useAuth } from '../context/AuthContext'
import Products from './Products'
import Orders from './Orders'
import Stores from './Stores'
import PaymentMethods from './PaymentMethods'

const drawerWidth = 240

const Dashboard = () => {
  const { user, logout } = useAuth()
  const navigate = useNavigate()
  const location = useLocation()
  const [selectedStore, setSelectedStore] = useState(null)

  const menuItems = [
    { text: 'المتاجر', icon: <Store />, path: '/dashboard/stores' },
    { text: 'المنتجات', icon: <Inventory />, path: '/dashboard/products' },
    { text: 'الطلبات', icon: <ShoppingCart />, path: '/dashboard/orders' },
    { text: 'طرق الدفع', icon: <Payment />, path: '/dashboard/payment-methods' },
  ]

  const handleLogout = () => {
    logout()
    navigate('/login')
  }

  useEffect(() => {
    // Redirect to stores if on root dashboard
    if (location.pathname === '/dashboard') {
      navigate('/dashboard/stores', { replace: true })
    }
  }, [location.pathname, navigate])

  return (
    <Box sx={{ display: 'flex' }}>
      <AppBar
        position="fixed"
        sx={{ zIndex: (theme) => theme.zIndex.drawer + 1 }}
      >
        <Toolbar>
          <Typography variant="h6" noWrap component="div" sx={{ flexGrow: 1 }}>
            ShopFree - لوحة التحكم
          </Typography>
          <Typography variant="body2" sx={{ mr: 2 }}>
            {user?.email}
          </Typography>
          <ListItemButton onClick={handleLogout} sx={{ color: 'white' }}>
            <ListItemIcon sx={{ color: 'white' }}>
              <Logout />
            </ListItemIcon>
            <ListItemText primary="تسجيل الخروج" />
          </ListItemButton>
        </Toolbar>
      </AppBar>
      <Drawer
        variant="permanent"
        sx={{
          width: drawerWidth,
          flexShrink: 0,
          '& .MuiDrawer-paper': {
            width: drawerWidth,
            boxSizing: 'border-box',
          },
        }}
      >
        <Toolbar />
        <Box sx={{ overflow: 'auto' }}>
          <List>
            {menuItems.map((item) => (
              <ListItem key={item.text} disablePadding>
                <ListItemButton
                  selected={location.pathname === item.path}
                  onClick={() => navigate(item.path)}
                >
                  <ListItemIcon>{item.icon}</ListItemIcon>
                  <ListItemText primary={item.text} />
                </ListItemButton>
              </ListItem>
            ))}
          </List>
        </Box>
      </Drawer>
      <Box
        component="main"
        sx={{
          flexGrow: 1,
          bgcolor: 'background.default',
          p: 3,
          width: { sm: `calc(100% - ${drawerWidth}px)` },
        }}
      >
        <Toolbar />
        <Container maxWidth="xl">
          <Routes>
            <Route path="stores" element={<Stores onStoreSelect={setSelectedStore} />} />
            <Route path="products" element={<Products selectedStore={selectedStore} />} />
            <Route path="orders" element={<Orders selectedStore={selectedStore} />} />
            <Route path="payment-methods" element={<PaymentMethods selectedStore={selectedStore} />} />
          </Routes>
        </Container>
      </Box>
    </Box>
  )
}

export default Dashboard

