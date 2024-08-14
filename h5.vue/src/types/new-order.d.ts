import type { AccountTypeEnum, OrderState } from '@/services/constants'
import type { AddressItem } from './address'
import type { PageParams } from '@/types/global'

/** 销售订单查询Input */
export type QueryPageSaleOrderInput = PageParams & {
  //账号类型
  accountType?: string | null
  //关键字
  keyWord?: string | null
  //开始时间
  startTime?: string | null
  //结束时间
  endTime?: string | null
}

/** 出货订单查询Input */
export type QueryPageShipmentOrderInput = PageParams & {
  //关键字
  keyWord?: string | null
  //开始时间
  startTime?: string | null
  //结束时间
  endTime?: string | null
}

//创建订单Item基类
export type BaseCreateOrderDetailInput = {
  // 数量
  count: number
  // 产品Id
  productId: string

  //以下为冗余显示的
  // 产品名
  productName: string
  // 产品类型
  productType: ProductTypeEnum
  // 是否散件
  // 散件 -> 没规格
  // 非散件 -> 有规格
  isLoose: boolean
  // 单位
  // kg/个
  unit: string
  // 规格
  // 箱/盒/袋
  spec?: string
  // 规格数
  // 一箱10kg/1盒10个/1袋2kg
  specCount: number
}

//创建订单基类
export type BaseCreateOrderInput = {
  // 经办人Id
  managerUserId?: number | null
  // 订单备注
  orderRemark?: string | null
  // 订单日期
  orderDate?: string | null
}

//查询订单基础dto
export type BaseQueryPageOrderDto = BaseFullAuditEntityDto<string> & {
  // 订单号
  orderNo: string
  // 经办人Id
  managerUserId: number
  // 经办人昵称
  // 创建订单时的昵称，修改后根据经办人Id来
  managerUser: string
  // 订单备注
  orderRemark?: string
  // 订单日期
  orderDate: string
  // 公司Id
  companyId: string
  // 公司名
  companyName?: string
}

//订单详情基础dto
export type BaseGetOrderDetailDto = BaseFullAuditEntityDto<string> & {
  // 订单号
  orderNo: string
  // 经办人Id
  managerUserId: string
  // 经办人昵称
  // 创建订单时的昵称，修改后根据经办人Id来
  managerUser: string
  // 订单备注
  orderRemark?: string
  // 订单日期
  orderDate: string
  // 公司Id
  companyId: string
  // 公司名
  companyName?: string
}

//订单详情基础Item dto
export type BaseGetOrderDetailItemDto = BaseEntityDto<number> & {
  // 订单Id
  orderId: string
  // 产品Id
  productId: string
  // 数量
  count: number
  // 产品名
  productName?: string
  // 是否散件 冗余
  // 散件 -> 没规格
  // 非散件 -> 有规格
  isLoose: boolean
  // 单位 冗余
  // kg/个
  unit?: string
  // 规格 冗余
  spec?: string
  // 规格数 冗余
  // 一箱10kg/1盒10个/1袋2kg 冗余
  specCount: number
}

//查询销售订单dto
export type QueryPageSaleOrderDto = BaseQueryPageOrderDto & {
  // 账号类型
  accountType?: AccountTypeEnum
  // 供应商|客户Id
  supplierClientId: number
  // 客户名
  supplierClientName?: string
  // 销售单状态
  saleOrderStatus: SaleOrderStatusEnum
  // 运费价格
  freightPrice: number
  // 销售总价
  // 订单详情总价（不含运费）
  sumSalePrice: number
  // 订单价格
  // 运费+销售总价
  orderPrice: number
}

//销售订单详情
export type GetSaleOrderDetailDto = BaseGetOrderDetailDto & {
  // 账号类型
  accountType?: AccountTypeEnum
  // 供应商|客户Id
  supplierClientId: number
  // 客户名
  supplierClientName?: string
  // 销售单状态
  saleOrderStatus: SaleOrderStatusEnum
  // 运费价格
  freightPrice: number
  // 销售总价
  // 订单详情总价（不含运费）
  sumSalePrice: number
  // 订单价格
  // 运费+销售总价
  orderPrice: number
  // 销售订单商品列表
  orderDetails?: GetSaleOrderDetailItemDto[]
}

//销售订单详情item
export type GetSaleOrderDetailItemDto = BaseGetOrderDetailItemDto & {
  // 单价
  unitPrice: number
  // 总价
  sumPrice: number
  // 备注
  remark?: string
}

//创建销售订单
export type CreateSaleOrderInput = BaseCreateOrderInput & {
  // 账号类型
  accountType?: AccountTypeEnum
  // 客户Id
  supplierId?: number | null
  // 运费价格
  freightPrice: number
  // 销售单状态
  saleOrderStatus?: SaleOrderStatusEnum
  // 销售订单详情
  orderDetails?: CreateSaleOrderDetailInput[] | null
}

//创建销售订单item
export type CreateSaleOrderDetailInput = BaseCreateOrderDetailInput & {
  // 单价
  unitPrice: number
  // 备注
  remark?: string
}
