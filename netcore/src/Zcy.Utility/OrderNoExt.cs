namespace Zcy.Utility
{
    public static class OrderNoExt
    {
        /// <summary>
        /// 数字转订单号(不包含前缀的)
        /// </summary>
        /// <param name="noIndex">订单序号</param>
        /// <param name="orderNoLength">订单序号长度</param>
        /// <returns></returns>
        public static string ToOrderNo(this int noIndex, int orderNoLength)
        {
            var noStr = noIndex.ToString().PadLeft(orderNoLength, '0');
            return noStr;
            //var tempStr = CalculateCheckDigit(noStr);
            //return tempStr + noStr;
        }

        /// <summary>
        /// 计算订单号的校验位，使用的是Luhn算法。
        /// </summary>
        /// <param name="input">订单序号</param>
        /// <returns></returns>
        public static char CalculateCheckDigit(string input)
        {
            var sum = 0;
            var alternate = false;

            // 从右到左遍历字符串
            for (var i = input.Length - 1; i >= 0; i--)
            {
                var c = input[i];
                if (char.IsDigit(c) == false)
                {
                    continue;
                }

                var n = c - '0';
                if (alternate)
                {
                    n *= 2;
                    if (n > 9)
                    {
                        n -= 9;
                    }
                }
                sum += n;
                alternate = !alternate;
            }

            var mod10 = sum % 10;
            var checkDigit = (10 - mod10) % 10;

            return (char)('0' + checkDigit);
        }

        /// <summary>
        /// 校验订单号是否一致
        /// </summary>
        /// <param name="orderNumber">完整订单号</param>
        /// <param name="orderNoLength">订单序号长度</param>
        /// <returns></returns>
        public static bool ValidateOrderNo(string orderNumber, int orderNoLength)
        {
            // 前缀长度 (16-19) + 1位校验位 + 订单长度数字
            if (orderNumber.Length < (orderNoLength + 17) ||
                orderNumber.Length > (orderNoLength + 20))
            {
                return false;
            }

            //包含校验位的订单序号
            var orderNoIndexStr = orderNumber.Substring(orderNumber.Length - orderNoLength - 1);
            //原始订单序号
            var originalOrderNumber = orderNumber.Substring(0, orderNumber.Length - orderNoLength - 1);
            var providedCheckDigit = orderNoIndexStr[0];
            var expectedCheckDigit = CalculateCheckDigit(originalOrderNumber);
            return providedCheckDigit == expectedCheckDigit;
        }

        /// <summary>
        /// 获取订单号序号
        /// </summary>
        /// <param name="orderNumber">完整订单号</param>
        /// <param name="orderNoLength">订单序号长度</param>
        /// <returns></returns>
        public static int GetOrderNoIndex(this string orderNumber, int orderNoLength)
        {
            // 前缀长度 (2-4) + 1位校验位 + 订单长度数字
            if (orderNumber.Length < (orderNoLength + 3) ||
                orderNumber.Length > (orderNoLength + 5))
            {
                return 0;
            }

            //原始订单序号
            var originalOrderNumber = orderNumber.Substring(orderNumber.Length - orderNoLength);
            return originalOrderNumber.TryToInt32();
        }
    }
}
