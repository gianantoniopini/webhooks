<script setup>
import { onMounted, ref } from 'vue';

const time = ref('');

onMounted(() => {
  const connection = new WebSocket('wss://localhost:5001/ws');

  connection.onopen = (event) => {
    console.info('Successfully connected to the websocket server...');
  };

  connection.onmessage = (event) => {
    console.log(event);
    time = event.data;
  };
});
</script>

<template>
  <h1>Webhooks</h1>
</template>
