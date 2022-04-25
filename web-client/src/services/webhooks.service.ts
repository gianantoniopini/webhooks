import axios, { AxiosResponse } from 'axios';
import CreateWebhookRequest from '@/interfaces/create-webhook-request.interface';
import Webhook from '@/interfaces/webhook.interface';

const axiosInstance = axios.create({
  baseURL: process.env.VUE_APP_WEBHOOKS_SENDER_API_BASE_URL,
  headers: {
    'Content-type': 'application/json'
  }
});

const getWebhooks = async (): Promise<Webhook[]> => {
  const { data } = await axiosInstance.get<Webhook[]>('/Webhooks');

  return data;
};

const createWebhook = async (
  payloadUrl: string,
  isActive: boolean
): Promise<Webhook> => {
  const request: CreateWebhookRequest = { payloadUrl, isActive };

  const { data: webhook } = await axiosInstance.post<
    CreateWebhookRequest,
    AxiosResponse<Webhook>
  >('/Webhooks', request);

  return webhook;
};

const sendWebhooks = async () => {
  await axiosInstance.post('/Webhooks/Send');
};

export { getWebhooks, createWebhook, sendWebhooks };
