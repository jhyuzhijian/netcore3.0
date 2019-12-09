using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Tool.Base
{
    public class Page
    {
        public Page() { }
        /// <summary>
        /// 实例化page对象 适用手机端/PC端
        /// </summary>
        /// <param name="currPage">当前页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="totalRecords">总记录数</param>
        /// <param name="isFistPageZero">当前第一页是否为0</param>
        public Page(int currPage, int pageSize, int totalRecords, bool isFistPageZero = false)
        {
            Page pa = UntilPage.GetPageObject(currPage, pageSize, totalRecords);
            this.currentPage = pa.currentPage;
            this.pageRecords = pa.pageRecords;
            this.hasNextPage = pa.hasNextPage;
            this.hasPreviousPage = pa.hasPreviousPage;
            this.totalPages = pa.totalPages;
            this.totalRecords = pa.totalRecords;
        }
        /// <summary>
        /// 当前页码
        /// </summary>
        public int currentPage = 0;
        /// <summary>
        /// 每页记录
        /// </summary>
        public int pageRecords = 10;
        /// <summary>
        /// 是否有下一页
        /// </summary>
        public bool hasNextPage { get; set; }
        /// <summary>
        /// 是否有前一页
        /// </summary>
        public bool hasPreviousPage { get; set; }
        /// <summary>
        /// 页码总数
        /// </summary>
        public int? totalPages { get; set; }
        /// <summary>
        /// 总记录数
        /// </summary>
        public int? totalRecords { get; set; }
       
    }
}
