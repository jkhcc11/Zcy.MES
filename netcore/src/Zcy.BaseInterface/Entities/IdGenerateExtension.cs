using Snowflake.Core;
using System;

namespace Zcy.BaseInterface.Entities
{
    public class IdGenerateExtension
    {
        private readonly IdWorker _idWorker;

        // 静态变量，用于保持对单例实例的引用
        private static IdGenerateExtension? _instance;
        public IdGenerateExtension(IdWorker idWorker)
        {
            _idWorker = idWorker;

            // 设置静态实例为当前实例
            _instance = this;
        }

        /// <summary>
        /// 生成Id
        /// </summary>
        /// <returns></returns>
        public long GenerateId()
        {
            return _idWorker.NextId();
        }

        /// <summary>
        /// 公共静态方法用于获取单例实例
        /// </summary>
        public static IdGenerateExtension Instance
        {
            get
            {
                if (_instance == null)
                {
                    throw new InvalidOperationException("IdGenerateExtension has not been initialized.");
                }

                return _instance;
            }
        }
    }
}
