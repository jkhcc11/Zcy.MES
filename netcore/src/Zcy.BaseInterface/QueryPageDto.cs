﻿using System.Collections.Generic;

namespace Zcy.BaseInterface
{
    /// <summary>
    /// 排序入参 接口
    /// </summary>
    public interface IZcySortInput
    {
        /// <summary>
        /// 排序
        /// </summary>
        List<ZcyOrderConditions>? OrderBy { get; set; }
    }

    public class QueryPageDto<TDto>
    {
        /// <summary>
        /// 总数
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// 列表
        /// </summary>
        public IReadOnlyList<TDto> Items { get; set; } = new List<TDto>();
    }

    public abstract class QueryPageInput: IZcySortInput
    {
        protected const int MaxPageSize = 1000;
        private int _pageSize = 10;

        /// <summary>
        /// 分页大小
        /// </summary>
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
        }

        /// <summary>
        /// 页
        /// </summary>
        public int Page { get; set; } = 1;

        /// <summary>
        /// 排序
        /// </summary>
        public List<ZcyOrderConditions>? OrderBy { get; set; }
    }
}
