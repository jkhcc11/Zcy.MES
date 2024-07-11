import type { PublicStatusEnum, ProductTypeEnum } from '@/services/constants'
import type { PageParams } from '@/types/global'

//有效产品列表dto
export type QueryValidProductDto = BaseFullAuditEntityDto<string> & {
  // 产品分类Id
  productTypeId: string
  // 产品分类名
  productTypeName?: string
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
  // 产品备注
  productRemark?: string
}

//产品详情级联工序Item
export type GetProductDetailCascadeItem = {
  //产品工序Id
  productProcessId: string
  //产品工艺名
  craftName: string
  //产品工艺计费类型
  billingType: BillingTypeEnum
}

//有效产品列表input
export type QueryValidProductInput = {
  // 产品分类Id
  productTypeId?: string | null
  // 产品类型
  productType?: string | null
  //关键字
  keyWord?: string | null
}
