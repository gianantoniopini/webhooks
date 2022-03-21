const { defineConfig } = require('@vue/cli-service');

module.exports = defineConfig({
  devServer: {
    port: process.env.VUE_APP_DEV_SERVER_PORT
  },
  transpileDependencies: true,
  lintOnSave: false
});
