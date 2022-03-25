# Webhooks

Small application to create and send webhooks.

## Requirements

- [.NET 5.0 SDK][1]
- [Node.js][2]
- [npm][3]
- [http-server][4] (only required to start the production version of the web-client)

## Setup

### 1. Webhooks.Receiver setup

#### 1.1 Switch into the Webhooks.Receiver directory

```sh
cd web-api/Webhooks.Receiver
```

#### 1.2 Build and run

```sh
dotnet run
```

### 2. Webhooks.Sender setup

#### 2.1 Switch into the Webhooks.Sender directory

```sh
cd web-api/Webhooks.Sender
```

#### 2.2 Build and run

```sh
dotnet run
```

### 3. web-client setup

#### 3.1 Switch into the web-client directory

```sh
cd web-client
```

#### 3.2 Install NPM packages

```sh
npm install
```

#### 3.3 Compiles and hot-reloads for development

```sh
npm run serve
```

#### 3.4 Compiles and minifies for production

```sh
npm run build
```

#### 3.5 Starts production

```sh
npm run serve:production
```

<!-- MARKDOWN LINKS -->

[1]: https://dotnet.microsoft.com/en-us/download/dotnet/5.0
[2]: https://nodejs.org/en/download/current/
[3]: https://nodejs.org/en/download/current/
[4]: https://www.npmjs.com/package/http-server
