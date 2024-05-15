using System;

namespace Zcy.BaseInterface
{
    /// <summary>
    /// 自定义异常
    /// </summary>
    public class ZcyCustomException : SystemException
    {
        public ZcyCustomException(string msg) : base(msg)
        {

        }

        public ZcyCustomException(string msg, Exception inner) : base(msg, inner)
        {

        }
    }
}
