import axios from 'axios';

const API_URL = 'https://localhost:7224/api';

const authService = {
  async register(username, email, password, password2) {
    const response = await axios.post(`${API_URL}/auth/register`, {
      username,
      email,
      password,
      password2
    });
    return response.data;
  },

  async login(username, password) {
    const response = await axios.post(`${API_URL}/auth/login`, {
      username,
      password
    });
    if (response.data.accessToken) {
      localStorage.setItem('access_token', response.data.accessToken);
      localStorage.setItem('user', JSON.stringify({ username: response.data.username }));
    }
    return response.data;
  },

  async refreshToken() {
    const token = localStorage.getItem('access_token');
    if (!token) return null;
    
    try {
      const response = await axios.post(`${API_URL}/auth/refresh`, {
        accessToken: token
      });
      if (response.data.accessToken) {
        localStorage.setItem('access_token', response.data.accessToken);
      }
      return response.data;
    } catch (error) {
      this.logout();
      return null;
    }
  },

  logout() {
    localStorage.removeItem('access_token');
    localStorage.removeItem('user');
  },

  getCurrentUser() {
    const userStr = localStorage.getItem('user');
    if (userStr) {
      return JSON.parse(userStr);
    }
    return null;
  },

  isAuthenticated() {
    return !!localStorage.getItem('access_token');
  }
};

export default authService;