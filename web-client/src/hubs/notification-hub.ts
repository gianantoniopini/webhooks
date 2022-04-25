import { HubConnectionBuilder, LogLevel } from '@microsoft/signalr';

const connection = new HubConnectionBuilder()
  .withUrl(process.env.VUE_APP_NOTIFICATION_HUB_URL)
  .configureLogging(LogLevel.Warning)
  .build();

const start = () => {
  connection.start().catch((error) => {
    console.error('Failed to connect to notification hub', error);
  });
};

export { connection, start };
