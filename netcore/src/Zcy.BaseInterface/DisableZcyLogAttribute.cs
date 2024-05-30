using System;

namespace Zcy.BaseInterface
{
    /// <summary>
    /// 禁用日志
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class DisableZcyLogAttribute : Attribute
    {

    }
}
