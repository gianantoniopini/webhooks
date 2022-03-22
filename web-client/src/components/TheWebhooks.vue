<script setup>
import { onBeforeMount, onBeforeUnmount } from 'vue';
import { createToast } from 'mosha-vue-toastify';
import { connection, start } from '@/hubs/notification-hub';
import CreateWebhook from '@/components/CreateWebhook.vue';
import WebhooksList from '@/components/WebhooksList.vue';

const onEmailSent = (email) => {
  createToast(`Email sent to ${email.to}`, {
    position: 'top-center',
    showCloseButton: true,
    timeout: 4000,
    transition: 'slide',
    type: 'info'
  });
};

onBeforeMount(() => {
  start();

  connection.on('emailSent', onEmailSent);
});

onBeforeUnmount(() => {
  connection.off('emailSent', onEmailSent);
});
</script>

<template>
  <div class="row">
    <div class="col-12">
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
  <WebhooksList />
</template>
