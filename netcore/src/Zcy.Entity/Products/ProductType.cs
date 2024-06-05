using Zcy.BaseInterface.Entities;
using Zcy.Entity.Company;

namespace Zcy.Entity.Products
{
    /// <summary>
    /// 产品分类
    /// </summary>
    public class ProductType : BaseEntity<long>, IBaseCompany
    {
        public const int TypeNameLength = 10;

        public ProductType(string typeName)
        {
            TypeName = typeName;
            TypeStatus = PublicStatusEnum.Normal;
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
        /// 类型状态
        /// </summary>
        public PublicStatusEnum TypeStatus { get; protected set; }

        /// <summary>
        /// 禁用
        /// </summary>
        public void Ban()
        {
            TypeStatus = PublicStatusEnum.Ban;
        }

        public void Open()
        {
            TypeStatus = PublicStatusEnum.Normal;
        }
    }
}
