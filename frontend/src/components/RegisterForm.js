import React, { useState } from 'react';
import { useFormik } from 'formik';
import * as Yup from 'yup';
import { useAuth } from '../context/AuthContext';
import { useNavigate, Link } from 'react-router-dom';
import {
  TextField,
  Button,
  Container,
  Typography,
  Box,
  Alert,
  Paper
} from '@mui/material';

const validationSchema = Yup.object({
  username: Yup.string()
    .min(3, 'Имя пользователя должно содержать минимум 3 символа')
    .max(100, 'Имя пользователя не может превышать 100 символов')
    .required('Имя пользователя обязательно'),
  email: Yup.string()
    .email('Некорректный формат email')
    .required('Email обязателен'),
  password: Yup.string()
    .min(6, 'Пароль должен содержать минимум 6 символов')
    .required('Пароль обязателен'),
  password2: Yup.string()
    .oneOf([Yup.ref('password')], 'Пароли не совпадают')
    .required('Подтверждение пароля обязательно')
});

const RegisterForm = () => {
  const { register } = useAuth();
  const navigate = useNavigate();
  const [error, setError] = useState('');

  const formik = useFormik({
    initialValues: {
      username: '',
      email: '',
      password: '',
      password2: ''
    },
    validationSchema: validationSchema,
    onSubmit: async (values) => {
      try {
        const result = await register(
          values.username,
          values.email,
          values.password,
          values.password2
        );
        if (result && result.success) {
          navigate('/tasks');
        } else {
          setError(result?.error || 'Ошибка регистрации');
        }
      } catch (err) {
        console.log('Ошибка:', err);
        setError(err?.response?.data?.message || 'Ошибка регистрации');
      }
    }
  });

  return (
    <Container maxWidth="sm">
      <Paper elevation={3} style={{ padding: '32px', marginTop: '64px' }}>
        <Typography variant="h4" component="h1" gutterBottom align="center">
          Регистрация
        </Typography>
        
        {error && (
          <Alert severity="error" style={{ marginBottom: '16px' }}>
            {error}
          </Alert>
        )}
        
        <form onSubmit={formik.handleSubmit}>
          <TextField
            fullWidth
            id="username"
            name="username"
            label="Имя пользователя"
            value={formik.values.username}
            onChange={formik.handleChange}
            error={formik.touched.username && Boolean(formik.errors.username)}
            helperText={formik.touched.username && formik.errors.username}
            margin="normal"
          />
          
          <TextField
            fullWidth
            id="email"
            name="email"
            label="Email"
            value={formik.values.email}
            onChange={formik.handleChange}
            error={formik.touched.email && Boolean(formik.errors.email)}
            helperText={formik.touched.email && formik.errors.email}
            margin="normal"
          />
          
          <TextField
            fullWidth
            id="password"
            name="password"
            label="Пароль"
            type="password"
            value={formik.values.password}
            onChange={formik.handleChange}
            error={formik.touched.password && Boolean(formik.errors.password)}
            helperText={formik.touched.password && formik.errors.password}
            margin="normal"
          />
          
          <TextField
            fullWidth
            id="password2"
            name="password2"
            label="Подтверждение пароля"
            type="password"
            value={formik.values.password2}
            onChange={formik.handleChange}
            error={formik.touched.password2 && Boolean(formik.errors.password2)}
            helperText={formik.touched.password2 && formik.errors.password2}
            margin="normal"
          />
          
          <Button
            type="submit"
            fullWidth
            variant="contained"
            color="primary"
            disabled={formik.isSubmitting}
            style={{ marginTop: '24px', marginBottom: '16px' }}
          >
            ЗАРЕГИСТРИРОВАТЬСЯ
          </Button>
          
          <Box textAlign="center">
            <Link to="/login">
              <Typography variant="body2">
                Уже есть аккаунт? Войти
              </Typography>
            </Link>
          </Box>
        </form>
      </Paper>
    </Container>
  );
};

export default RegisterForm;