<template>
  <div class="main-container">
    <TableBody ref="tableBody">
      <template #header>
        <TableHeader :show-filter="true" @search="onSearch" @reset-search="onResetSearch">
          <template #search-content>
            <DataForm
              ref="searchForm"
              :form-config="{
                labelWidth: 60,
              }"
              :options="SearchFormOptions"
              preset="grid-item"
            />
          </template>

          <template #top-toolbar>
            <AddButton @add="onAddItem" />
          </template>
        </TableHeader>
      </template>
      <template #default>
        <n-data-table
          size="small"
          :loading="tableLoading"
          :data="dataList"
          :columns="tableColumns"
          :row-key="rowKey"
          :style="{ height: `${tableHeight}px` }"
          :flex-height="true"
          :scroll-x="800"
          @update:checked-row-keys="onRowCheck"
        />
      </template>
      <template #footer>
        <TableFooter :pagination="pagination" :showRefresh="false" />
      </template>
    </TableBody>

    <ModalDialog
      ref="modalDialog"
      :submitLoading="submitLoading"
      contentHeight="55vh"
      @confirm="onConfirm"
    >
      <template #content>
        <n-form
          ref="editForm"
          label-width="100"
          size="medium"
          label-placement="left"
          :model="editFormDataRef"
          :rules="editFormDataRule"
        >
          <n-grid :cols="2">
            <n-form-item-gi label="类型" path="subAccountTypeId">
              <n-select
                placeholder="请选择类型"
                filterable
                :options="typeOptions"
                v-model:value="editFormDataRef.subAccountTypeId"
                :on-update:value="onCookieTypeChange"
              />
            </n-form-item-gi>
            <n-form-item-gi label="别名" path="alias">
              <n-input placeholder="备注和显示用" v-model:value="editFormDataRef.alias" />
            </n-form-item-gi>
            <n-form-item-gi label="天翼账号" v-if="tyInfo.isShow">
              <n-input
                placeholder="账号"
                :allow-input="onlyAllowNumber"
                :readonly="!tyInfo.isShow"
                :disabled="!tyInfo.isShow"
                :maxlength="11"
                show-count
                clearable
                v-model:value="tyInfo.tyUserName"
              />
            </n-form-item-gi>
            <n-form-item-gi label="天翼密码" v-if="tyInfo.isShow">
              <n-input
                placeholder="输入密码后 回车 获取"
                :readonly="!tyInfo.isShow"
                :disabled="!tyInfo.isShow"
                v-model:value="tyInfo.tyPwd"
                @keyup="onTyPwdKeyUp"
              />
            </n-form-item-gi>

            <n-form-item-gi label="扫码" v-if="aliInfo.isShow">
              <n-spin :show="aliInfo.loading">
                <img :src="qrData.imgBase64" class="canvas" v-if="aliInfo.isShow" />

                <n-tooltip placement="bottom" trigger="hover">
                  <template #trigger>
                    <n-button type="info" size="small" @click="getTokenByQrAli"> 获取 </n-button>
                  </template>
                  <span> 先扫码，后点获取。获取不到就切换一下类型，再切回来 </span>
                </n-tooltip>
              </n-spin>
            </n-form-item-gi>

            <n-form-item-gi label="Cookie" :span="2" path="cookie">
              <n-input
                placeholder="Cookie"
                type="textarea"
                v-model:value="editFormDataRef.cookie"
              />
            </n-form-item-gi>
            <n-form-item-gi label="旧账号信息" path="oldSubAccountInfo">
              <n-input
                placeholder="新增可不用 多个, 逗号隔开"
                v-model:value="editFormDataRef.oldSubAccountInfo"
              />
            </n-form-item-gi>
            <n-form-item-gi label="业务Id" path="businessId">
              <n-input
                placeholder="适用于跨网盘切换资源 非必要不要填"
                v-model:value="editFormDataRef.businessId"
              />
            </n-form-item-gi>
            <n-form-item-gi label="主账号列表" :span="2" path="relationalUserArray">
              <n-select
                v-model:value="editFormDataRef.relationalUserArray"
                filterable
                multiple
                tag
                placeholder="主账号Id，按回车确认"
                :show-arrow="false"
                :show="false"
              />
            </n-form-item-gi>
            <n-form-item-gi label="同步更新" :span="2" path="isSyncServerCookie">
              <n-switch v-model:value="editFormDataRef.isSyncServerCookie">
                <template #checked> 同步服务器Cookie</template>
                <template #unchecked> 不同步服务器Cookie </template>
              </n-switch>
            </n-form-item-gi>
          </n-grid>
        </n-form>

        <!-- <DataForm
          ref="editForm"
          :options="EditFormOptions"
          :form-config="formConfig"
          preset="grid-item"
        /> -->
      </template>
    </ModalDialog>
  </div>
</template>

