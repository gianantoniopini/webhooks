import { screen } from '@testing-library/vue';
import AxiosMockAdapter from 'axios-mock-adapter';
import axiosInstance from '@/utils/http-utils';
import {
  createWebhooks,
  mockGetWebhooksRequest,
  mockNotificationHubConnectionStart,
  renderComponent
} from './helpers/TheWebhooks.Helper';

const axiosMockAdapter = new AxiosMockAdapter(axiosInstance, {
  delayResponse: 500
});

const setup = (webhooksCount: number) => {
  mockNotificationHubConnectionStart();

  const webhooks = createWebhooks(webhooksCount);
  mockGetWebhooksRequest(axiosMockAdapter, webhooks);

  renderComponent();

  return {
    webhooks
  };
};

it('renders Registered Webhooks', async () => {
  const webhooksCount = 10;
  const { webhooks } = setup(webhooksCount);

  expect(webhooks).toHaveLength(webhooksCount);
  for (const webhook of webhooks) {
    const id = await screen.findByText(webhook.id);
    expect(id).toBeInTheDocument();

    const payloadUrl = await screen.findByText(webhook.payloadUrl);
    expect(payloadUrl).toBeInTheDocument();

    const createdAt = await screen.findByText(webhook.createdAt.toUTCString());
    expect(createdAt).toBeInTheDocument();
  }
});

afterEach(async () => {
  axiosMockAdapter.reset();
  jest.restoreAllMocks();
});
