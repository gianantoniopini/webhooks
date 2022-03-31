<script setup>
import { reactive } from 'vue';

import CreateWebhook from '@/components/CreateWebhook.vue';
import SendWebhooks from '@/components/SendWebhooks.vue';
import WebhooksList from '@/components/WebhooksList.vue';

const newWebhook = reactive({});

const onWebhookCreated = (webhook) => {
  newWebhook.value = webhook;
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
