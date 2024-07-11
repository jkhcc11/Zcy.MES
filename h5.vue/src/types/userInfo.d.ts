/** 通用的用户信息 */
type BaseProfile = {
  /** 用户ID */
  userId: string
  /** 昵称 */
  userNick: string
  /** 结算基数 */
  baseSettlement: number
  /** 公司Id  */
  companyId: string
  /**
   * 是否超管
   */
  isSuperAdmin: boolean
  /**
   * 工号
   */
  userNo: string
}

/** 登录用户信息 */
export type LoginResult = BaseProfile & {
  // /** 手机号 */
  // mobile: string
  /** 登录凭证 */
  accessToken: string
}

//小程序登录权限
export type GetLoginInfoWithWxDto = {
  // 基础数据
  isShowBaseData: boolean
  // 采购销售
  isPurchaseSaleShow: boolean
  // 财务备忘录
  isFinancialMemoShow: boolean
  // 我的报工
  meReport: boolean
  // 管理报工
  adminReport: boolean
  // 报工统计
  bossReport: boolean
  // 是否禁用
  isBan: boolean
}

// /** 个人信息 用户详情信息 */
// export type ProfileDetail = BaseProfile & {
//   /** 性别 */
//   gender?: Gender
//   /** 生日 */
//   birthday?: string
//   /** 省市区 */
//   fullLocation?: string
//   /** 职业 */
//   profession?: string
// }
// /** 性别 */
// export type Gender = '女' | '男'

// /** 个人信息 修改请求体参数 */
// export type ProfileParams = Pick<
//   ProfileDetail,
//   'nickname' | 'gender' | 'birthday' | 'profession'
// > & {
//   /** 省份编码 */
//   provinceCode?: string
//   /** 城市编码 */
//   cityCode?: string
//   /** 区/县编码 */
//   countyCode?: string
// }
