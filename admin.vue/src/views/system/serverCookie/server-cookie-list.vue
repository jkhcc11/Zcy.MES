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
            <n-tooltip placement="bottom" trigger="hover">
              <template #trigger>
                <AddButton @add="onAddItem" />
              </template>
              <span> 除天翼系列外，其他不用添加此信息 </span>
            </n-tooltip>
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
      @confirm="onConfirm"
      contentHeight="40vh"
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
            <n-form-item-gi label="子账号" path="subAccountId" :span="2">
              <n-select
                placeholder="请选择子账号"
                filterable
                :options="subAccountOptions"
                v-model:value="editFormDataRef.subAccountId"
                :on-update:value="onSubAccountChange"
              />
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
            <n-form-item-gi label="Ip" :span="2" path="serverIp">
              <n-input placeholder="解析服务器Ip" v-model:value="editFormDataRef.serverIp" />
            </n-form-item-gi>
            <n-form-item-gi label="Cookie" :span="2" path="cookieInfo">
              <n-input
                placeholder="服务器Cookie"
                type="textarea"
                v-model:value="editFormDataRef.cookieInfo"
              />
            </n-form-item-gi>
          </n-grid>
        </n-form>

        <!-- <DataForm ref="editForm" :options="EditFormOptions" :form-config="formConfig" /> -->
      </template>
    </ModalDialog>
  </div>
</template>

