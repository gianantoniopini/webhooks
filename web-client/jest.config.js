// eslint-disable-next-line unicorn/prefer-module
module.exports = {
  clearMocks: true,
  collectCoverage: true,
  collectCoverageFrom: ['./src/**', '!**/assets/**', '!**/__tests__/**'],
  coverageDirectory: 'coverage',
  coverageThreshold: {
    global: {
      lines: 55
    }
  },
  preset: '@vue/cli-plugin-unit-jest',
  setupFilesAfterEnv: ['<rootDir>/jest-setup.js'],
  testMatch: ['**/__tests__/**/*.Test.js']
};
