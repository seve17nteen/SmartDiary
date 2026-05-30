import React from 'react';
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import { AuthProvider } from './context/AuthContext';
import PrivateRoute from './components/PrivateRoute';
import PublicRoute from './components/PublicRoute';
import LoginForm from './components/LoginForm';
import RegisterForm from './components/RegisterForm';
import Navbar from './components/Navbar';
import TaskList from './components/TaskList';

function App() {
  return (
    <BrowserRouter>
      <AuthProvider>
        <Navbar />
        <Routes>
          <Route path="/login" element={
            <PublicRoute>
              <LoginForm />
            </PublicRoute>
          } />
          <Route path="/register" element={
            <PublicRoute>
              <RegisterForm />
            </PublicRoute>
          } />
          <Route path="/tasks" element={
            <PrivateRoute>
              <TaskList />
            </PrivateRoute>
          } />
          <Route path="/" element={
            <PrivateRoute>
              <TaskList />
            </PrivateRoute>
          } />
        </Routes>
      </AuthProvider>
    </BrowserRouter>
  );
}

export default App;