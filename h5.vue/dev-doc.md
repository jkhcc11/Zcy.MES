# 开发相关
1、安装依赖
`pnpm i --registry=https://registry.npmmirror.com`

- 插件 `uni-helper`、`uni-create-view`、`uniapp小程序扩展`、这个必须是v2.0.12版本 否则会有红字提示`Vue - Official v2.0.12`、`Prettier - Code formatter`

2、小程序|H5

```
# 微信小程序端
npm run dev:mp-weixin

# H5端
npm run dev:h5

# App端
需 HbuilderX 工具，运行 - 运行到手机或模拟器

```

3、开发相关

-  全局拦截 `utils/http.ts`
-  Api地址 `services` 文件夹
-  跳转

```
	<navigator url="navigate/navigate?title=navigate" hover-class="navigator-hover">
					<button type="default">跳转到新页面</button>
				</navigator>
				<navigator url="redirect/redirect?title=redirect" open-type="redirect" hover-class="other-navigator-hover">
					<button type="default">在当前页打开</button>
				</navigator>
				<navigator url="/pages/tabBar/extUI/extUI" open-type="switchTab" hover-class="other-navigator-hover">
					<button type="default">跳转tab页面</button>
				</navigator>
```
