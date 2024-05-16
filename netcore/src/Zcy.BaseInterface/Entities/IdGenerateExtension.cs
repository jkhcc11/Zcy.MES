using Snowflake.Core;
using System;

namespace Zcy.BaseInterface.Entities
{
    public class IdGenerateExtension
    {
        private readonly IdWorker _idWorker;
        public IdGenerateExtension(IdWorker idWorker)
        {
            _idWorker = idWorker;
        }

        /// <summary>
        /// 生成Id
        /// </summary>
        /// <returns></returns>
        public long GenerateId()
        {
            return _idWorker.NextId();
        }
    }
}
