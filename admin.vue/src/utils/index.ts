import { DataTableColumn, NTag, TreeSelectOption } from 'naive-ui'
import { TablePropsType } from '@/types/components'
import { VodEpItem } from '@/store/types'

export function isExternal(path: string) {
  return /^(https?:|mailto:|tel:)/.test(path)
}

export function uuid() {
  const s: Array<any> = []
  const hexDigits = '0123456789abcdef'
  for (let i = 0; i < 36; i++) {
    s[i] = hexDigits.substr(Math.floor(Math.random() * 0x10), 1)
  }
  s[14] = '4' // bits 12-15 of the time_hi_and_version field to 0010
  s[19] = hexDigits.substr((s[19] & 0x3) | 0x8, 1) // bits 6-7 of the clock_seq_hi_and_reserved to 01
  s[8] = s[13] = s[18] = s[23] = '-'
  const uuid = s.join('')
  return uuid
}

export function randomString(length: number) {
  const str = '0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ'
  let result = ''
  for (let i = length; i > 0; --i) {
    result += str[Math.floor(Math.random() * str.length)]
  }
  return result
}

/**
 * 中划线字符驼峰
 * @param {*} str 要转换的字符串
 * @returns 返回值
 */
export function toHump(str: string): string {
  if (!str) return str
  return str
    .replace(/\-(\w)/g, function (all, letter) {
      return letter.toUpperCase()
    })
    .replace(/(\s|^)[a-z]/g, function (char) {
      return char.toUpperCase()
    })
}

export function sortColumns(originColumns: DataTableColumn[], newColumns: TablePropsType[]) {
  if (!originColumns || !newColumns) {
    return
  }
  if (newColumns.length === 0) {
    originColumns.length = 0
  } else {
    const selectionItem = originColumns.find((it) => it.type === 'selection')
    originColumns.length = 0
    if (selectionItem) {
      originColumns.push(selectionItem)
    }
    originColumns.push(...newColumns)
  }
}

export function transformTreeSelect(
  origin: any[],
  labelName: string,
  keyName: string
): TreeSelectOption[] {
  const tempSelections: TreeSelectOption[] = []
  origin.forEach((it) => {
    const selection = {
      label: it[labelName],
      key: it[keyName],
    } as TreeSelectOption
    if (it.children) {
      selection.children = transformTreeSelect(it.children, labelName, keyName)
    }
    tempSelections.push(selection)
  })
  return tempSelections
}

/**
 * 枚举转换为字典
 * @param enumObj 枚举 必须是一个具有字符串或数字索引签名的对象
 * @returns 返回 {label: xxx, value: xxx} 数组
 */
export function getListByEnum<T extends { [index: string]: string | number }>(
  enumObj: T
): { label: keyof T; value: T[keyof T] }[] {
  const result = []
  for (const key in enumObj) {
    if (isNaN(parseInt(key))) {
      // 过滤掉枚举值的数字索引
      result.push({ label: key as keyof T, value: enumObj[key] })
    }
  }
  return result
}

/**
 * 找到并删除数组某一项
 * @param arrayItems 待处理数组
 * @param predicate 条件 (item)=>item.type==xxxx
 */
export function findByRemove<T>(
  arrayItems: Array<T>,
  predicate: (value: T, index: number, array: T[]) => unknown
) {
  const filterItem = arrayItems.filter(predicate)
  if (filterItem.length > 0) {
    const index = arrayItems.indexOf(filterItem[0])
    if (index != -1) {
      arrayItems.splice(index, 1)
    }
  }
}

/**
 * 根据条件获取数据某一个值 没有则null
 * @param arrayItems 待处理数组
 * @param predicate 条件 (item)=>item.type==xxxx
 */
export function firstOrDefault<T>(
  arrayItems: Array<T>,
  predicate: (value: T, index: number, array: T[]) => unknown
) {
  const filterItem = arrayItems.filter(predicate)
  if (filterItem.length > 0) {
    return filterItem[0]
  }

  return null
}

/**
 * 多枚举转为一个数组
 * @param num 待转换的数据 必须是2的 幂次值
 * @returns 返回一个数组
 */
export function numberToArray(num: number): number[] {
  const result: number[] = []
  let power = 0

  while (num > 0) {
    if (num & 1) {
      const value = 1 << power
      result.push(value)
    }
    power++
    num = num >> 1
  }

  return result
}

/**
 * 数组转一个多枚举值
 * @param arr 待转换的数组 [1,2]
 * @returns 多枚举值 3
 */
export function arrayToNumber(arr: number[]): number {
  let result = 0

  for (const num of arr) {
    result |= num
  }

  return result
}

/**
 * table排序转为sql排序
 * @param order table排序类型
 * @returns 返回sql排序类型
 */
export function toOrderBy(order: 'ascend' | 'descend'): 'asc' | 'desc' {
  if (order == 'ascend') {
    return 'asc'
  }

  return 'desc'
}

/**
 * 影片名处理
 * @param filename 待处理名
 * @returns 影片基础名
 */
export function vodNameProcess(filename: string): string | null {
  //   1975飞越疯人院.One.Flew.Over.the.Cuckoo's.Nest.mkv=>飞越疯人院
  // 1975飞越疯人院.mkv=>飞越疯人院
  // [LIBHD]凹凸魔女的亲子日常01.mp4=>   匹配失败
  // 2010.监守自盗.Inside.Job.mkv=>监守自盗

  // 移除括号内的内容和扩展名
  const nameWithoutExtension = filename.split('.')[0]
  // 查找并移除开头的年份（四位数字）
  const titleStart = nameWithoutExtension.search(/[^\d]/)
  // 如果没有找到年份，则返回“匹配失败”
  if (titleStart < 4) {
    return null
  }

  // 提取年份之后的内容作为电影名称
  return nameWithoutExtension.substring(titleStart).trim()
}

/**
 * 使用正则表达式替换掉所有非字母数字汉字的字符
 * @param str 待处理字符串
 * @returns 移除特殊字符后的字符串
 */
export function removeSpecialCharacters(str: string) {
  // 使用正则表达式替换掉所有非字母数字汉字的字符
  return str.replace(/[^\w\u4e00-\u9fa5]/gi, '')
}

/**
 * 播放链接转为标准剧集数组
 * @param playUrls 播放链接
 * @returns 标准剧集数组
 */
export function getEpItemsByVodPlayUrls(playUrls: string): VodEpItem[] {
  const epItems = playUrls
    .replace(/\r\n/g, '\n')
    .split('\n')
    .map((line, index) => {
      const [, epUrl] = line.split('$')
      return { episodeName: (index + 1).toString(), episodeUrl: epUrl } as VodEpItem
    })

  if (epItems.length == 1) {
    epItems[0].episodeName = '极速'
  }

  return epItems
}

/**
 * 过滤掉对象中所有为 null 的属性
 * @param obj 待处理对象
 * @returns
 */
export const removeNullProperties = (obj: any) => {
  return Object.entries(obj).reduce((acc: any, [key, value]) => {
    if (value !== null) {
      acc[key] = value
    }
    return acc
  }, {})
}
