import { render, screen } from '@testing-library/vue';
import AxiosMockAdapter from 'axios-mock-adapter';
import axiosInstance from '@/utils/http-utils';
import {
  mockNotificationHubConnectionStart,
  mockGetWebhooksRequest,
  waitForLoadingMessageToAppear,
  waitForLoadingMessageToDisappear
} from '@/components/__tests__/helpers/TheWebhooks.Helper';
import App from '../App.vue';
import router from '@/router';

const axiosMockAdapter = new AxiosMockAdapter(axiosInstance, {
  delayResponse: 500
});

const setup = async () => {
  mockNotificationHubConnectionStart();
  mockGetWebhooksRequest(axiosMockAdapter, []);

  render(App, {
    global: {
      plugins: [router]
    }
  });

  await router.isReady();

  await waitForLoadingMessageToAppear();
  await waitForLoadingMessageToDisappear();
};

it('renders Webhooks heading', async () => {
  await setup();

  expect(
    screen.queryByRole('heading', {
      name: 'Webhooks'
    })
  ).toBeInTheDocument();
});

afterEach(async () => {
  axiosMockAdapter.reset();
  jest.restoreAllMocks();
});