<script lang="ts">
  import { get, post } from '@/api/http'
  import { cloudDiskApi, parseUserApi } from '@/api/url'
  import {
    TableActionModel,
    usePagination,
    useRenderAction,
    useRowKey,
    useTable,
    useTableColumn,
    useTableHeight,
  } from '@/hooks/table'
  import { DataFormType, ModalDialogType } from '@/types/components'
  import { DataTableColumn, FormRules, NForm, useDialog, useMessage } from 'naive-ui'
  import { defineComponent, h, onMounted, reactive, ref } from 'vue'
  import { SearchFormOptions } from './Data'
  import Qrcode from 'qrcode'
  import useCookieTypeStore from '@/store/modules/cookie-type'
  import useSubAccountListStore from '@/store/modules/sub-account-list'
  export default defineComponent({
    name: 'SubAccount',
    setup() {
      const formParams = ref<any>({})
      const submitLoading = ref(false)
      const useSubAccountStore = useSubAccountListStore()
      const useCookieType = useCookieTypeStore()
      const canvasRef = ref<any>(null)
      const tyInfo = reactive({
        tyUserName: '',
        tyPwd: '',
        isShow: false,
      })
      const aliInfo = reactive({
        isShow: false,
        time: '',
        cKey: '',
        loading: false,
      })

      const qrData = ref<any>({
        imgBase64: '',
      })

      //新增或编辑
      const modalDialog = ref<ModalDialogType | null>(null)
      const editForm = ref<typeof NForm | null>(null)
      const editFormDataRef = ref<any>({
        subAccountId: null,
        subAccountTypeId: null,
        alias: null,
        cookie: null,
        oldSubAccountInfo: null,
        businessId: null,
        isSyncServerCookie: false,
        relationalUserArray: null,
      })
      const typeOptions = ref<any[]>([])
      const editFormDataRule = {
        subAccountTypeId: {
          required: true,
          message: '请选择类型',
          trigger: ['change'],
        },
        alias: {
          required: true,
          trigger: ['blur', 'change'],
          message: '请输入别名',
        },
        cookie: {
          required: true,
          trigger: ['blur', 'change'],
          message: '请输入cookie',
        },
      } as FormRules

      //表格
      const searchForm = ref<DataFormType | null>(null)
      const pagination = usePagination(doRefresh)
      //pagination.pageSize = 5
      const table = useTable()
      const message = useMessage()
      const rowKey = useRowKey('id')
      const checkedRowKeys = [] as Array<any>
      const tableColumns = useTableColumn(
        [
          //table.selectionColumn,
          {
            type: 'expand',
            fixed: 'left',
            renderExpand: (rowData) => {
              return h('div', {
                innerHTML: `<strong>Cookie：</strong>${
                  rowData.cookie
                } <br/> <strong>关联主账号列表：</strong>${rowData.relationalUserIds ?? ''}`,
              })
            },
          },
          //  table.indexColumn,
          {
            title: '别名',
            key: 'alias',
            fixed: 'left',
          },
          {
            title: '子账号类型',
            key: 'subAccountTypeStr',
            fixed: 'left',
          },
          {
            title: 'Cookie',
            key: 'cookie',
            width: 100,
            ellipsis: true,
          },
          {
            title: '旧账号信息',
            key: 'oldSubAccountInfo',
          },
          {
            title: '业务Id',
            key: 'businessId',
          },
          {
            title: '关联主账号列表',
            key: 'relationalUserIds',
            width: 100,
            ellipsis: true,
          },
          {
            title: '创建时间',
            key: 'createdTime',
          },
          {
            title: '修改时间',
            key: 'modifyTime',
          },
          {
            title: '操作',
            key: 'actions',
            fixed: 'right',
            render: (rowData) => {
              const normalAction = useRenderAction([
                {
                  label: '编辑',
                  onClick: onUpdateItem.bind(null, rowData),
                },
              ] as TableActionModel[])

              return normalAction
            },
          },
        ],
        {
          align: 'center',
        } as DataTableColumn
      )
      //刷新
      function doRefresh() {
        const params = searchForm.value?.generatorParams()
        if (params) {
          formParams.value = params
        }

        get({
          url: parseUserApi.querySubAccount,
          data: () => {
            return {
              ...formParams.value,
              page: pagination.page,
              pageSize: pagination.pageSize,
            }
          },
        })
          .then((res) => {
            //请求成功处理
            table.handleSuccess(res.data)
            pagination.setTotalSize(res.data.dataCount)
          })
          .catch(console.log)
      }

      //选中
      function onRowCheck(rowKeys: Array<any>) {
        checkedRowKeys.length = 0
        checkedRowKeys.push(...rowKeys)
      }

      //查询
      function onSearch() {
        pagination.page = 1
        doRefresh()
      }
      //重置查询
      function onResetSearch() {
        searchForm.value?.reset()
      }

      //添加
      function onAddItem() {
        modalDialog.value?.show().then(() => {
          Object.keys(editFormDataRef.value).forEach((key) => {
            editFormDataRef.value[key] = null
          })
          editFormDataRef.value.isSyncServerCookie = false
          //editForm.value?.reset()
        })
      }

      //编辑
      function onUpdateItem(item: any) {
        // 遍历所有键
        Object.keys(item).forEach((key) => {
          // 检查 editFormDataRef.value 是否有这个键
          if (editFormDataRef.value.hasOwnProperty(key)) {
            // 如果有，那么将 temp 中的值赋给 editFormDataRef.value
            editFormDataRef.value[key] = item[key] || null
          }
        })
        editFormDataRef.value.isSyncServerCookie = false
        //editFormDataRef.value.subAccountId=item.subAccountId;
        // EditFormOptions.forEach((it: any) => {
        //   it.value.value = item[it.key] || null
        // })
        modalDialog.value?.show()
      }

      function onConfirm() {
        editForm.value?.validate((errors: any) => {
          if (!errors) {
            // console.log('pd', editFormDataRef.value)
            // message.success('验证成功')

            const pd = editFormDataRef.value
            submitLoading.value = true
            post({
              url: parseUserApi.createAndUpdateSubAccount,
              data: pd,
            })
              .then((res) => {
                if (res.isSuccess) {
                  doRefresh()
                  message.success(res.msg)
                  modalDialog.value?.close()
                } else {
                  message.error(res.msg)
                }
              })
              .finally(async () => {
                submitLoading.value = false
                //添加后清空本地缓存
                useSubAccountStore.reset()
                await useSubAccountStore.init()
              })
          }
        })
        // if (editForm.value?.validator()) {
        //   const pd = editForm.value?.generatorParams()
        //   submitLoading.value = true
        //   post({
        //     url: parseUserApi.createAndUpdateSubAccount,
        //     data: pd,
        //   })
        //     .then((res) => {
        //       if (res.isSuccess) {
        //         doRefresh()
        //         message.success(res.msg)
        //         modalDialog.value?.close()
        //       } else {
        //         message.error(res.msg)
        //       }
        //     })
        //     .finally(async () => {
        //       submitLoading.value = false
        //       //添加后清空本地缓存
        //       useSubAccountStore.reset()
        //       await useSubAccountStore.init()
        //     })
        // }
      }

      //类型切换
      async function onCookieTypeChange(newVal: string) {
        editFormDataRef.value.subAccountTypeId = newVal
        tyInfo.isShow = useCookieType.tyTypeId == newVal
        aliInfo.isShow = useCookieType.aliTypeId == newVal
        if (tyInfo.isShow == false) {
          tyInfo.tyUserName = ''
          tyInfo.tyPwd = ''
        }

        if (aliInfo.isShow) {
          qrData.value.value = 'https://www.baidu.com/' + Date.UTC
          await generatorQr()
        }
      }

      //天翼pwd回车
      function onTyPwdKeyUp(e: any) {
        if (e.key === 'Enter') {
          if (tyInfo.tyUserName && tyInfo.tyPwd) {
            submitLoading.value = true
            post({
              url: cloudDiskApi.loginWithTyPerson,
              data: {
                userName: tyInfo.tyUserName,
                pwd: tyInfo.tyPwd,
                pageLoadType: 'normal',
              },
            })
              .then((result) => {
                editFormDataRef.value.cookie = result.data
              })
              .finally(() => {
                submitLoading.value = false
              })

            //console.log('result', result)
            //console.log('info', tyInfo.value.tyUserName + tyInfo.value.tyPwd)
          }
        }
      }

      //生成二维码
      const generatorQr = () => {
        aliInfo.loading = true
        get({
          url: cloudDiskApi.getQrWithAli,
        })
          .then((getQrReuslt) => {
            aliInfo.cKey = getQrReuslt.data.cKey
            aliInfo.time = getQrReuslt.data.time
            Qrcode.toDataURL(getQrReuslt.data.codeContent, {
              width: 115,
              // color: {
              //   dark: it.darkColor,
              //   light: it.lightColor,
              // },
            }).then((result) => {
              qrData.value.imgBase64 = result
            })
          })
          .finally(() => {
            aliInfo.loading = false
          })
      }

      //获取token
      async function getTokenByQrAli() {
        aliInfo.loading = true
        get({
          url: cloudDiskApi.getTokenWithAli,
          data: {
            aliTime: aliInfo.time,
            aliCKey: aliInfo.cKey,
          },
        })
          .then((getTokenReuslt) => {
            editFormDataRef.value.cookie = getTokenReuslt.data
            aliInfo.isShow = false
          })
          .finally(() => {
            aliInfo.loading = false
          })

        // console.log('getTokenByQrAli')
      }

      onMounted(async () => {
        table.tableHeight.value = await useTableHeight()

        await useCookieType.init()
        typeOptions.value = useCookieType.cachedCookieType
        doRefresh()
      })
      return {
        ...table,
        rowKey,
        submitLoading,
        pagination,
        searchForm,
        tableColumns,
        SearchFormOptions,
        editFormDataRef,
        editFormDataRule,
        typeOptions,
        aliInfo,
        tyInfo,
        canvasRef,
        qrData,
        onSearch,
        onResetSearch,
        onAddItem,
        doRefresh,
        onConfirm,
        modalDialog,
        editForm,
        onRowCheck,
        onCookieTypeChange,
        onTyPwdKeyUp,
        onlyAllowNumber: (value: string) => !value || /^\d+$/.test(value),
        getTokenByQrAli,
      }
    },
  })
</script>

<style lang="scss" scoped>
  .canvas {
    width: 115px !important;
    height: 115px !important;
  }
</style>
