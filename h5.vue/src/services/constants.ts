//分页大小
export const CommonPageSize = 10

//渠道
export enum AccountTypeEnum {
  支付宝 = 1,
  微信 = 2,
  扫码支付 = 3,
  银行卡 = 4,
  其他 = 100,
}

//订单状态
export enum SaleOrderStatusEnum {
  待付款 = 5,
  已完成 = 100,
}

//产品类型
export enum ProductTypeEnum {
  通用件 = 1,
  原料 = 2,
  加工产品 = 3,
  销售产品 = 4,
}

//客户类型
export enum ClientTypeEnum {
  供应商 = 1,
  客户 = 5,
}

//收支类型
export enum IncomeTypeEnum {
  进账 = 1,
  出账 = 5,
}

//通用状态
export enum PublicStatusEnum {
  正常 = 1,
  待处理 = 2,
  禁用 = 5,
  驳回 = 6,
}

//计费类型
export enum BillingTypeEnum {
  计时 = 1,
  计件 = 2,
}

/** 账户渠道列表 */
export const AccountTypeEnumList = [
  { value: AccountTypeEnum.支付宝, text: '支付宝', tagType: 'primary' },
  { value: AccountTypeEnum.微信, text: '微信', tagType: 'success' },
  { value: AccountTypeEnum.扫码支付, text: '扫码支付', tagType: 'default' },
  { value: AccountTypeEnum.银行卡, text: '银行卡', tagType: 'default' },
  { value: AccountTypeEnum.其他, text: '其他', tagType: 'default' },
]

/** 销售订单状态列表 */
export const SaleOrderStatusEnumList = [
  { value: SaleOrderStatusEnum.待付款, text: '待付款', tagType: 'warning' },
  { value: SaleOrderStatusEnum.已完成, text: '已完成', tagType: 'success' },
]

/** 产品类型列表 */
export const ProductTypeEnumList = [
  { value: ProductTypeEnum.通用件, text: '通用件', tagType: 'default' },
  { value: ProductTypeEnum.原料, text: '原料', tagType: 'default' },
  { value: ProductTypeEnum.加工产品, text: '加工产品', tagType: 'primary' },
  { value: ProductTypeEnum.销售产品, text: '销售产品', tagType: 'success' },
]

/** 收支类型列表 */
export const IncomeTypeEnumList = [
  { value: IncomeTypeEnum.进账, text: '进账', tagType: 'success' },
  { value: IncomeTypeEnum.出账, text: '出账', tagType: 'warning' },
]

/** 通用状态列表 */
export const PublicStatusEnumList = [
  { value: PublicStatusEnum.正常, text: '正常', tagType: 'success' },
  { value: PublicStatusEnum.禁用, text: '禁用', tagType: 'error' },
  { value: PublicStatusEnum.待处理, text: '待处理', tagType: 'default' },
  { value: PublicStatusEnum.驳回, text: '驳回', tagType: 'warning' },
]

/** 客户类型列表 */
export const ClientTypeEnumList = [
  { value: ClientTypeEnum.供应商, text: '供应商', tagType: 'primary' },
  { value: ClientTypeEnum.客户, text: '客户', tagType: 'warning' },
]

/** 计费类型列表 */
export const BillingTypeEnumList = [
  { value: BillingTypeEnum.计件, text: '计件', tagType: 'primary' },
  { value: BillingTypeEnum.计时, text: '计时', tagType: 'warning' },
]
