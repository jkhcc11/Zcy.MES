//有效供应商|客户
export type GetValidSupplierClientDto = BaseEntityDto<string> & {
  //客户名
  clientName: string
}

//有效员工
export type GetCurrentCompanyValidEmployeeDto = BaseEntityDto<string> & {
  //员工名
  userNick: string
  //员工账号名
  userName: string
}
