import { fireEvent, screen, waitFor } from '@testing-library/vue';
import AxiosMockAdapter from 'axios-mock-adapter';
import axiosInstance from '@/utils/http-utils';
import {
  createWebhooks,
  mockGetWebhooksRequest,
  mockNotificationHubConnectionStart,
  renderComponent,
  waitForRefreshButtonToBeDisabled,
  waitForRefreshButtonToBeEnabled
} from './helpers/TheWebhooks.Helper';

const axiosMockAdapter = new AxiosMockAdapter(axiosInstance, {
  delayResponse: 500
});

const setup = async (
  webhooksCount: number,
  apiRequestNetworkError?: boolean
) => {
  mockNotificationHubConnectionStart();

  const webhooks = createWebhooks(webhooksCount);
  mockGetWebhooksRequest(axiosMockAdapter, webhooks, apiRequestNetworkError);

  renderComponent();

  await waitForRefreshButtonToBeDisabled();
  await waitForRefreshButtonToBeEnabled();

  const refreshButton = screen.getByRole('button', {
    name: 'Refresh'
  });

  return {
    webhooks,
    refreshButton
  };
};

describe('clicking the Refresh button', () => {
  it('disables button while loading', async () => {
    const { refreshButton } = await setup(10);

    await fireEvent.click(refreshButton);

    await waitFor(() => {
      expect(refreshButton).toBeDisabled();
    });
    await waitFor(() => {
      expect(refreshButton).toBeEnabled();
    });
  });

  it('renders Error message if api request fails', async () => {
    jest.spyOn(console, 'error').mockImplementationOnce(() => {
      // do nothing
    });
    const { refreshButton } = await setup(0, true);

    await fireEvent.click(refreshButton);

    const errorMessage = await screen.findByText(
      'An unexpected error occurred. Please try again.'
    );
    expect(errorMessage).toBeInTheDocument();
  });
});

afterEach(() => {
  axiosMockAdapter.reset();
  jest.restoreAllMocks();
});
