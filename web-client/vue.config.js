// eslint-disable-next-line unicorn/prefer-module
const { defineConfig } = require('@vue/cli-service');
// eslint-disable-next-line unicorn/prefer-module
const zlib = require('zlib');

// eslint-disable-next-line unicorn/prefer-module
module.exports = defineConfig({
  devServer: {
    port: process.env.VUE_APP_DEV_SERVER_PORT
  },
  transpileDependencies: true,
  lintOnSave: false,
  pages: {
    index: {
      entry: 'src/main.js',
      title: 'Webhooks'
    }
  },
  pluginOptions: {
    compression: {
      brotli: {
        filename: '[file].br[query]',
        algorithm: 'brotliCompress',
        include: /\.(js|css|html|svg|json)(\?.*)?$/i,
        compressionOptions: {
          params: {
            [zlib.constants.BROTLI_PARAM_QUALITY]: 11
          }
        },
        minRatio: 0.8
      },
      gzip: {
        filename: '[file].gz[query]',
        algorithm: 'gzip',
        include: /\.(js|css|html|svg|json)(\?.*)?$/i,
        minRatio: 0.8
      }
    }
  }
});
