<template>
  <CommonQueryList
    ref="commonQueryListRef"
    :queryApi="userApi.query"
    :tableColumns="tableColumns"
    :isShowQuery="true"
    :searchFormOptions="SearchUserOptions"
    :defaultPageSize="20"
    :scrollX="1000"
    :submitLoading="submitLoading"
    :editDialogTilte="editTitle"
    :onDialogConfirm="onBtnClick.onSaveSubmit"
    editContentHeight="50vh"
  >
    <template #tableToolbar>
      <n-space>
        <n-button type="info" size="small" @click="onBtnClick.create"> 创建 </n-button>
      </n-space>
    </template>

    <template #showDialogContent>
      <DataForm
        ref="submitForm"
        :options="CreateUserFormOptions"
        :form-config="{
          labelWidth: 100,
          size: 'medium',
          labelAlign: 'right',
        }"
        preset="grid-item"
        v-if="optType != 'setRole'"
      />

      <DataForm
        ref="setRoleSubmitForm"
        :options="SetRoleFormOptions"
        :form-config="{
          labelWidth: 100,
          size: 'medium',
          labelAlign: 'right',
        }"
        preset="grid-item"
        v-if="optType == 'setRole'"
      />
    </template>
  </CommonQueryList>
</template>

<script lang="ts">
  import { post, sendDelete } from '@/api/http'
  import { userApi } from '@/api/url'
  import { TableActionModel, useRenderAction, useTable } from '@/hooks/table'
  import { useMessage, useDialog } from 'naive-ui'
  import { defineComponent, onBeforeMount, onMounted, ref } from 'vue'
  import { DataFormType, FormItem } from '@/types/components'
  import { SearchUserOptions, CreateUserFormOptions, SetRoleFormOptions } from './Data'
  import { renderSelect, renderTagByEnum } from '@/hooks/form'
  import { UserStatusEnum } from '@/store/types'
  import useUserStore from '@/store/modules/user'
  import useRoleCacheStore from '@/store/modules/system-role'
  import useCompanyCacheStore from '@/store/modules/company'
  export default defineComponent({
    name: 'EmployeeList',
    setup() {
      const useCompanyCache = useCompanyCacheStore()
      const useUser = useUserStore()
      const useRoleCache = useRoleCacheStore()
      const message = useMessage()
      const commonQueryListRef = ref()
      const naiveDialog = useDialog()

      //提交
      const submitLoading = ref(false)
      const submitForm = ref<DataFormType | null>(null)
      const setRoleSubmitForm = ref<DataFormType | null>(null)
      const editTitle = ref('新增')
      const optType = ref<'add' | 'setRole' | 'update'>('add')

      //表格
      const table = useTable()
      const tableColumns = [
        table.indexColumn,
        {
          title: '用户名',
          key: 'userName',
        },
        {
          title: '用户昵称',
          key: 'userNick',
        },
        {
          title: '状态',
          key: 'userStatus',
          render: (rowData: any) =>
            renderTagByEnum(
              rowData.userStatus,
              UserStatusEnum,
              {
                1: 'success',
                10: 'warning',
                11: 'error',
              },
              {
                size: 'small',
              }
            ),
        },
        {
          title: '联系方式',
          key: 'userPhone',
        },
        {
          title: '工号',
          key: 'userNo',
        },
        {
          title: '启用登录',
          key: 'isEnableLogin',
          render(rowData: any) {
            return rowData.isEnableLogin ? '是' : '否'
          },
        },
        // {
        //   title: '结算基数',
        //   key: 'baseSettlement',
        // },

        {
          title: '创建时间',
          key: 'createdTime',
          width: 110,
          ellipsis: {
            tooltip: true,
          },
        },
        {
          title: '修改时间',
          key: 'modifyTime',
          width: 110,
          ellipsis: {
            tooltip: true,
          },
        },
        {
          title: '操作',
          key: 'actions',
          fixed: 'right',
          width: 180,
          render: (rowData: any) => {
            const tempArray: TableActionModel[] = []

            tempArray.push({
              label: '编辑',
              type: 'info',
              onClick: onBtnClick.edit.bind(null, rowData),
            } as TableActionModel)

            if (rowData.isEnableLogin) {
              tempArray.push({
                label: '禁用',
                type: 'error',
                onClick: onBtnClick.ban.bind(null, rowData),
              } as TableActionModel)
            } else {
              tempArray.push({
                label: '启用',
                type: 'success',
                onClick: onBtnClick.ban.bind(null, rowData),
              } as TableActionModel)
            }

            if (rowData.userStatus != UserStatusEnum.正常) {
              tempArray.push({
                label: '删除',
                type: 'error',
                onClick: onBtnClick.delete.bind(null, rowData),
              } as TableActionModel)
            } else {
              if (useUser.isSuperAdmin) {
                tempArray.push({
                  label: '设置角色',
                  type: 'success',
                  onClick: onBtnClick.setRole.bind(null, rowData),
                } as TableActionModel)
              }

              tempArray.push({
                label: '离职',
                type: 'warning',
                onClick: onBtnClick.depart.bind(null, rowData),
              } as TableActionModel)

              tempArray.push({
                label: '重置密码',
                type: 'warning',
                onClick: onBtnClick.reset.bind(null, rowData),
              } as TableActionModel)
            }

            return useRenderAction(tempArray)
          },
        },
      ]

      const onBtnClick = {
        //创建
        create: function () {
          optType.value = 'add'
          editTitle.value = '新增'
          submitForm.value?.reset()
          commonQueryListRef.value?.showDialog()
        },
        //设置角色
        setRole: function (rowData: any) {
          optType.value = 'setRole'
          editTitle.value = '设置角色'
          SetRoleFormOptions.forEach((it: any) => {
            if (it.key == 'userId') {
              it.value.value = rowData['id'] || null
              return
            }

            it.value.value = rowData[it.key] || null
          })

          commonQueryListRef.value?.showDialog()
        },
        //编辑
        edit: function (rowData: any) {
          optType.value = 'update'
          editTitle.value = `${rowData.userNick} 编辑`

          CreateUserFormOptions.forEach((it: any) => {
            if (it.key == 'isEnableLogin') {
              it.value.value = rowData[it.key] || false
              return
            }

            it.value.value = rowData[it.key] || null

            if (it.key == 'userName' || it.key == 'companyId') {
              it.disabled.value = true
              return
            }
          })

          commonQueryListRef.value?.showDialog()
        },
        //删除
        delete: function (rowData: any) {
          naiveDialog.warning({
            title: '提示',
            content: '是否要删除此数据？',
            positiveText: '删除',
            onPositiveClick: () => {
              sendDelete({
                url: userApi.delete + '/' + rowData.id,
              })
                .then((res) => {
                  doRefresh()
                  message.success(res.msg)
                })
                .catch(console.log)
            },
          })
        },
        //提交保存
        onSaveSubmit: function () {
          if (submitForm.value?.validator() || setRoleSubmitForm.value?.validator()) {
            let pd = submitForm.value?.generatorParams()
            let postUrl = userApi.create
            let method = 'PUT'
            submitLoading.value = true
            if (optType.value == 'setRole') {
              postUrl = userApi.setUserRole
              pd = setRoleSubmitForm.value?.generatorParams()
              method = 'POST'
            } else if (optType.value == 'update') {
              postUrl = userApi.modify
              pd = submitForm.value?.generatorParams()
              pd.userId = pd.id
              method = 'POST'
            }

            let tempPd = {
              companyId: useUser.companyId,
              ...pd,
            }
            if (pd.companyId) {
              //超管
              tempPd = pd
            }

            post({
              url: postUrl,
              data: tempPd,
              method: method,
            })
              .then((res) => {
                message.success(res.msg)

                doRefresh()
                commonQueryListRef.value?.closeDialog()
              })
              .finally(() => {
                submitLoading.value = false
              })
          }
        },
        //启用|禁用
        ban: function (rowData: any) {
          sendDelete({
            url: userApi.banOrEnable + '/' + rowData.id,
          })
            .then((res) => {
              doRefresh()
              message.success(res.msg)
            })
            .catch(console.log)
        },
        //重置密码
        reset: function (rowData: any) {
          naiveDialog.warning({
            title: '提示',
            content: `是否要重置【${rowData.userNick}】此用户密码？`,
            positiveText: '重置',
            onPositiveClick: () => {
              post({
                url: userApi.resetPwd + '/' + rowData.id,
              })
                .then((res) => {
                  doRefresh()
                  message.success(res.msg)
                })
                .catch(console.log)
            },
          })
        },
        //离职
        depart: function (rowData: any) {
          naiveDialog.warning({
            title: '提示',
            content: `是否设置【${rowData.userNick}】此用户为离职状态？`,
            positiveText: '确定',
            onPositiveClick: () => {
              post({
                url: userApi.depart + '/' + rowData.id,
              })
                .then((res) => {
                  doRefresh()
                  message.success(res.msg)
                })
                .catch(console.log)
            },
          })
        },
      }

      //刷新
      function doRefresh() {
        commonQueryListRef.value?.doRefresh()
      }

      onBeforeMount(() => {
        if (useUser.isSuperAdmin) {
          tableColumns.splice(8, 0, {
            title: '角色',
            key: 'roles',
            render(rowData: any) {
              if (rowData.roles) {
                return rowData.roles.join(',')
              }
            },
          })
        }
      })

      onMounted(async () => {
        if (useUser.isSuperAdmin) {
          await useCompanyCache.initCompanyData()
          await useRoleCache.init()

          const existingItemIndex = CreateUserFormOptions.findIndex(
            (item) => item.key === 'companyId'
          )
          if (existingItemIndex == -1) {
            CreateUserFormOptions.push({
              label: '所属公司',
              key: 'companyId',
              value: ref(null),
              span: 3,
              disabled: ref(false),
              reset(formItem: any) {
                formItem.value.value = null
                formItem.disabled.value = false
              },
              render: (formItem: any) => {
                return renderSelect(formItem.value, useCompanyCache.cachedItems, {
                  placeholder: '选择公司',
                  disabled: formItem.disabled.value,
                })
              },
            } as FormItem)
          }
        }
      })

      return {
        userApi,
        commonQueryListRef,
        tableColumns,
        SearchUserOptions,
        SetRoleFormOptions,
        CreateUserFormOptions,
        submitLoading,
        submitForm,
        setRoleSubmitForm,
        editTitle,
        optType,
        onBtnClick,
      }
    },
  })
</script>
