using Zcy.BaseInterface.BaseModel;
using Zcy.BaseInterface.Entities;
using Zcy.Entity.Company;

namespace Zcy.Dto.Products
{
    /// <summary>
    /// 分页查询产品分类
    /// </summary>
    public class QueryPageProductTypeDto : BaseFullAuditEntityDto<long>,IBaseCompanyDto
    {
        public QueryPageProductTypeDto(string typeName)
        {
            TypeName = typeName;
        }

        /// <summary>
        /// 分类名
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// 公司Id
        /// </summary>
        public long CompanyId { get; set; }

        /// <summary>
        /// 公司名
        /// </summary>
        public string? CompanyName { get; set; }

        /// <summary>
        /// 类型状态
        /// </summary>
        public PublicStatusEnum TypeStatus { get; set; }
    }
}
