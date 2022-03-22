<script setup>
import { computed, reactive, ref, watch } from 'vue';
import { createToast } from 'mosha-vue-toastify';
import WebhooksService from '@/services/webhooks.service';
import { onError } from '@/utils/error-handling';

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

const createWebhook = async () => {
  validatePayloadUrl(payloadUrl.value);
  if (invalid.value) {
    return;
  }

  loading.value = true;
  const { close: closeToast } = createToast('Creating new webhook...', {
    position: 'top-center',
    showCloseButton: false,
    timeout: -1,
    transition: 'slide',
    type: 'info'
  });

  try {
    await WebhooksService.createWebhook(payloadUrl.value, isActive.value);
  } catch (error) {
    onError(error);
  } finally {
    closeToast();
    loading.value = false;
  }
};

const validatePayloadUrl = (value) => {
  validationErrors.payloadUrl = '';

  if (!value || !value.trim()) {
    validationErrors.payloadUrl = 'Payload Url is required';
  }
};

const onSubmit = () => {
  // Do nothing
};
</script>

<template>
  <div class="row">
    <div class="col-12">
      <h4>Create Webhook</h4>
    </div>
    <div class="col-12">
      <form class="row g-2" @submit.prevent="onSubmit">
        <div class="col-12">
          <label for="payloadUrl" class="form-label">Payload Url</label>
          <input
            id="payloadUrl"
            v-model="payloadUrl"
            type="text"
            class="form-control"
            aria-describedby="payloadUrlInvalidFeedback"
            :class="{
              'is-invalid': validationErrors.payloadUrl
            }"
          />
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
            />
            <label for="isActive" class="form-check-label">Active</label>
          </div>
        </div>
        <div class="col-12 pt-2">
          <button
            :disabled="createButtonDisabled"
            type="submit"
            class="btn btn-primary"
            @click="createWebhook"
          >
            Create
          </button>
        </div>
      </form>
    </div>
  </div>
</template>
