using System.Collections.Generic;

namespace Zcy.BaseInterface.BaseModel
{
    /// <summary>
    /// 批量操作 input
    /// </summary>
    public class BatchOperationsInput
    {
        public BatchOperationsInput(List<long> ids)
        {
            Ids = ids;
        }

        /// <summary>
        /// 主键集合
        /// </summary>
        public virtual List<long> Ids { get; set; } = new List<long>();
    }
}
