# 2.2.2

修复

1. 修复`picker`值选定残留问题

# 2.2.1

优化

1. 新增`getMenuList`方法，获取菜单数据【示例6】
2. 新增`getMenuIndex`方法，获取指定`prop`的菜单索引位置
3. 优化示例使用方法
4. 修复互斥、联动的值处理问题
5. 更新了说明文档

# 2.2.0

推荐更新

1. 新增更多的示例，示例更全面丰富
2. 新增`openMenuItemPopup`方法，打开指定菜单弹窗【示例8】
3. 新增`closeMenuPopup`方法，关闭菜单弹窗【示例5】
4. 新增`getMenuValue`方法，获取菜单的值【示例8】
5. 新增`updateMenu`方法，更新指定菜单项【示例7】
6. 新增`setMenuLoading`方法，让某个菜单项处于加载中状态，异步数据有用【示例7】
7. 修复`fixedTopValue`及顶部样式错误【示例2】
8. 新增`syncDataKey`支持异步数据嵌套回调【示例7】
9. 新增异步数据加载状态【示例7】
10. 修复日期快捷获取上个月时间的错误
11. 修复手动关闭时存在的点击残留
12. 优化菜单项联动、互斥选择
13. 优化异步数据阻塞菜单回显问题
14. 修复数据回调时，会重新渲染菜单问题
15. `@close`参数2支持返回当前菜单列表(Array)
16. `@confirm`参数2支持返回当前菜单已选内容(Object)
17. 菜单项`daterange`支持`showQuick`控制是否显示日期快选
18. 移除`menuActiveText`，用处不大
19. 更新了说明文档

# 2.1.1

优化

1. 优化图标字体命名

# 2.1.0

推荐更新

1. 现阶段由于兼容性问题，移除动态插槽
2. 新增五个拓展插槽，`data`类型为 `slot1`/`slot2`/`slot3`/`slot4`/`slot5`
3. 修复倒三角点击蒙层没有复原
4. 新增更多演示示例
5. 优化非固定页面顶部的效果
6. 类型`cell`(下拉列表)数据项新增 `disabled` 用来禁用部分不可用选项
7. 修复小程序对`v-show`、主题色的兼容问题
8. 优化对 APP 的兼容

# 2.0.9

修复

1. 修复初始化数据时可能存在的选项高亮问题

# 2.0.8

优化

1. 优化拷贝函数引起的 App 兼容问题

# 2.0.7

优化

1. 优化模块图标支持主题换色
2. 新增`fixedTopValue`，用于优化异形屏导致搜索框被挡住问题，具体请参考示例项目

# 2.0.6

优化

1. 优化三角图标支持主题换色

# 2.0.5

修复

1. 修复 `picker` 大量数据时未能滚动问题

# 2.0.4

修复

1. 修复弹窗后点击 `click`、`sort` 类型未能收起弹窗

# 2.0.3

优化

1. 移除 scss 的 @use 用法，以修复可能会导致部分用户的 lint 错误

# 2.0.2

优化

1. 优化菜单样式

# 2.0.1

新增

1. 下拉列表`cell`、级联`picker`新增选中图标`showIcon`

# 2.0.0

移除 TS

1. 移除了 TS 写法，现在是纯粹 JS 版本的 Vue3 版本
2. 进一步完善使用文档及示例

# 1.2.1

新增功能

1. 新增顶部搜索，当 type 为 `slot` 时，可在弹窗内容自定义插槽
2. 新增自定插槽，当 type 为 `search` 时，头部显示搜索框
3. 优化界面样式

# 1.1.2

优化异步菜单项数据

1. 菜单项新增 `syncDataFn` 函数字段，返回异步菜单项数据内容

# 1.1.1

优化固定弹窗问题

1. `da-dropdown`新增 `bgColor` 字段，当固定在顶部时，需要填写背景颜色，默认`#fff`
2. 优化弹窗时底部滑动问题

# 1.1.0

优化数据问题

### 优化

1. `da-dropdown`新增 `prop` 字段，通过 prop 的唯一性来区分已选的数据
2. `da-dropdown`新增 `fixedTop` 字段，为 true 时将会固定在头部
3. 优化说明文档

# 1.0.0

初始版本 1.0.0，基于 Typescript+Scss 进行开发，基本完善相关各大平台的小程序兼容问题

### 新增

1. 下拉列表（单选）
2. 点击高亮
3. 点击排序
4. 下拉筛选（单选按钮、多选按钮、滑动选择器）
5. 级联筛选（单选）
6. 日期筛选（日期快选、日期区间选择）