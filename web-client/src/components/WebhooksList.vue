<script setup>
import { onMounted, reactive, ref, watch } from 'vue';
import { getWebhooks } from '@/services/webhooks.service';
import { handleError } from '@/utils/error-handling';

const props = defineProps(['newWebhook']);

const loading = ref(false);
const webhooks = reactive({
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

watch(props.newWebhook, () => {
  refresh();
});

onMounted(() => {
  refresh();
});
</script>

<template>
  <div class="row">
    <div class="col-12">
      <h4>Registered Webhooks</h4>
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
      <table class="table table-responsive">
        <thead class="table-light">
          <tr>
            <th scope="col">#</th>
            <th scope="col">Id</th>
            <th scope="col">Payload Url</th>
            <th scope="col" class="text-center">Active</th>
            <th scope="col" class="text-end">Created At</th>
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
            <td class="text-end">{{ webhook.createdAt }}</td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</template>
