import React, { createContext, useState, useContext, useEffect } from 'react';
import authService from '../services/authService';

const AuthContext = createContext();

export const useAuth = () => useContext(AuthContext);

export const AuthProvider = ({ children }) => {
  const [user, setUser] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const currentUser = authService.getCurrentUser();
    const isAuthenticated = authService.isAuthenticated();
    
    if (currentUser && isAuthenticated) {
      setUser(currentUser);
    }
    setLoading(false);
  }, []);

  const register = async (username, email, password, password2) => {
    try {
      await authService.register(username, email, password, password2);
      return await login(username, password);
    } catch (error) {
      throw error;
    }
  };

  const login = async (username, password) => {
    try {
      const data = await authService.login(username, password);
      if (data.accessToken) {
        setUser({ username: data.username });
        return { success: true };
      }
      return { success: false, error: 'Ошибка входа' };
    } catch (error) {
      return {
        success: false,
        error: error.response?.data?.message || 'Ошибка входа'
      };
    }
  };

  const logout = () => {
    authService.logout();
    setUser(null);
  };

  const value = {
    user,
    loading,
    register,
    login,
    logout,
    isAuthenticated: !!user && authService.isAuthenticated()
  };

  return (
    <AuthContext.Provider value={value}>
      {children}
    </AuthContext.Provider>
  );
};