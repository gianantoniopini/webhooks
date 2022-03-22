import { createToast } from 'mosha-vue-toastify';

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

export { onError };
