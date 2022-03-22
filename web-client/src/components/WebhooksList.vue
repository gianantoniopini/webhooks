<script setup>
import { onMounted, reactive, ref } from 'vue';
import { createToast } from 'mosha-vue-toastify';
import WebhooksService from '@/services/webhooks.service';
import { onError } from '@/utils/error-handling';

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
    webhooks.data = await WebhooksService.getWebhooks();
  } catch (error) {
    onError(error);
  } finally {
    closeToast();
    loading.value = false;
  }
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
      <table class="table table-responsive">
        <thead class="table-light">
          <tr>
            <th scope="col">#</th>
            <th scope="col">Id</th>
            <th scope="col">Payload Url</th>
            <th scope="col" class="text-center">Active</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="(webhook, index) in webhooks.data" :key="index">
            <th scope="row">{{ index + 1 }}</th>
            <td>{{ webhook.id }}</td>
            <td>{{ webhook.payloadUrl }}</td>
            <td class="text-center">
              <input
                v-model="webhook.isActive"
                type="checkbox"
                class="form-check-input"
                disabled
              />
            </td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</template>
