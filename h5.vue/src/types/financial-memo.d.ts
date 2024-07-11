//查询收支记录
export type QueryPageIncomeRecordInput = QueryPageInput & {
  // 关键字
  keyWord?: string
  // 收支类型
  incomeType?: string
  // 账户类型
  accountType?: string
  // 开始时间
  startTime?: Date
  // 结算时间
  endTime?: Date
}

//查询收支记录dto
export type QueryPageIncomeRecordDto = BaseFullAuditEntityDto<string> & {
  // 收支类型
  incomeType: IncomeTypeEnum
  // 账户类型
  accountType: AccountTypeEnum
  // 记录名
  recordName: string
  // 金额
  money: number
  // 记录日期
  recordDate: string
  // 经办人
  managerUser?: string
  // 备注
  remark?: string
}

//创建收支记录
export type CreateIncomeRecordInput = {
  // 收支类型
  incomeType: IncomeTypeEnum
  // 账户类型
  accountType: AccountTypeEnum
  // 记录名
  recordName: string
  // 金额
  money: number
  // 记录日期
  // 默认当天
  recordDate?: string | null
  // 经办人
  managerUser?: string | null
  // 备注
  remark?: string | null
}

//创建收款
export type CreateProceedsRecordInput = {
  // 客户Id
  supplierClientId?: number | null
  // 收款记录名
  proceedsRecordName: string
  // 记录日期
  recordDate?: string | null
  // 收款账户类型
  accountType: AccountTypeEnum
  // 收款金额
  money?: number | null
  // 备注
  remark?: string | null
}

//查询收款input
export type QueryPageProceedsRecordInput = QueryPageInput & {
  // 客户Id
  supplierClientId?: number
  // 关键字
  keyWord?: string
  // 开始时间
  startTime?: Date
  // 结算时间
  endTime?: Date
}

//查询收款dto
export type QueryPageProceedsRecordDto = BaseFullAuditEntityDto<string> & {
  supplierClientName?: string
  // 收款记录名
  proceedsRecordName: string
  // 记录日期
  recordDate: string
  // 收款账户类型
  accountType: AccountTypeEnum
  // 收款金额
  money: number
  // 备注
  remark?: string
}
