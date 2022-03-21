import { HubConnectionBuilder, LogLevel } from '@microsoft/signalr';

const connection = new HubConnectionBuilder()
  .withUrl(process.env.VUE_APP_NOTIFICATION_HUB_URL)
  .configureLogging(LogLevel.Information)
  .build();

const start = () => {
  connection.start().catch((err) => {
    console.error('Failed to connect to notification hub', err);
  });
};

export { connection, start };
