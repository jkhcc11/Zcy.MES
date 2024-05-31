import { baseURL } from './axios.config'

export const baseAddress = baseURL

export const test = '/test'

export const login = '/mes-normal/User/login'
export const refresh = '/parse-user/refresh'

export const updateUserInfo = '/updateUser'

export const addUserInfo = '/addUser'

// export const getMenuListByRoleId = '/user-permission/get-menu'
export const getMenuListByRoleId = '/mes-login/User/get-permission'

export const getAllMenuByRoleId = '/getAllMenuByRoleId'

export const deleteUserById = '/deleteUserById'

export const getDepartmentList = '/getDepartmentList'

export const addDepartment = '/addDepartment'

export const getRoleList = '/getRoleList'

export const getMenuList = '/getMenuList'

export const getParentMenuList = '/getParentMenuList'

export const getTableList = '/getTableList'

export const getCardList = '/getCardList'

export const getCommentList = '/getCommentList'

//cookie类型
export const cookieTypeApi = {
  query: '/cookie-type/query',
  createAndUpdate: '/cookie-type/create-and-update',
  delete: '/cookie-type/delete',
}

//用户
export const parseUserApi = {
  querySubAccount: '/parse-user/query-sub-account',
  createAndUpdateSubAccount: '/parse-user/create-and-update-sub-account',
  getAllCookieType: '/parse-user/get-all-cookie-type',
  getAllSubAccount: '/parse-user/get-all-sub-account',
}

//基础信息
export const baseUserInfoApi = {
  getLoginInfo: '/user-info/getLoginUserInfo',
  saveInfo: '/user-info/saveUserInfo',
  create: '/user-info/create',
  audit: '/user-info/audit',
  query: '/user-info/query',
  delayDate: '/user-info/delay-data',
  updateUserRemark: '/user-info/update-user-remark',
}

//网盘api
export const cloudDiskApi = {
  queryListWithAli: '/ali/index',
  getCookieTypeIdWithAi: '/ali/get-cookie-type-id',
  renameWithAli: '/ali/rename',
  getQrWithAli: '/ali/get-qr',
  getTokenWithAli: '/ali/get-token',

  queryListWithSt: '/st/index',
  getCookieTypeIdWithSt: '/st/get-cookie-type-id',
  renameWithSt: '/st/rename',

  queryListWithTyPerson: '/ty/index',
  getCookieTypeIdWithTyPerson: '/ty/get-cookie-type-id',
  renameWithTyPerson: '/ty/rename',
  loginWithTyPerson: '/ty/login-with-ty-person',

  queryListWithTyCrop: '/ty/crop-index',
  getCookieTypeIdWithTyCrop: '/ty/crop-get-cookie-type-id',
  renameWithTyCrop: '/ty/crop-rename',

  queryListWithTyFamily: '/ty/family-index',
  getCookieTypeIdWithTyFamily: '/ty/family-get-cookie-type-id',
  renameWithTyFamily: '/ty/family-rename',
  syncWithTyFamily: '/ty/family-sync',

  queryListWith139: '/pan139/index',
  getCookieTypeIdWith139: '/pan139/get-cookie-type-id',
  renameWith139: '/pan139/rename',

  queryListWithTxShare: '/tx-cloud-share/index',
  getCookieTypeIdWithTxShare: '/tx-cloud-share/get-cookie-type-id',
  renameWithTxShare: '/tx-cloud-share/rename',
  syncWithTxShare: '/tx-cloud-share/sync',

  queryListWith115: '/pan115/index',
  getCookieTypeIdWith115: '/pan115/get-cookie-type-id',
  renameWith115: '/pan115/rename',
}

//Server-cookie类型
export const serverCookieApi = {
  query: '/server-cookie/query',
  createAndUpdate: '/server-cookie/create-and-update',
  delete: '/server-cookie/delete',
  audit: '/server-cookie/audit',
}

//预览
export const previewApi = {
  common: '/preview/common',
}

//系统
export const systemApi = {
  sendVod: '/system/vod-send',
  batchCreateToken: '/system/batch-create-token',
}

//解析记录
export const recordHistoryApi = {
  query: '/record-history/query',
  queryTop: '/record-history/get-top',
  queryDaySum: '/record-history/query-day-sum',
}

export const parseSelfApi = {
  tyLogin: '/api-v2/login-with-ty-person',
}

//角色菜单
export const roleMenuApi = {
  query: '/mes-manager/RoleMenu/menu/query',
  getAllMenuTree: '/mes-manager/RoleMenu/menu/get-all-menu-tree',
  delete: '/mes-manager/RoleMenu/menu/delete',
  activateCancel: '/mes-manager/RoleMenu/menu/activate-cancel',
  createAndUpdate: '/mes-manager/RoleMenu/menu/create-and-update',
  getAllOneMenu: '/mes-manager/RoleMenu/menu/get-all-one-menu',

  createAndUpdateRoleMenu: '/mes-manager/RoleMenu/role-auth/create-and-update',
  getRoleActivateMenu: '/mes-manager/RoleMenu/role-auth',

  queryRole: '/mes-manager/RoleMenu/role/query',
  createAndUpdateRole: '/mes-manager/RoleMenu/role/create-and-update',
  deleteRole: '/mes-manager/RoleMenu/role/delete',
}
