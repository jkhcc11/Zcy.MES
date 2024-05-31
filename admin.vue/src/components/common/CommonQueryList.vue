<template>
  <div class="main-container">
    <n-spin :show="submitLoading">
      <TableBody ref="tableBody">
        <template #header>
          <TableHeader
            :show-filter="isShowQuery"
            :isExpansion="isExpansion"
            @search="onSearch"
            @reset-search="onResetSearch"
          >
            <template #table-config>
              <!-- 表格右边工具 -->
              <slot name="tableToolbarRight"></slot>
              <SortableTable class="ml-4" :columns="localTableColumns" @update="onUpdateTable" />
            </template>
            <template #search-content>
              <DataForm
                ref="searchForm"
                :options="searchFormOptions"
                preset="grid-item"
                :form-config="{
                  labelWidth: 70,
                  size: 'small',
                }"
              />
            </template>
            <template #top-toolbar>
              <!-- 表格工具区域 -->
              <slot name="tableToolbar"></slot>
              <slot name="topSummary"></slot>
            </template>
          </TableHeader>
        </template>
        <template #default>
          <n-data-table
            size="small"
            :loading="tableLoading"
            :data="dataList"
            :columns="localTableColumns"
            :row-key="tableRowKey"
            :style="{ height: `${tableHeight}px` }"
            :flex-height="true"
            :checked-row-keys="checkedRowKeys"
            :scroll-x="scrollX"
            @update:checked-row-keys="onRowCheck"
            @update:sorter="handleUpdateSorter"
          />
        </template>
        <template #footer>
          <TableFooter :pagination="pagination" :showRefresh="false" />
        </template>
      </TableBody>

      <ModalDialog
        ref="modalDialogRef"
        @confirm="onDialogConfirm"
        :maskClosable="false"
        :isClosable="true"
        :title="editDialogTilte"
        :submit-loading="submitLoading"
        :contentHeight="editContentHeight"
        :contentWidth="editContentWidth"
      >
        <template #content>
          <!-- 弹窗内容 -->
          <slot name="showDialogContent"></slot>
          <!-- <DataForm
            ref="editForm"
            :options="editFormOptions"
            :form-config="formConfig"
            preset="grid-item"
          /> -->
        </template>
      </ModalDialog>

      <!-- 隐藏组件 -->
      <div style="display: none">
        <slot name="hideElement"></slot>
      </div>
    </n-spin>
  </div>
</template>

