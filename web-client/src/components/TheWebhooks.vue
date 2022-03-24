<script setup>
import CreateWebhook from '@/components/CreateWebhook.vue';
import SendWebhooks from '@/components/SendWebhooks.vue';
import WebhooksList from '@/components/WebhooksList.vue';
</script>

<template>
  <div class="row">
    <div class="col-12 text-center">
      <h1>Webhooks</h1>
    </div>
  </div>
  <div class="row">
    <div class="col-12">
      <hr />
    </div>
  </div>
  <CreateWebhook />
  <div class="row">
    <div class="col-12">
      <hr />
    </div>
  </div>
  <SendWebhooks />
  <div class="row">
    <div class="col-12">
      <hr />
    </div>
  </div>
  <WebhooksList />
</template>
