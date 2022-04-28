import axios, { AxiosInstance } from 'axios';

const axiosInstance: AxiosInstance = axios.create({
  baseURL: process.env.VUE_APP_WEBHOOKS_SENDER_API_BASE_URL,
  headers: {
    'Content-type': 'application/json'
  }
});

export default axiosInstance;
