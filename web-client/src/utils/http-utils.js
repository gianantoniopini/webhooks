import axios from 'axios';

const webhooksSenderAxiosInstance = axios.create({
  baseURL: process.env.VUE_APP_WEBHOOKS_SENDER_URL,
  headers: {
    'Content-type': 'application/json'
  }
});

export { webhooksSenderAxiosInstance };
