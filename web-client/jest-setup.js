import '@testing-library/jest-dom';

// 1. environment variables
process.env.VUE_APP_NOTIFICATION_HUB_URL =
  'http://localhost:6001/notificationhub';
process.env.VUE_APP_WEBHOOKS_SENDER_API_BASE_URL = 'http://localhost:5001/api';
