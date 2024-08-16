/**
 * 日期格式化函数
 * @param date 日期对象
 * @param format 日期格式，默认为 YYYY-MM-DD HH:mm:ss
 */
export const formatDate = (date: Date, format = 'YYYY-MM-DD HH:mm:ss') => {
  // 获取年月日时分秒，通过 padStart 补 0
  const year = String(date.getFullYear())
  const month = String(date.getMonth() + 1).padStart(2, '0')
  const day = String(date.getDate()).padStart(2, '0')
  const hours = String(date.getHours()).padStart(2, '0')
  const minutes = String(date.getMinutes()).padStart(2, '0')
  const seconds = String(date.getSeconds()).padStart(2, '0')

  // 返回格式化后的结果
  return format
    .replace('YYYY', year)
    .replace('MM', month)
    .replace('DD', day)
    .replace('HH', hours)
    .replace('mm', minutes)
    .replace('ss', seconds)
}

/**
 * 获取业务默认日期范围
 * @param diffDay 多少天之前 默认365（一年）
 * @returns  sDate:开始 eDate:结束
 */
export const getBusinessDefaultDateTimeRange = (diffDay: number = 365) => {
  // 获取当前日期
  const currentDate = new Date()

  // 格式化日期为YYYY-MM-DD
  const formatDate = (date: Date) => {
    const year = date.getFullYear()
    const month = String(date.getMonth() + 1).padStart(2, '0')
    const day = String(date.getDate()).padStart(2, '0')
    return `${year}-${month}-${day}`
  }

  // 计算开始日期（当前日期减去365天）
  const startDate = new Date()
  startDate.setDate(currentDate.getDate() - diffDay)

  // 格式化开始日期和结算日期
  return {
    sDate: formatDate(startDate),
    eDate: formatDate(currentDate),
  }
}

/**
 * 获取是否授权了定位权限
 * https://uniapp.dcloud.net.cn/api/other/authorize.html#authorize
 * @param { Boolean } launchAuth: 是否发起授权请求, 初次有效
 * @return { Boolean }
 */
export function getUserInfoAuth(launchAuth: boolean) {
  return new Promise((resolve) => {
    uni.getSetting({
      success: (res) => {
        if (launchAuth && res.authSetting['scope.userInfo'] === undefined) {
          return uni.authorize({
            scope: 'scope.userInfo',
            success: () => {
              resolve(true)
            },
            fail: () => {
              resolve(false)
            },
          })
        }

        resolve(res.authSetting['scope.userInfo'])
      },
      fail: (err) => {
        console.error(err)
      },
    })
  })
}
