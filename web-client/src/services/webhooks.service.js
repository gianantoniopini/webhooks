import { webhooksSenderAxiosInstance } from '@/utils/http-utils';

class WebhooksService {
  async getWebhooks() {
    const { data } = await webhooksSenderAxiosInstance.get('/Webhooks');

    return data;
  }

  async createWebhook(payloadUrl, isActive) {
    const webhook = { id: 0, payloadUrl, isActive };
    await webhooksSenderAxiosInstance.post('/Webhooks', webhook);
  }
}

export default new WebhooksService();
