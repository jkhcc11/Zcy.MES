using Zcy.BaseInterface.BaseModel;

namespace Zcy.Dto.Products
{
    /// <summary>
    /// 查询有效的产品分类
    /// </summary>
    public class QueryValidProductTypeDto : BaseEntityDto<long>
    {
        public QueryValidProductTypeDto(string typeName)
        {
            TypeName = typeName;
        }

        /// <summary>
        /// 分类名
        /// </summary>
        public string TypeName { get; set; }
    }
}
