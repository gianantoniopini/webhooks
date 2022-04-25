export default interface Webhook {
  id: number;
  payloadUrl: string;
  isActive: boolean;
  createdAt: Date;
}
