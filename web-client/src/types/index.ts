export interface CreateWebhookRequest {
  payloadUrl: string;
  isActive: boolean;
}

export class Webhook {
  id: number;
  payloadUrl: string;
  isActive: boolean;
  createdAt: Date;

  constructor() {
    this.id = 0;
    this.payloadUrl = '';
    this.isActive = false;
    this.createdAt = new Date();
  }
}

export interface WebhookPayload {
  message: string;
}
