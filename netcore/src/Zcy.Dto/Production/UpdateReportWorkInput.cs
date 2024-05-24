using System.ComponentModel.DataAnnotations;
using static Zcy.BaseInterface.AuthorizationConst;

namespace Zcy.Dto.Production
{
    /// <summary>
    /// 更新报工记录
    /// </summary>
    public class UpdateReportWorkInput
    {
        [Range(9999999, long.MaxValue, ErrorMessage = "报工Id参数错误")]
        public long Id { get; set; }

        /// <summary>
        /// 实结算价格
        /// </summary>
        [Range(0.01, 99999999)]
        public decimal? ActualSettlementPrice { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(EntityConst.BaseRemarkLength)]
        public string? Remark { get; set; }
    }
}
