import axios from 'axios';

const axiosInstance = axios.create({
  baseURL: process.env.VUE_APP_WEBHOOKS_SENDER_API_BASE_URL,
  headers: {
    'Content-type': 'application/json'
  }
});

const getWebhooks = async () => {
  const { data } = await axiosInstance.get('/Webhooks');

  return data;
};

const createWebhook = async (payloadUrl, isActive) => {
  const request = { payloadUrl, isActive };

  const { data: webhook } = await axiosInstance.post('/Webhooks', request);

  return webhook;
};

const sendWebhooks = async () => {
  await axiosInstance.post('/Webhooks/Send');
};

export { getWebhooks, createWebhook, sendWebhooks };
