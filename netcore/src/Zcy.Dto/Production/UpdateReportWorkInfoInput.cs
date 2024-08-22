using System.ComponentModel.DataAnnotations;
using static Zcy.BaseInterface.AuthorizationConst;

namespace Zcy.Dto.Production
{
    /// <summary>
    /// 更新报工
    /// </summary>
    public class UpdateReportWorkInfoInput
    {
        [Range(9999999, long.MaxValue, ErrorMessage = "报工Id参数错误")]
        public long Id { get; set; }

        /// <summary>
        /// 工作量
        /// </summary>
        /// <remarks>
        ///  数量或时长
        /// </remarks>
        [Range(0.01, 99999999)]
        public decimal WordDuration { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(EntityConst.BaseRemarkLength)]
        public string? Remark { get; set; }
    }
}
