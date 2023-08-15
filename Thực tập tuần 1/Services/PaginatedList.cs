using Microsoft.EntityFrameworkCore;

namespace Thực_tập_tuần_1.Services
{
    public class PaginatedList<T> : List<T>
    {
        public int PageIndex { get; private set; } // số trang theo thứ tự
        public int ToTalPages { get; private set; } // tổng số trang
        // tạo contructor
        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            ToTalPages = (int)Math.Ceiling(count / (double)pageSize);
            // Math.Ceiling dùng để tìm số nguyên nhỏ nhất >= số đã truyền ví dụ Math.Ceiling(12.3) = 13
            this.AddRange(items);
        }
        public bool HasPreviousPage => PageIndex > 1; // chuyển về trang trước nếu số lượng trang > 1
        public bool HasNextPage => PageIndex < ToTalPages; // Chuyển sang trang sau nếu stt của trang < tổng số trang
        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }
    }
}
