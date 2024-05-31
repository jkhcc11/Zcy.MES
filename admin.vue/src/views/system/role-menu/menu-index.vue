<template>
  <div class="main-container">
    <TableBody>
      <template #header>
        <TableHeader :show-filter="true" @search="doRefresh" @reset-search="onResetSearch">
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
          :loading="tableLoading"
          :data="dataList"
          :row-key="rowKey"
          :columns="tableColumns"
        />
      </template>
      <template #footer>
        <TableFooter :pagination="pagination" :showRefresh="false" />
      </template>
    </TableBody>
    <ModalDialog
      ref="modalDialog"
      @confirm="onConfirm"
      content-height="50vh"
      :submitLoading="submitLoading"
    >
      <template #content>
        <DataForm
          ref="dataForm"
          :options="EditFormOptions"
          :form-config="{
            labelWidth: 100,
            size: 'medium',
            labelAlign: 'right',
          }"
          preset="grid-item"
        />
      </template>
    </ModalDialog>
  </div>
</template>

<script lang="ts">
  import { defineComponent, h, onMounted, ref } from 'vue'
  import { post, get, sendDelete } from '@/api/http'
  import { roleMenuApi } from '@/api/url'
  import {
    TableActionModel,
    useRenderAction,
    useRowKey,
    useTable,
    useTableColumn,
    usePagination,
    useTableHeight,
  } from '@/hooks/table'
  import { NIcon, useDialog, useMessage } from 'naive-ui'
  import { TableColumn } from 'naive-ui/lib/data-table/src/interface'
  import SvgIcon from '@/components/svg-icon/index.vue'
  import { DataFormType, ModalDialogType } from '@/types/components'
  import { SearchFormOptions, EditFormOptions } from './Data'
  import useAllMenuTreeStore from '@/store/modules/all-menu-tree'
  export default defineComponent({
    name: 'MenuIndex',
    setup() {
      const useAllMenuTree = useAllMenuTreeStore()
      const table = useTable()
      const naiveDialog = useDialog()
      const message = useMessage()
      const submitLoading = ref(false)
      const modalDialog = ref<ModalDialogType | null>(null)

      const pagination = usePagination(doRefresh)
      const formParams = ref<any>({})
      const searchForm = ref<DataFormType | null>(null)

      const dataForm = ref<DataFormType | null>(null)

      const rowKey = useRowKey('id')
      const tableColumns = useTableColumn(
        [
          {
            title: '菜单名称',
            key: 'menuName',
          },
          {
            title: '路由名',
            key: 'routeName',
          },
          {
            title: '菜单地址',
            key: 'menuUrl',
          },
          {
            title: '菜单图标',
            key: 'icon',
            render: (rowData) => {
              return rowData.iconPrefix === 'iconfont'
                ? h(SvgIcon, {
                    prefix: rowData.iconPrefix ? (rowData.iconPrefix as string) : 'iconfont',
                    name: rowData.icon ? (rowData.icon as string) : 'menu',
                  })
                : '无'
            },
          },
          {
            title: '是否缓存',
            key: 'isCache',
            render: (rowData) => {
              return h('div', null, { default: () => (rowData.isCache ? '是' : '否') })
            },
          },
          {
            title: '本地目录',
            key: 'localFilePath',
          },
          {
            title: '排序',
            key: 'orderBy',
          },
          // {
          //   title: '是否隐藏',
          //   key: 'hidden',
          //   render: (rowData) => {
          //     return h('div', null, { default: () => (rowData.hidden ? '是' : '否') })
          //   },
          // },
          // {
          //   title: '是否固定标题栏',
          //   key: 'affix',
          //   render: (rowData) => {
          //     return h('div', null, { default: () => (rowData.affix ? '是' : '否') })
          //   },
          // },
          {
            title: '操作',
            key: 'actions',
            render: (rowData) => {
              return useRenderAction([
                {
                  label: '编辑',
                  onClick: onUpdateItem.bind(null, rowData),
                },
                {
                  label: '删除',
                  type: 'error',
                  onClick: onDeleteItem.bind(null, rowData),
                },
              ] as TableActionModel[])
            },
          },
        ],
        {
          align: 'center',
        } as TableColumn
      )

      //更新
      function doRefresh() {
        const params = searchForm.value?.generatorParams()
        if (params) {
          formParams.value = params
        }

        get({
          url: roleMenuApi.query,
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
            pagination.setTotalSize(res.data.total)
          })
          .catch(console.log)
      }

      //重置查询
      function onResetSearch() {
        searchForm.value?.reset()
      }

      //添加
      function onAddItem() {
        modalDialog.value?.show().then(() => {
          dataForm.value?.reset()
        })
      }

      //更新
      function onUpdateItem(item: any) {
        EditFormOptions.forEach((it) => {
          it.value.value = item[it.key] || null
        })

        modalDialog.value?.show()
      }

      //确认提交
      function onConfirm() {
        if (!dataForm.value?.validator()) {
          return
        }

        const pd = dataForm.value?.generatorParams()
        if (pd.id && pd.parentMenuId === pd.id) {
          message.error('父节点不能选择自己')
          return
        }

        submitLoading.value = true
        post({
          url: roleMenuApi.createAndUpdate,
          data: pd,
        })
          .then((res) => {
            if (res.isSuccess) {
              doRefresh()
              message.success(res.msg)
              useAllMenuTree.refresh()
              modalDialog.value?.close()
            } else {
              message.error(res.msg)
            }
          })
          .finally(() => {
            submitLoading.value = false
          })
      }

      //删除
      function onDeleteItem(item: any) {
        const pd = {
          ids: [item.id],
        }

        naiveDialog.warning({
          title: '提示',
          content: `是否要删除【${item.menuName}】数据？`,
          positiveText: '删除',
          onPositiveClick: () => {
            sendDelete({
              url: roleMenuApi.delete,
              data: pd,
            })
              .then((res) => {
                if (res.isSuccess) {
                  doRefresh()
                  message.success(res.msg)
                  useAllMenuTree.refresh()
                  modalDialog.value?.close()
                } else {
                  message.error(res.msg)
                }
              })
              .catch(console.log)
          },
        })
      }

      onMounted(async () => {
        table.tableHeight.value = await useTableHeight()
        await useAllMenuTree.init()
        doRefresh()
      })

      return {
        rowKey,
        modalDialog,
        dataForm,
        searchForm,
        ...table,
        pagination,
        submitLoading,
        SearchFormOptions,
        EditFormOptions,
        tableColumns,
        doRefresh,
        onResetSearch,
        onAddItem,
        onDeleteItem,
        onConfirm,
      }
    },
  })
</script>
<style lang="scss" scoped>
  .icon-wrapper {
    list-style: none;
    border: 1px solid #f5f5f5;
    overflow: hidden;
    padding: 0;
    .icon-item {
      float: left;
      width: 25%;
      font-size: 26px;
      border-right: 1px solid #f5f5f5;
      border-bottom: 1px solid #f5f5f5;
      text-align: center;
      cursor: pointer;
      & > div {
        font-size: 12px;
      }
      &:hover {
        box-shadow: 0 2px 4px rgba(0, 0, 0, 0.12), 0 0 6px rgba(0, 0, 0, 0.04);
      }
    }
  }
</style>
