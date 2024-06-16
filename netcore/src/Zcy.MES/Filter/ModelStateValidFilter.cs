using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Zcy.BaseInterface.BaseModel;

namespace Zcy.MES.Filter
{
    /// <summary>
    /// ModelState 入参校验
    /// </summary>
    public class ModelStateValidFilter : IActionFilter
    {
        private readonly ILogger<ModelStateValidFilter> _logger;

        public ModelStateValidFilter(ILogger<ModelStateValidFilter> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 执行action后
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        /// <summary>
        /// 执行action前
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ModelState.IsValid)
            {
                return;
            }

            // 校验参数失败
            var errorMessages = context.ModelState.Values
                .SelectMany(e => e.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            var detailedErrorMessages = string.Join("; ", errorMessages);
            _logger.LogWarning("参数校验失败: {DetailedErrorMessages}", detailedErrorMessages);

            //校验参数失败
            var errorMsg = errorMessages
                .FirstOrDefault();
            var errMsg = string.IsNullOrWhiteSpace(errorMsg) ? "参数校验失败，请检查" : errorMsg;
            var result = KdyResult.Error(KdyResultCode.ParError, errMsg);
            context.Result = new OkObjectResult(result);
        }
    }
}
