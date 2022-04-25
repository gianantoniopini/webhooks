<script setup lang="ts">
import { computed, reactive, ref, watch } from 'vue';
import { createWebhook } from '@/services/webhooks.service';
import { handleError } from '@/utils/error-handling';
import Webhook from '@/interfaces/webhook.interface';

const loading = ref(false);
const payloadUrl = ref('');
const isActive = ref(true);
const validationErrors = reactive({
  payloadUrl: ''
});

watch(payloadUrl, (value) => {
  validatePayloadUrl(value);
});

const invalid = computed(() => validationErrors.payloadUrl.length > 0);
const createButtonDisabled = computed(() => loading.value || invalid.value);

const emit = defineEmits<{
  (eventName: 'webhookCreated', value: Webhook): void;
}>();

const onCreate = async () => {
  validatePayloadUrl(payloadUrl.value);
  if (invalid.value) {
    return;
  }

  loading.value = true;

  try {
    const webhook = await createWebhook(payloadUrl.value, isActive.value);

    emit('webhookCreated', webhook);
  } catch (error) {
    handleError(error);
  } finally {
    loading.value = false;
  }
};

const validatePayloadUrl = (value: string) => {
  validationErrors.payloadUrl = '';

  if (!value || !value.trim()) {
    validationErrors.payloadUrl = 'Payload Url is required';
  }
};

const onSubmit = () => {
  // Do nothing
};
</script>

<!-- prettier-ignore -->
<template>
  <div class="row">
    <div class="col-12">
      <h2>Create Webhook</h2>
    </div>
    <div class="col-12">
      <form
        class="row g-3"
        @submit.prevent="onSubmit"
      >
        <div class="col-12">
          <label
            for="payloadUrl"
            class="form-label"
          >Payload Url</label>
          <input
            id="payloadUrl"
            v-model="payloadUrl"
            type="text"
            class="form-control"
            aria-describedby="payloadUrlInvalidFeedback"
            :class="{
              'is-invalid': validationErrors.payloadUrl
            }"
          >
          <div
            id="payloadUrlInvalidFeedback"
            class="invalid-feedback"
            role="alert"
          >
            {{ validationErrors.payloadUrl }}
          </div>
        </div>
        <div class="col-12">
          <div class="form-check">
            <input
              id="isActive"
              v-model="isActive"
              type="checkbox"
              class="form-check-input"
            >
            <label
              for="isActive"
              class="form-check-label"
            >Active</label>
          </div>
        </div>
        <div class="col-12">
          <button
            :disabled="createButtonDisabled"
            type="submit"
            class="btn btn-primary"
            @click="onCreate"
          >
            Create
          </button>
        </div>
      </form>
    </div>
  </div>
</template>
