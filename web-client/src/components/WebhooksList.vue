<script setup>
import { onMounted, reactive, ref } from 'vue';
import { createToast } from 'mosha-vue-toastify';
import WebhooksService from '@/services/webhooks.service';

const loading = ref(false);
const webhooks = reactive({
  data: []
});

const retrieveWebhooks = async () => {
  loading.value = true;
  const { close: closeToast } = createToast(
    'Retrieving registered webhooks...',
    {
      position: 'top-center',
      showCloseButton: false,
      timeout: -1,
      transition: 'slide',
      type: 'info'
    }
  );

  try {
    webhooks.data = await WebhooksService.getRegisteredWebhooks();
  } catch (error) {
    onError(error);
  } finally {
    closeToast();
    loading.value = false;
  }
};

const onError = (error) => {
  createToast('An unexpected error occurred. Please try again.', {
    position: 'top-center',
    showCloseButton: true,
    timeout: 4000,
    transition: 'slide',
    type: 'warning'
  });
  console.error(error);
};

const onSubmit = () => {
  // Do nothing
};

onMounted(() => {
  retrieveWebhooks();
});
</script>

<template>
  <div class="row">
    <div class="col-12">
      <h4>Registered Webhooks</h4>
      <hr />
    </div>
    <div class="col-12">
      <form class="row form" @submit.prevent="onSubmit">
        <div class="col-12">
          <button
            :disabled="loading"
            type="submit"
            class="btn btn-primary"
            @click="retrieveWebhooks"
          >
            Refresh
          </button>
        </div>
      </form>
    </div>
    <div class="col-12 pt-2">
      <div class="row border border-dark bg-light fw-bold">
        <div class="col-lg-5 border border-dark text-lg-start">Id</div>
        <div class="col-lg-4 border border-dark text-lg-start">Payload Url</div>
        <div class="col-lg-3 border border-dark text-lg-start">Is Active</div>
      </div>
      <div
        v-for="(webhook, index) in webhooks.data"
        :key="index"
        class="row border"
      >
        <div class="col-lg-5 border text-lg-start">
          {{ webhook.id }}
        </div>
        <div class="col-lg-4 border text-lg-start">
          {{ webhook.payloadUrl }}
        </div>
        <div class="col-lg-3 border text-lg-start">
          {{ webhook.isActive }}
        </div>
      </div>
    </div>
  </div>
</template>
