import { Ref, UnwrapRef } from 'vue'
import { RouteMeta, RouteRecordNormalized, RouteRecordRaw } from 'vue-router'

export const baseRemarkLength = 150
export const moneyMax = 99999999
export const recordNameLength = 20
export const constSystemInput = 'systeminput'
export const constNewZyFlag = 'zcyai.com'

export const ConstVideoCountries = [
  '美国',
  '英国',
  '韩国',
  '日本',
  '泰国',
  '中国香港',
  '中国大陆',
  '中国台湾',
  '其他',
]

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

export interface AppConfigState {
  projectName: string
  theme: ThemeMode
  sideTheme: SideTheme
  themeColor: string
  layoutMode: LayoutMode
  deviceType: DeviceType
  sideWidth: number
  pageAnim: PageAnim
  isFixedNavBar: boolean
  isCollapse: boolean
  actionBar: {
    isShowSearch: boolean
    isShowMessage: boolean
    isShowRefresh: boolean
    isShowFullScreen: boolean
  }
}

export interface VisitedRouteState {
  visitedRoutes: RouteRecordNormalized[]
  affixRoutes: RouteRecordNormalized[]
}

export interface CachedRouteState {
  cachedRoutes: string[]
}

export interface OriginRoute {
  parentPath?: string
  menuUrl: string
  menuName?: string
  routeName?: string
  hidden?: boolean
  outLink?: string
  affix?: boolean
  cacheable?: boolean
  isRootPath?: boolean
  iconPrefix?: string
  icon?: string
  badge?: string | number
  isSingle?: boolean
  localFilePath?: string
  children?: Array<OriginRoute>
}

export interface RouteMetaType extends RouteMeta {
  icon?: string
  title?: string
  cacheable?: boolean
  affix?: boolean
}

export interface SplitTab {
  label: string
  iconPrefix?: string | unknown
  icon: string
  fullPath: string
  children?: Array<RouteRecordRaw>
  checked: Ref<UnwrapRef<boolean>>
}

//搜索参数
export interface CloudDiskSearch {
  keyWord: string | null
  subAccountId: string
  fileId: string | null
  extId: string | null
}

//页头
export interface CloudDiskPageHeader {
  pathId: string
  pathName: string
  isCurrent: Boolean
}

//cms入库信息
export interface CmsSendInfo {
  sendType: CmsTypeEnum
  apiUrl?: string | null
  sendPassWord?: string
  vodTypeName?: string | null
  playFrom?: string | null
}

//剧集Item
export interface VodEpItem {
  id: string | null
  episodeName: string
  episodeUrl: string
}

//豆瓣信息
export interface DouBanInfo {
  /**
   * db豆瓣Id
   */
  id: string
  /**
   * 影片名
   */
  videoTitle: string
  /**
   * 国家
   */
  videoCountries: string | null
  /**
   * 导演
   */
  videoDirectors: string | null
  /**
   * 演员 多个逗号隔开
   */
  videoCasts: string | null
  /**
   * 影片年
   */
  videoYear: number | null
  /**
   * 海报
   */
  videoImg: string | null
}
//影片详情
export interface VodDetail {
  /**
   * 影片Key
   */
  id: string
  /**
   * 关键字
   */
  keyWord: string
  /**
   * 影片年
   */
  videoYear: number | null
  /**
   *影片扩展信息
   */
  videoMainInfo: DouBanInfo
  /**
   * 剧集组
   */
  episodeGroup: Array<any>
}

