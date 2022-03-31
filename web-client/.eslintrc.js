// eslint-disable-next-line unicorn/prefer-module
module.exports = {
  root: true,
  env: {
    node: true,
    'vue/setup-compiler-macros': true
  },
  extends: [
    'eslint:recommended',
    'plugin:prettier/recommended',
    'plugin:vue/vue3-essential',
    'plugin:vue/vue3-strongly-recommended',
    'plugin:vue/vue3-recommended',
    'plugin:sonarjs/recommended',
    'plugin:unicorn/recommended'
  ],
  parserOptions: {
    parser: '@babel/eslint-parser'
  },
  rules: {
    'no-console': process.env.NODE_ENV === 'production' ? 'warn' : 'off',
    'no-debugger': process.env.NODE_ENV === 'production' ? 'warn' : 'off',
    'unicorn/filename-case': [
      'error',
      {
        case: 'kebabCase'
      }
    ],
    'unicorn/prevent-abbreviations': [
      'error',
      {
        allowList: {
          props: true
        }
      }
    ]
  },
  overrides: [
    {
      files: ['*.vue'],
      rules: {
        'unicorn/filename-case': [
          'error',
          {
            case: 'pascalCase'
          }
        ]
      }
    }
  ]
};
