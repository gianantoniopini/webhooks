<script setup>
import { onBeforeMount, onBeforeUnmount } from 'vue';
import { createToast } from 'mosha-vue-toastify';
import { connection, start } from '@/hubs/notification-hub';

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
  <h1>Webhooks</h1>
</template>