//影片更新
export interface VodUpdateState {
  fromPage: 'cloudlink' | 'other' | null
  /**
   * 影片名和豆瓣名是否一致
   */
  isSameName: boolean
  /**
   * 演员名是否至少一个一致
   */
  isSameCasts: boolean
  /**
   * 是否完全匹配（名称、年份、演员至少匹配一个和豆瓣一致）
   */
  isAllMatch?: boolean | null
  /**
   * 基础影片名（输入的）
   */
  baseVodName: string
  /**
   * 基础系列Id（选择的）
   */
  baseVodSeriesId: string | null
  /**
   * 基础系列名（选择的）
   */
  baseVodSeriesStr: string | null
  /**
   * 播放链接 多个换行 格式：
   * 第1集$xxxxxxx
   * 第2集$xxxxxxx
   */
  vodPlayUrlsWithStr: string
  /**
   * 播放链接列表
   */
  vodPlayItems: VodEpItem[]
  /**
   * 豆瓣Url
   */
  doubanUrl: string
  /**
   * 豆瓣Id
   */
  doubanId: string | null
  /**
   * 豆瓣入库后的Id
   */
  doubanDbId: string | null
  /**
   * 豆瓣名
   */
  doubanName: string | null
  /**
   * 豆瓣年
   */
  doubanYear: number | null
  /**
   * 豆瓣海报
   */
  doubanConvert: string | null
  /***
   * 豆瓣演员信息
   */
  doubanCasts: string | null

  /**
   * 影片Id
   */
  vodId: string | null
  /**
   * 操作的影片名
   */
  vodName: string | null
  /**
   * 影片年
   */
  vodYear: number | null
  /**
   * 影片演员信息
   */
  videoCasts: string | null
  /**
   * 影片分组Id
   */
  groupId: string | null
  /**
   * 剧集Item
   */
  epItems: VodEpItem[]
}

export enum BooleanEnum {
  是 = 'true',
  否 = 'false',
}

export enum FileTypeEnum {
  视频 = 1,
  音频 = 2,
  图片 = 3,
  文件 = 5,
  文件夹 = 10,
  切片视频 = 101,
}

export enum ServerCookieStatusEnum {
  初始化 = 1,
  正常 = 5,
}

export enum CmsTypeEnum {
  苹果V10 = 1,
  海洋 = 10,
  自用 = 100,
}

export enum RecordHistoryTypeEnum {
  访问量 = 1,
  实际请求 = 5,
}

//任务类型
export enum ConvertTaskTypeEnum {
  转码 = 1,
  迁移 = 2,
  找资源 = 10,
  发布资源 = 11,
}

//任务状态
export enum VideoConvertTaskStatusEnum {
  待处理 = 1,
  进行中 = 5,
  审核中 = 8,
  已完成 = 10,
}

//链接类型
export enum SourceLinkTypeEnum {
  百度云 = 1,
  阿里云盘 = 2,
  YYW盘 = 3,
  其他 = 4,
  迅雷 = 5,
  夸克 = 6,
}

//任务影片类型
export enum TaskVodTypeEnum {
  新电影_2000年以后 = 1,
  旧电影_2000年以前 = 2,
  电视剧 = 10,
}

//转换订单状态
export enum ConvertOrderStatusEnum {
  待审核 = 1,
  已完成 = 5,
  作废 = 6,
}

//记录类型
export enum VodManagerRecordTypeEnum {
  电影保存 = 1,
  电视剧保存 = 2,
  创建任务 = 5,
  更新资料存在豆瓣 = 10,
  更新资料不存在豆瓣 = 11,
  录入和反馈 = 15,
  接单审核 = 20,
  接单审核多 = 21,
  下架 = 25,
}

//反馈类型
export enum UserDemandTypeEnum {
  反馈 = 5,
  录入 = 10,
}

//反馈状态
export enum FeedBackInfoStatusEnum {
  待审核 = 0,
  处理中 = 5,
  正常 = 10,
  忽略 = 11,
}

//影片状态
export enum VideoMainStatusEnum {
  正常 = 0,
  禁用 = 1,
  登录 = 2,
  下架 = 10,
}

//影片类型
export enum VodSubtypeEnum {
  未知 = 0,
  电影 = 5,
  电视剧 = 6,
  纪录片 = 7,
  综艺 = 8,
  动画 = 9,
}

//剧集组类型
export enum EpisodeGroupTypeEnum {
  视频播放 = 1,
  视频下载 = 2,
}

//搜索类型
export enum VodSearchTypeEnum {
  低分影片 = 5,
  待维护资源 = 6,
  当天更新 = 2,
  未完结 = 1,
  未匹配豆瓣 = 3,
  已关联解说 = 4,
  资源站资源 = 7,
}

//2024------------------mes
//状态
export enum PublicStatusEnum {
  正常 = 1,
  禁用 = 5,
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
