import { render, screen } from '@testing-library/vue';
import App from '../App.vue';
import router from '@/router';

const setup = async () => {
  render(App, {
    global: {
      plugins: [router]
    }
  });

  await router.isReady();
};

it('renders Webhooks heading', async () => {
  await setup();

  expect(
    screen.queryByRole('heading', {
      name: 'Webhooks'
    })
  ).toBeInTheDocument();
});
