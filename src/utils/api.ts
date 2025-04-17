// Import axios for making HTTP requests
import axios from 'axios';

// Create an Axios instance for API requests
const api = axios.create({
  baseURL: `${import.meta.env.VITE_API_BASE_URL}/api`, // ✅ THIS will dynamically load from .env
  headers: {
    'Content-Type': 'application/json',
  },
});

// Add a request interceptor to include the token in every request
api.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem('token');
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

// Export the Axios instance for use in the application
export default api;

/**
 * Summary:
 * This Axios instance is configured to:
 * - Use the secure backend base URL (`https://localhost:7065/api`)
 * - Send JSON by default (`Content-Type: application/json`)
 * - Automatically include the user's JWT (if stored in localStorage) in the `Authorization` header
 */
