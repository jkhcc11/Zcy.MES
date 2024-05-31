<template>
  <div class="main-container">
    <TableBody>
      <template #header>
        <TableHeader :show-filter="false">
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
    </TableBody>
    <ModalDialog
      ref="modalDialog"
      @confirm="onConfirm"
      content-height="50vh"
      :submitLoading="submitLoading"
    >
      <template #content>
        <n-tree
          ref="refTree"
          :data="useAllMenuTree.getTreeData"
          checkable
          block-line
          cascade
          :default-expanded-keys="useAllMenuTree.getAllOneMenuIds"
          :default-checked-keys="menuTreeInfo.checkedKeys"
        />
      </template>
    </ModalDialog>

    <ModalDialog ref="editModalDialog" @confirm="onEditConfirm" :submitLoading="submitLoading">
      <template #content>
        <DataForm
          ref="editForm"
          :options="EditRoleFormOptions"
          :form-config="{
            labelWidth: 100,
            size: 'medium',
            labelAlign: 'right',
          }"
        />
      </template>
    </ModalDialog>
  </div>
</template>

<script lang="ts">
  import { defineComponent, onMounted, reactive, ref } from 'vue'
  import { post, get, sendDelete } from '@/api/http'
  import { roleMenuApi } from '@/api/url'
  import {
    TableActionModel,
    useRenderAction,
    useRowKey,
    useTable,
    useTableColumn,
    useTableHeight,
  } from '@/hooks/table'
  import { useDialog, useMessage } from 'naive-ui'
  import { TableColumn } from 'naive-ui/lib/data-table/src/interface'
  import { DataFormType, ModalDialogType } from '@/types/components'
  import { EditRoleFormOptions } from './Data'
  import useAllMenuTreeStore from '@/store/modules/all-menu-tree'
  export default defineComponent({
    name: 'RoleIndex',
    setup() {
      const useAllMenuTree = useAllMenuTreeStore()
      const table = useTable()
      const naiveDialog = useDialog()
      const message = useMessage()
      const submitLoading = ref(false)
      const modalDialog = ref<ModalDialogType | null>(null)
      const refTree = ref<any>(null)

      //新增或编辑
      const editModalDialog = ref<ModalDialogType | null>(null)
      const editForm = ref<DataFormType | null>(null)

      //树
      const menuTreeInfo = reactive({
        checkedKeys: [] as any[],
        currentRoleFlag: [],
      })

      const rowKey = useRowKey('routeFlag')
      const tableColumns = useTableColumn(
        [
          {
            title: '显示名',
            key: 'roleShowName',
          },
          {
            title: '角色标识',
            key: 'roleName',
          },
          {
            title: '备注',
            key: 'remark',
          },
          {
            title: '操作',
            key: 'actions',
            render: (rowData) => {
              return useRenderAction([
                {
                  label: '菜单权限',
                  type: 'success',
                  onClick: onShowMenu.bind(null, rowData),
                },
                {
                  label: '编辑',
                  type: 'info',
                  onClick: onEdit.bind(null, rowData),
                },
                {
                  label: '删除',
                  type: 'error',
                  onClick: onDelete.bind(null, rowData),
                },
              ] as TableActionModel[])
            },
          },
        ],
        {
          align: 'center',
        } as TableColumn
      )

      //添加
      function onAddItem() {
        editModalDialog.value?.show().then(() => {
          editForm.value?.reset()
        })
      }

      //编辑
      function onEdit(item: any) {
        EditRoleFormOptions.forEach((it: any) => {
          it.value.value = item[it.key] || null
        })
        editModalDialog.value?.show()
      }

      //删除
      function onDelete(item: any) {
        naiveDialog.warning({
          title: '提示',
          content: `是否确认删除：${item.roleName} 此条数据？`,
          positiveText: '删除',
          onPositiveClick: () => {
            sendDelete({
              url: roleMenuApi.deleteRole + '/' + item.id,
              data: {},
            })
              .then((res) => {
                if (res.isSuccess) {
                  doRefresh()
                  message.success(res.msg)
                } else {
                  message.error(res.msg)
                }
              })
              .catch(console.log)
          },
        })
      }

      //新增|修改提交
      function onEditConfirm() {
        if (!editForm.value?.validator()) {
          return
        }

        const pd = editForm.value?.generatorParams()
        submitLoading.value = true
        post({
          url: roleMenuApi.createAndUpdateRole,
          data: pd,
        })
          .then((res) => {
            if (res.isSuccess) {
              doRefresh()
              message.success(res.msg)
              editModalDialog.value?.close()
            } else {
              message.error(res.msg)
            }
          })
          .finally(() => {
            submitLoading.value = false
          })
      }

      //更新
      function doRefresh() {
        // var tempData = {
        //   data: getListByEnum(SysRoleNameEnum).map((item: any) => {
        //     return {
        //       roleName: item.label,
        //       routeFlag: item.value,
        //     }
        //   }),
        // }
        // table.handleSuccess(tempData)

        get({
          url: roleMenuApi.queryRole,
          data: () => {
            return {
              page: 1,
              pageSize: 50,
            }
          },
        })
          .then((res) => {
            //请求成功处理
            table.handleSuccess(res.data)
          })
          .catch(console.log)
      }

      //激活菜单处理 只需要子节点
      function handleActivatedMenu(menuItems: any[]) {
        //生成树
        const buildTree = (items, parentId = '0') => {
          return items
            .filter((item) => item.parentMenuId === parentId)
            .map((item) => ({ ...item, children: buildTree(items, item.menuId) }))
        }

        //提取子节点id
        const extractIds = (node) => {
          // If a node has children, get the ids from the children only.
          if (node.children && node.children.length > 0) {
            return node.children.flatMap((child) => extractIds(child))
          }

          // If a node doesn't have children, return its menuId.
          return [node.menuId]
        }

        // Build the tree starting from parentMenuId "0"
        const tree = buildTree(menuItems)

        // Extract all menuIds from the tree. This will start from the root nodes.
        const childrenMenuIds = tree.flatMap((rootNode) => extractIds(rootNode))
        return childrenMenuIds
      }

      //角色权限
      function onShowMenu(item: any) {
        menuTreeInfo.checkedKeys.length = 0
        menuTreeInfo.currentRoleFlag = item.id
        submitLoading.value = true

        get({
          url: roleMenuApi.getRoleActivateMenu + '/' + item.id,
          data: {},
        })
          .then((res) => {
            //[{"parentMenuId":"0","menuId":"1772631108559704064","menuName":"Dashboard"}]

            // const tempData = res.data.map((item) => {
            //   return item.menuId
            // })
            const tempData = handleActivatedMenu(res.data)
            menuTreeInfo.checkedKeys.push(...tempData)
            // console.log(res)
            // menuData.length = 0
            // menuData.push(...handleMenuData(res.data, defaultCheckedKeys, defaultExpandedKeys))
          })
          .catch(console.log)
          .finally(() => {
            submitLoading.value = false
          })

        modalDialog.value?.show()
      }

      //确认提交
      function onConfirm() {
        //提交
        submitLoading.value = true

        //获取选中的和半选的节点
        const checkedKeys = refTree.value?.getCheckedData().keys
        const indeterminateKeys = refTree.value?.getIndeterminateData().keys
        // console.log(
        //   'treeData',
        //   refTree.value?.getCheckedData(),
        //   refTree.value?.getIndeterminateData()
        // )
        // return

        post({
          url: roleMenuApi.createAndUpdateRoleMenu,
          data: {
            roleId: menuTreeInfo.currentRoleFlag,
            menuItems: [...checkedKeys, ...indeterminateKeys].map((item) => {
              return {
                menuId: item,
              }
            }),
          },
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

      //   //树更新
      //   function updateCheckedKeys(
      //     keys: Array<string | number>,
      //     options: Array<TreeOption | null>,
      //     meta: {
      //       node: TreeOption | null
      //       action: 'check' | 'uncheck'
      //     }
      //   ) {
      //     menuTreeInfo.checkedKeys.length = 0
      //     menuTreeInfo.checkedKeys.push(...keys)
      //     console.log('updateCheckedKeys', menuTreeInfo.indeterminateKeys, keys, options, meta)
      //   }

      onMounted(async () => {
        table.tableHeight.value = await useTableHeight()
        await useAllMenuTree.init()
        doRefresh()
      })

      return {
        refTree,
        rowKey,
        modalDialog,
        menuTreeInfo,
        ...table,
        submitLoading,
        tableColumns,
        useAllMenuTree,
        editModalDialog,
        editForm,
        EditRoleFormOptions,
        doRefresh,
        onConfirm,
        onEditConfirm,
        onAddItem,
        //updateCheckedKeys,
      }
    },
  })
</script>