<script lang="ts">
  import { get, post, sendDelete } from '@/api/http'
  import { usePagination, useRowKey, useTable, useTableColumn, useTableHeight } from '@/hooks/table'
  import { DataFormType, FormItem, ModalDialogType, TablePropsType } from '@/types/components'
  import { findByRemove, toOrderBy, sortColumns } from '@/utils'
  import { DataTableColumn, FormProps, useMessage } from 'naive-ui'
  import { defineComponent, ref, toRef, onMounted, reactive } from 'vue'
  export default defineComponent({
    //通用表格选择组件
    name: 'CommonQueryList',
    props: {
      queryApi: String,
      /**
       * 表格列
       */
      tableColumns: Array<DataTableColumn>,
      /**
       * 弹窗确认回调
       */
      onDialogConfirm: Function,
      /**
       * 搜索配置
       */
      searchFormOptions: Array<FormItem>,
      /**
       * 提交中 用于loading
       */
      submitLoading: Boolean,
      /**
       * 是否展示查询
       */
      isShowQuery: Boolean,
      /**
       * 固定列时 需要 eg:1800
       */
      scrollX: Number,
      /**
       * 表格列唯一标识 默认为id
       */
      rowKey: {
        type: String,
        defalut: 'id',
      },
      /**
       * 编辑弹窗高度 默认30vh
       */
      editContentHeight: {
        type: String,
        defalut: '30vh',
      },
      /**
       * 编辑弹窗宽度
       */
      editContentWidth: String,
      /**
       * 编辑弹窗标题
       */
      editDialogTilte: {
        type: String,
        defalut: '操作',
      },
      /**
       * 默认分页大小 10
       */
      defaultPageSize: {
        type: Number,
        defalut: 10,
      },
      /**
       * 是否展开搜索
       */
      isExpansion: {
        type: Boolean,
        default: false,
      },
      /**
       * 表格格式
       */
      tableAlign: {
        type: String,
        defalut: 'center',
      },
    },
    emits: ['LoadDataSuccess'],
    setup(props, { emit }) {
      const message = useMessage()
      const queryApiRef = toRef(props, 'queryApi')
      const rowKeyRef = toRef(props, 'rowKey')
      const tableColumnsRef = toRef(props, 'tableColumns')
      const defaultPageSizeRef = toRef(props, 'defaultPageSize')
      const formParams = ref()
      const modalDialogRef = ref<ModalDialogType | null>(null)
      const tableAlignRef = toRef(props, 'tableAlign')

      //编辑
      const formConfig = {
        labelWidth: 100,
        size: 'medium',
        labelAlign: 'right',
      } as FormProps

      //搜索
      const searchForm = ref<DataFormType | null>(null)

      //表格
      const tableLoading = ref(true)
      const pagination = usePagination(doRefresh)
      pagination.pageSize = defaultPageSizeRef.value ?? 10
      const table = useTable()
      const tableRowKey = useRowKey(rowKeyRef.value ?? 'id')
      const checkedRowKeys = reactive<any[]>([])
      const checkedRows = reactive<any[]>([])
      //如果要动态列  需要使用reactive 做响应式
      const localTableColumns = reactive(
        useTableColumn(tableColumnsRef.value ?? [], {
          align: tableAlignRef.value,
        } as DataTableColumn)
      )

      //排序
      const sortStatesRef = reactive<any[]>([])

      //刷新
      function doRefresh() {
        //清空选中
        checkedRowKeys.length = 0
        tableLoading.value = true
        const params = searchForm.value?.generatorParams()
        if (params) {
          formParams.value = params
        }

        let pd: any = {
          ...formParams.value,
        }

        let tempSorts: any[] = []
        //排序
        sortStatesRef.forEach((item) => {
          tempSorts.push(`${item.columnKey} ${toOrderBy(item.order)}`)
        })
        pd.sorts = tempSorts

        get({
          url: queryApiRef.value ?? '',
          data: () => {
            return {
              ...pd,
              page: pagination.page,
              pageSize: pagination.pageSize,
            }
          },
        })
          .then((res) => {
            //请求成功处理
            emit('LoadDataSuccess', res.data.items)
            table.handleSuccess(res.data)
            pagination.setTotalSize(res.data.total)
          })
          .catch(console.log)
          .finally(() => {
            tableLoading.value = false
          })
      }

      //选中
      function onRowCheck(rowKeys: Array<any>, rows: object[]) {
        checkedRowKeys.length = 0
        checkedRowKeys.push(...rowKeys)

        checkedRows.length = 0
        checkedRows.push(...rows)
      }

      //搜索
      function onSearch() {
        pagination.page = 1
        doRefresh()
      }

      //重置查询
      function onResetSearch() {
        pagination.page = 1
        searchForm.value?.reset()
        doRefresh()
      }

      //清除选中
      function clearSelected() {
        checkedRows.length = 0
        checkedRowKeys.length = 0
      }

      //排序
      function handleUpdateSorter(sorters: any) {
        if (Array.isArray(sorters)) {
          //多排序
          sortStatesRef.length = 0
          sorters.forEach((item) => {
            if (!item.order) {
              return false
            }

            sortStatesRef.push(item)
          })
        } else {
          //单排序
          findByRemove(sortStatesRef, (item) => item.columnKey == sorters.columnKey)
          if (!sorters.order) {
            //取消排序
            return
          }

          sortStatesRef.push(sorters)
        }

        doRefresh()
        //console.log('sortStatesRef', sortStatesRef)
      }

      //列表更新
      function onUpdateTable(newColumns: Array<TablePropsType>) {
        sortColumns(localTableColumns, newColumns)
      }

      //展示窗口
      function showDialog() {
        modalDialogRef.value?.show()
      }
      //关闭窗口
      function closeDialog() {
        modalDialogRef.value?.close()
      }

      //获取选中行信息
      function getCheckedRow() {
        return {
          checkedRowKeys: checkedRowKeys,
          checkedRows: checkedRows,
        }
      }

      //发送批量操作请求
      function sendBatchByIds(
        type: 'delete' | 'post',
        url: string,
        ids: string[],
        postData: any | null
      ) {
        let pd = {
          ids: ids,
        }
        if (postData) {
          pd = postData
        }

        tableLoading.value = true
        if (type == 'delete') {
          sendDelete({
            url: url,
            data: () => pd,
          })
            .then((res) => {
              message.success(res.msg)
              doRefresh()
            })
            .finally(() => {
              tableLoading.value = false
            })
        } else {
          post({
            url: url,
            data: () => pd,
          })
            .then((res) => {
              message.success(res.msg)
              doRefresh()
            })
            .finally(() => {
              tableLoading.value = false
            })
        }
      }

      // //下载文件
      // function ondownloadFile(url: string, data: any) {
      //   tableLoading.value = true
      //   if (!data && formParams.value) {
      //     data = formParams.value
      //   }
      //   downloadFile({
      //     url,
      //     data: data,
      //   })
      //     .then((response) => {
      //       //不要启用mock
      //       const { headers, data } = response
      //       const resData = new Blob([data])
      //       var headerName = headers['content-disposition']
      //       var keyFlag = "UTF-8''",
      //         keyIndex = headerName.indexOf(keyFlag),
      //         newName = decodeURIComponent(headerName).substring(20)
      //       if (keyIndex !== -1) {
      //         newName = headerName.substring(keyIndex + keyFlag.length)
      //       }

      //       var fileName = decodeURIComponent(newName)

      //       const link = document.createElement('a')
      //       link.href = window.URL.createObjectURL(resData)
      //       link.setAttribute('download', fileName) // 设置下载的文件名
      //       //document.body.appendChild(link)
      //       link.click()
      //       URL.revokeObjectURL(resData)
      //     })
      //     .finally(() => {
      //       tableLoading.value = false
      //     })
      // }

      /**
       * 获取查询参数
       */
      function getQueryParams() {
        return formParams.value
      }

      onMounted(async () => {
        table.tableHeight.value = await useTableHeight()
        doRefresh()
      })

      return {
        ...table,
        tableLoading,
        tableRowKey,
        localTableColumns,
        pagination,
        formConfig,
        checkedRowKeys,
        checkedRows,
        modalDialogRef,
        searchForm,
        onRowCheck,
        onSearch,
        onResetSearch,
        clearSelected,
        handleUpdateSorter,
        onUpdateTable,
        showDialog, //给父组件使用
        closeDialog,
        doRefresh,
        getCheckedRow,
        sendBatchByIds,
        getQueryParams,
      }
    },
  })
</script>
