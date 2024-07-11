export const baseRemarkLength = 150
export const moneyMax = 99999999
export const recordNameLength = 20
export const constSystemInput = 'systeminput'
export const constNewZyFlag = 'zcyai.com'

export enum LayoutMode {
  LTR = 'ltr',
  LCR = 'lcr',
  TTB = 'ttb',
}

export enum DeviceType {
  PC = 'pc',
  PAD = 'pad',
  MOBILE = 'mobile',
}

export enum ThemeMode {
  LIGHT = 'light',
  DARK = 'dark',
}

export enum SideTheme {
  DARK = 'dark',
  WHITE = 'white',
  BLUE = 'blue',
  IMAGE = 'image',
}

export enum PageAnim {
  FADE = 'fade',
  OPACITY = 'opacity',
  DOWN = 'down',
  SCALE = 'scale',
}

export interface UserState {
  userId: string
  companyId: string
  accessToken: string
  refreshToken: string
  userNo: string
  userNick: string
  baseSettlement: number
  avatar: string
  isSuperAdmin: boolean
}

export enum BooleanEnum {
  是 = 'true',
  否 = 'false',
}

//2024------------------mes
//状态
export enum PublicStatusEnum {
  正常 = 1,
  待处理 = 2,
  禁用 = 5,
  驳回 = 6,
}

//系统角色名
export enum SysRoleNameEnum {
  正常 = 'normal',
  超管 = 'root',
  管理员 = 'admin',
  老板 = 'boss',
}

//收支类型
export enum IncomeTypeEnum {
  进账 = 1,
  出账 = 5,
}

//客户类型
export enum ClientTypeEnum {
  供应商 = 1,
  客户 = 5,
}

//账户类型
export enum AccountTypeEnum {
  支付宝 = 1,
  微信 = 2,
  扫码支付 = 3,
  银行卡 = 4,
  其他 = 100,
}

//工艺类型
export enum CraftTypeEnum {
  独立工艺 = 1,
  产品工艺 = 2,
}

//计费类型
export enum BillingTypeEnum {
  计时 = 1,
  计件 = 2,
}

//产品类型
export enum ProductTypeEnum {
  通用件 = 1,
  原料 = 2,
  加工产品 = 3,
  销售产品 = 4,
}

//用户状态
export enum UserStatusEnum {
  正常 = 1,
  离职 = 10,
  禁用 = 11,
}

//销售订单状态
export enum SaleOrderStatusEnum {
  待付款 = 5,
  已完成 = 100,
}
