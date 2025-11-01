import axios from 'axios';

const API_BASE_URL = import.meta.env.VITE_API_URL || 'https://localhost:7215/api';

const api = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

// Request interceptor for adding auth token
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

export const contentService = {
  getAll: (params) => api.get('/content', { params }),
  getById: (id) => api.get(`/content/${id}`),
  getBySlug: (slug) => api.get(`/content/slug/${slug}`),
  create: (data) => api.post('/content', data),
  update: (id, data) => api.put(`/content/${id}`, data),
  delete: (id) => api.delete(`/content/${id}`),
  enhance: (id, provider) => api.post(`/content/${id}/enhance?provider=${provider}`),
};

export const categoryService = {
  getAll: () => api.get('/category'),
  getById: (id) => api.get(`/category/${id}`),
  getBySlug: (slug) => api.get(`/category/slug/${slug}`),
};

export const commentService = {
  getByContentId: (contentId) => api.get(`/comment/content/${contentId}`),
  create: (data) => api.post('/comment', data),
  approve: (id) => api.put(`/comment/${id}/approve`),
  delete: (id) => api.delete(`/comment/${id}`),
};

export default api;
