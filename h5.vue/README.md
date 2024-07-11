## 项目简介

### 项目说明

ZcyMES：微信小程序端。

当前仓库是 **uni-app** 开发的**微信小程序端**，通过**条件编译**能兼容 **H5 端** 和 **App 端**。

注：暂未测试 **H5 端** 

<!-- ### 在线体验

<table>
  <tr>
    <td>体验小程序端</td>
    <td><a target="_blank" href="https://megasu.gitee.io/uniapp-shop-vue3-ts/">体验 H5 端</a></td>
    <td><a target="_blank" href="https://gitee.com/Megasu/uniapp-shop-vue3-ts/releases/download/v1.0.0/heima-shop.apk">体验 App 端(安卓)</a></td>
  </tr>
  <tr>
    <td><img width="300" src="./README/images/code-mp-weixin.png" alt=""></td>
    <td><img width="300" src="./README/images/code-h5.png" alt=""></td>
    <td><img width="300" src="./README/images/code-android.png" alt=""></td>
  </tr>
</table> -->

## 项目演示

### 在线演示

项目已上线，微信搜索小程序 **ZcyMES演示** 即可体验。

### 开发环境

- Windows 版本： Windows 11 家庭中文版
- 开发工具： VS Code 、 微信开发者工具
- Node 版本： v16.15.0
- pnpm 版本：v8.6.10

### 运行程序

1. 安装依赖

```shell
# npm
npm i --registry=https://registry.npmmirror.com

# pnpm
pnpm i --registry=https://registry.npmmirror.com
```

2. 运行程序

```shell
# 微信小程序端
npm run dev:mp-weixin

# H5端
npm run dev:h5

# App端
需 HbuilderX 工具，运行 - 运行到手机或模拟器
```

3. 微信开发者工具导入 `/dist/dev/mp-weixin` 目录
