<script setup lang="ts">
import { onMounted, reactive, ref, watch, PropType } from 'vue';
import { getWebhooks } from '@/services/webhooks.service';
import { handleError } from '@/utils/error-handling';
import { Webhook } from '@/types';

const props = defineProps({
  newWebhook: {
    type: Object as PropType<Webhook>,
    default() {
      return new Webhook();
    },
    required: false
  }
});

const loading = ref(false);
const webhooks: { data: Webhook[] } = reactive({
  data: []
});

const refresh = async () => {
  loading.value = true;

  try {
    webhooks.data = await getWebhooks();
  } catch (error) {
    handleError(error);
  } finally {
    loading.value = false;
  }
};

const formatDate = (date: Date) => {
  return new Date(date).toUTCString();
};

watch(props.newWebhook, () => {
  refresh();
});

onMounted(() => {
  refresh();
});
</script>

<!-- prettier-ignore -->
<template>
  <div class="row">
    <div class="col-12">
      <h2>Registered Webhooks</h2>
    </div>
    <div class="col-12">
      <button
        :disabled="loading"
        type="button"
        class="btn btn-secondary"
        @click="refresh"
      >
        Refresh
      </button>
    </div>
    <div class="col-12 pt-2">
      <div v-if="loading" class="text-center fw-light py-4">Loading data, please wait...</div>
      <table v-else class="table table-responsive">
        <thead class="table-light">
          <tr>
            <th scope="col">
              Id
            </th>
            <th scope="col">
              Payload Url
            </th>
            <th scope="col">
              Status
            </th>
            <th
              scope="col"
              class="text-end"
            >
              Created At
            </th>
          </tr>
        </thead>
        <tbody>
          <tr
            v-for="(webhook, index) in webhooks.data"
            :key="index"
          >
            <td>{{ webhook.id }}</td>
            <td>{{ webhook.payloadUrl }}</td>
            <td>{{ webhook.isActive ? 'Active' : 'Disabled' }}</td>
            <td class="text-end">
              {{ formatDate(webhook.createdAt) }}
            </td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</template>