<script lang="ts">
  import { get, post, sendDelete } from '@/api/http'
  import { renderTagByEnum } from '@/hooks/form'
  import { parseSelfApi, serverCookieApi } from '@/api/url'
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
  import { ServerCookieStatusEnum } from '@/store/types'
  import { SearchFormOptions } from './Data'
  import Axios from 'axios'
  import useSubAccountListStore from '@/store/modules/sub-account-list'
  import useUserParseConfigStore from '@/store/modules/user-parse-config'
  export default defineComponent({
    name: 'ServerCookie',
    setup() {
      const formParams = ref<any>({})
      const submitLoading = ref(false)
      const naiveDialog = useDialog()
      const useSubAccountList = useSubAccountListStore()
      const userParseConfigStore = useUserParseConfigStore()

      //新增或编辑
      const modalDialog = ref<ModalDialogType | null>(null)
      const editForm = ref<typeof NForm | null>(null)
      const editFormDataRef = ref<any>({
        subAccountId: null,
        serverIp: null,
        cookieInfo: null,
        id: null,
      })
      const subAccountOptions = ref<any[]>([])
      const editFormDataRule = {
        subAccountId: {
          required: true,
          message: '请选择子账号',
          trigger: ['change'],
        },
        serverIp: {
          required: true,
          trigger: ['blur', 'change'],
          message: '请输入服务器IP',
        },
        cookieInfo: {
          required: true,
          trigger: ['blur', 'change'],
          message: '请输入服务器Cookie',
        },
      } as FormRules
      const tyInfo = reactive({
        tyUserName: '',
        tyPwd: '',
        isShow: false,
      })

      //表格
      const searchForm = ref<DataFormType | null>(null)
      const pagination = usePagination(doRefresh)
      // pagination.pageSize = 10
      const table = useTable()
      const message = useMessage()
      const rowKey = useRowKey('id')
      const checkedRowKeys = [] as Array<any>
      const tableColumns = useTableColumn(
        [
          //table.selectionColumn,
          //table.indexColumn,
          {
            type: 'expand',
            fixed: 'left',
            renderExpand: (rowData) => {
              return h('div', {
                innerHTML: `<strong>Cookie：</strong>${rowData.cookieInfo}`,
              })
            },
          },
          {
            title: '子账号别名',
            fixed: 'left',
            key: 'subAccountAlias',
          },
          {
            title: '服务器Ip',
            key: 'serverIp',
          },
          {
            title: 'Cookie',
            key: 'cookieInfo',
            width: 100,
            ellipsis: true,
            // ellipsis: {
            //   tooltip: true,
            // },
          },
          {
            title: '状态',
            key: 'serverCookieStatus',
            render: (rowData) =>
              renderTagByEnum(rowData.serverCookieStatus, ServerCookieStatusEnum, {
                5: 'success',
              }),
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
              const actionArray = [
                {
                  label: '编辑',
                  onClick: onUpdateItem.bind(null, rowData),
                },
                {
                  label: '删除',
                  type: 'error',
                  onClick: onDeleteItem.bind(null, rowData),
                },
              ] as TableActionModel[]
              if (rowData.serverCookieStatus == ServerCookieStatusEnum.初始化) {
                actionArray.push({
                  label: '审批',
                  type: 'warning',
                  onClick: onAuditItem.bind(null, rowData),
                } as TableActionModel)
              }

              const normalAction = useRenderAction(actionArray)
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
          url: serverCookieApi.query,
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
            if (key == 'serverIp') {
              return
            }

            editFormDataRef.value[key] = null
          })
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
        // EditFormOptions.forEach((it: any) => {
        //   it.value.value = item[it.key] || null
        // })
        modalDialog.value?.show()
      }

      //审批
      function onAuditItem(item: any) {
        naiveDialog.warning({
          title: '提示',
          content: `是否确认审批别名：${item.subAccountAlias} 此条数据？`,
          positiveText: '确认',
          onPositiveClick: () => {
            post({
              url: serverCookieApi.audit + '/' + item.id,
            })
              .then((res) => {
                doRefresh()
                message.success(res.msg)
              })
              .catch(console.log)
          },
        })
      }

      //删除
      function onDeleteItem(item: any) {
        const pd = {
          ids: [item.id],
        }
        naiveDialog.warning({
          title: '提示',
          content: `是否确认删除别名：${item.subAccountAlias} 此条数据？`,
          positiveText: '删除',
          onPositiveClick: () => {
            sendDelete({
              url: serverCookieApi.delete,
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
              .catch(console.log)
          },
        })
      }

      function onConfirm() {
        editForm.value?.validate((errors: any) => {
          if (!errors) {
            //console.log('pd', editFormDataRef.value)
            //message.success('验证成功')
            const pd = editFormDataRef.value
            submitLoading.value = true
            post({
              url: serverCookieApi.createAndUpdate,
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
              .finally(() => {
                submitLoading.value = false
              })
          }
        })
      }

      //子账号切换
      function onSubAccountChange(newVal: string) {
        editFormDataRef.value.subAccountId = newVal
        tyInfo.isShow = useSubAccountList.isTyTypeBySubAccountId(newVal)
        if (tyInfo.isShow == false) {
          tyInfo.tyUserName = ''
          tyInfo.tyPwd = ''
        }
      }

      //天翼pwd回车
      async function onTyPwdKeyUp(e: any) {
        if (e.key === 'Enter') {
          if (!userParseConfigStore.userParseConfig.customUrl) {
            message.warning('请前往个人中心先设置Api地址')
            return
          }

          if (tyInfo.tyUserName && tyInfo.tyPwd) {
            submitLoading.value = true

            //原生请求
            const service = Axios.create({ timeout: 10 * 60 * 1000 })
            service
              .post(userParseConfigStore.userParseConfig.customUrl + parseSelfApi.tyLogin, {
                userName: tyInfo.tyUserName,
                pwd: tyInfo.tyPwd,
                pageLoadType: 'none',
              })
              .then((response) => {
                //console.log('result', response)
                if (response.data.isSuccess) {
                  editFormDataRef.value.cookieInfo = response.data.data
                } else {
                  message.error(response.data.msg)
                }
              })
              .finally(() => {
                submitLoading.value = false
              })
            //请求解析api的
            // const result = await post({
            //   url: cloudDiskApi.loginWithTyPerson,
            //   data: {
            //     userName: tyInfo.tyUserName,
            //     pwd: tyInfo.tyPwd,
            //     pageLoadType: 'none',
            //   },
            // })

            //console.log('result', userParseConfigStore.userParseConfig.customUrl)
            //editFormDataRef.value.cookie = result.data

            //console.log('info', tyInfo.value.tyUserName + tyInfo.value.tyPwd)
          }
        }
      }

      onMounted(async () => {
        table.tableHeight.value = await useTableHeight()
        subAccountOptions.value = useSubAccountList.subAccountList
        doRefresh()
        await useSubAccountList.init()
        await userParseConfigStore.init()
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
        subAccountOptions,
        editFormDataRule,
        editForm,
        tyInfo,
        onSearch,
        onResetSearch,
        onAddItem,
        doRefresh,
        onConfirm,
        modalDialog,
        onRowCheck,
        onSubAccountChange,
        onTyPwdKeyUp,
        onlyAllowNumber: (value: string) => !value || /^\d+$/.test(value),
      }
    },
  })
</script>
