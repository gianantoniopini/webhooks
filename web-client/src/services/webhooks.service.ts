import { AxiosResponse } from 'axios';
import axiosInstance from '@/utils/http-utils';
import { CreateWebhookRequest, Webhook } from '@/types';

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
