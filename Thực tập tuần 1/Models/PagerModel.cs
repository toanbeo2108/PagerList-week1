using Microsoft.AspNetCore.Mvc.Rendering;

namespace Thực_tập_tuần_1.Models
{
    public class PagerModel
    {
        public int TotalItems { get; private set; }
        public int CurrentPage { get; private set; } // trang hiện tại
        public int PageSize { get; private set; } // kích thước trang
        public int TotalPages { get; private set; } // tổng số trang
        public int StartPage { get; private set; } // trang bắt đầu
        public int EndPage { get; private set; }
        public int StartRecord { get; private set; }
        public int EndRecord { get; private set; }

        // public propertíes
        public string Action { get; set; } = "FinalTest";
        public string SearchString { get; set; }
        public string Sortexpression { get; set; }
        public int Status { get; set; }

        public PagerModel(int totalItems, int currentPage, int pageSize = 3)
        {
            this.TotalItems = totalItems;
            this.CurrentPage = currentPage;
            this.PageSize = pageSize;
            this.TotalPages = (int)Math.Ceiling((decimal)TotalItems / (decimal)PageSize);
            int totalPages = this.TotalPages;
            int startPage = currentPage - 3;
            int endPage = currentPage + 3;
            if (startPage <= 0)
            {
                endPage = endPage - (startPage - 1);
                startPage = 1;
            }
            if (endPage > totalPages)
            {
                endPage = totalPages;
                if (endPage > 10)
                {
                    startPage = endPage - 6;
                }
            }
            StartRecord = (currentPage -1) * PageSize + 1;
            EndRecord = StartRecord - 1 + pageSize;

            if (EndRecord > totalItems)
            {
                EndRecord = totalItems;
            }
            if (totalItems == 0)
            {
                StartRecord = 0;
                StartPage = 0;
                EndRecord = 0;
                CurrentPage = 0;
            }
            else
            {
                StartPage = startPage;
                EndPage = endPage;
            }
        }
        public List<SelectListItem> GetPageSize()
        {
            var pageSizes = new List<SelectListItem>();
            for (int i = 5; i < 50; i += 5)
            {
                if (i == this.PageSize)
                {
                    pageSizes.Add(new SelectListItem(i.ToString(), i.ToString(), true));
                }
                else
                {
                    pageSizes.Add(new SelectListItem(i.ToString(), i.ToString()));
                }
            }
            return pageSizes;
        }
        public List<SelectListItem> GetStatus()
        {
            var status = new List<SelectListItem>();
            for (int i = 0; i <= 4; i++)
            {
                 if(i == 1)
                {
                    status.Add(new SelectListItem("Cần làm", i.ToString(), true));
                }
                else if (i == 2)
                {
                    status.Add(new SelectListItem("Đang làm", i.ToString(), true));
                }
                else if (i == 3)
                {
                    status.Add(new SelectListItem("Đã hoàn thành", i.ToString(), true));
                }
                else
                {
                    status.Add(new SelectListItem("Tất cả", i.ToString(), true));
                }
            }
            return status;
        }
    }
}
