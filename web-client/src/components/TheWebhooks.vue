<script setup lang="ts">
import { reactive } from 'vue';
import CreateWebhook from '@/components/CreateWebhook.vue';
import SendWebhooks from '@/components/SendWebhooks.vue';
import WebhooksList from '@/components/WebhooksList.vue';
import Webhook from '@/interfaces/webhook.interface';

const newWebhook: Webhook = reactive({
  id: 0,
  payloadUrl: '',
  isActive: false,
  createdAt: new Date()
});

const onWebhookCreated = (webhook: Webhook) => {
  newWebhook.id = webhook.id;
  newWebhook.payloadUrl = webhook.payloadUrl;
  newWebhook.isActive = webhook.isActive;
  newWebhook.createdAt = webhook.createdAt;
};
</script>

<!-- prettier-ignore -->
<template>
  <div class="row">
    <div class="col-12 text-center">
      <h1>Webhooks</h1>
    </div>
  </div>
  <div class="row">
    <div class="col-12">
      <hr>
    </div>
  </div>
  <CreateWebhook @webhook-created="onWebhookCreated" />
  <div class="row">
    <div class="col-12">
      <hr>
    </div>
  </div>
  <SendWebhooks />
  <div class="row">
    <div class="col-12">
      <hr>
    </div>
  </div>
  <WebhooksList :new-webhook="newWebhook" />
</template>
