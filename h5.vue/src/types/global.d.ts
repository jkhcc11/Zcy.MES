/** 通用分页结果类型 */
export type PageResult<T> = {
  //总数
  total: number
  //items
  items: T[]
}

/** 通用分页参数类型 */
export type PageParams = {
  /** 页码：默认值为 1 */
  page?: number
  /** 页大小：默认值为 10 */
  pageSize?: number
}

//基础
export type BaseEntityDto<TKey> = {
  // key
  id: TKey
}

//创建基础
export type BaseFullAuditEntityDto<TKey> = BaseEntityDto<TKey> & {
  // 创建时间
  createdTime: Date
  // 用户Id
  createdUserId?: number
  // 修改时间
  modifyTime?: Date
  // 修改人
  modifyUserId?: number
  // 删除时间
  deletedTime?: Date
  // 删除人
  deletedUserId?: number
  // 是否删除
  isDelete: boolean
}

//过滤筛选Item
export type FilterItem = {
  //显示
  label: string
  //value
  value: string
  //后缀
  suffix?: string | null
}
