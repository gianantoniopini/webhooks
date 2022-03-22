import { webhooksSenderAxiosInstance } from '@/utils/http-utils';

class WebhooksService {
  async getRegisteredWebhooks() {
    const { data } = await webhooksSenderAxiosInstance.get('/Webhooks');

    return data;
  }
}

export default new WebhooksService();
