<script setup>
import { onBeforeMount, onBeforeUnmount, ref } from 'vue';
import { connection, start } from '@/hubs/notification-hub';
import { sendWebhooks } from '@/services/webhooks.service';
import { handleError } from '@/utils/error-handling';
import { createToast } from 'mosha-vue-toastify';

const loading = ref(false);

const onSend = async () => {
  loading.value = true;

  try {
    await sendWebhooks();
  } catch (error) {
    handleError(error);
  } finally {
    loading.value = false;
  }
};

const onWebhookReceived = (webhookPayload) => {
  createToast(
    `Received webhook payload with message '${webhookPayload.message}'`,
    {
      position: 'top-center',
      showCloseButton: true,
      timeout: 4000,
      transition: 'slide',
      type: 'info'
    }
  );
};

onBeforeMount(() => {
  start();

  connection.on('webhookReceived', onWebhookReceived);
});

onBeforeUnmount(() => {
  connection.off('webhookReceived', onWebhookReceived);
});
</script>

<template>
  <div class="row">
    <div class="col-12">
      <h4>Send Webhooks</h4>
    </div>
    <div class="col-12">
      <button
        :disabled="loading"
        type="button"
        class="btn btn-primary"
        @click="onSend"
      >
        Send
      </button>
    </div>
  </div>
</template>
