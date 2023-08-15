using Microsoft.Data.SqlClient;
using Thực_tập_tuần_1.Models;

namespace Thực_tập_tuần_1.IServices
{
    public interface IWorkServices
    {
        public List<Work> GetAllWork();
        public List<Work> GetWorksPage(string sortProperty, SortOrder sortOder, string searchString = "", int status = 0); // get all
        public Work GetWorkById(Guid id);
        public List<Work> GetWorkByName(string name);
        public bool CreateWork(Work work);
        public bool UpdateWork(Work work);
        public bool DeleteWork(Guid id);
    }
}
