import { render, screen, waitFor } from '@testing-library/vue';
import AxiosMockAdapter from 'axios-mock-adapter';
import { StatusCodes } from 'http-status-codes';
import { connection } from '@/hubs/notification-hub';
import { Webhook } from '@/types';
import TheWebhooks from '@/components/TheWebhooks.vue';

export const mockNotificationHubConnectionStart = (): void => {
  connection.start = jest.fn(() => Promise.resolve());
};

export const createWebhooks = (count: number): Webhook[] => {
  const now = new Date();
  const webhooks: Webhook[] = [];

  const indexes = [...Array.from({ length: count }).keys()].map(
    (index) => index
  );
  for (const index of indexes) {
    const createdAt = new Date(now.getTime() - 15 * index * 60_000);

    const webhook = {
      id: index + 1,
      payloadUrl: `https://url${index}.com`,
      isActive: true,
      createdAt
    } as Webhook;

    webhooks.push(webhook);
  }

  return webhooks;
};

export const mockGetWebhooksRequest = (
  axiosMockAdapter: AxiosMockAdapter,
  webhooks: Webhook[],
  networkError?: boolean
): void => {
  const url = '/Webhooks';

  if (networkError) {
    axiosMockAdapter.onGet(url).networkError();
    return;
  }

  axiosMockAdapter.onGet(url).reply(StatusCodes.OK, webhooks);
};

export const renderComponent = () => {
  render(TheWebhooks);
};

const loadingMessage = 'Loading data, please wait...';

export const waitForLoadingMessageToAppear = async (): Promise<HTMLElement> => {
  return await screen.findByText(loadingMessage);
};

export const waitForLoadingMessageToDisappear = async (): Promise<void> => {
  await waitFor(
    () => {
      return screen.queryByText(loadingMessage) === null
        ? Promise.resolve()
        : Promise.reject();
    },
    { timeout: 1000 }
  );
};
