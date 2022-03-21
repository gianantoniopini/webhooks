<script setup>
import { onBeforeMount, onBeforeUnmount } from 'vue';
import { connection, start } from '@/hubs/notification-hub';

const onEmailSent = (email) => {
  console.info('emailSent', email);
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
