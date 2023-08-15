using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Thực_tập_tuần_1.IServices;
using Thực_tập_tuần_1.Models;

namespace Thực_tập_tuần_1.Services
{
    public class WorkServices : IWorkServices
    {
        WorkDbContext _context;
        public WorkServices()
        {
            _context = new WorkDbContext();
        }

        public bool CreateWork(Work work)
        {
            try
            {
                _context.Works.Add(work);
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeleteWork(Guid id)
        {
            try
            {
                var work = _context.Works.Find(id);
                _context.Remove(work);
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<Work> GetAllWork()
        {
            return _context.Works.ToList();
        }

        public Work GetWorkById(Guid id)
        {
            return _context.Works.FirstOrDefault(x => x.IdWork == id);
        }

        public List<Work> GetWorkByName(string name)
        {
            return _context.Works.Where(p => p.Name == name).ToList();
        }
        private List<Work> DoSort(List<Work> works,string sortProperty, SortOrder sortOder) // hàm thực hiện sắp xếp
        {
            if (sortProperty.ToLower() == "name")   // nếu thuộc tính sắp xếp == name thì sét xem sắp xếp theo tăng hay giảm
            {
                if (sortOder == SortOrder.Ascending)    // nếu tăng dần
                {
                    works = works.OrderBy(p => p.Name).ToList();
                }
                else // ngược lại thì giảm dần
                {
                    works = works.OrderByDescending(p => p.Name).ToList();
                }
            }
            else
            {
                if (sortOder == SortOrder.Ascending)
                {
                    works = works.OrderBy(p => p.WorkTime).ToList();
                }
                else // ngược lại thì giảm dần
                {
                    works = works.OrderByDescending(p => p.WorkTime).ToList();
                }
            }
            return works; // trả về
        }
        public List<Work> GetWorksPage(string sortProperty, SortOrder sortOder, string SearchString = "", int status = 0)
        {
            List<Work> works;
            if (String.IsNullOrEmpty(SearchString) == false)
            {
                works = _context.Works.Where(p => p.Name.ToLower().Contains(SearchString.ToLower())).ToList();
                switch (status) 
                {
                    case 1:
                        works = works.Where(p => p.Status == 1).ToList();
                        break;
                    case 2:
                        works = works.Where(p => p.Status == 2).ToList();
                        break;
                    case 3:
                        works = works.Where(p => p.Status == 3).ToList();
                        break;
                    default:
                        break;
                }
            }
            else
            {
                works = _context.Works.ToList();
                switch (status)
                {
                    case 1:
                        works = works.Where(p => p.Status == 1).ToList();
                        break;
                    case 2:
                        works = works.Where(p => p.Status == 2).ToList();
                        break;
                    case 3:
                        works = works.Where(p => p.Status == 3).ToList();
                        break;
                    default:
                        break;
                }
            }
            works = DoSort(works, sortProperty, sortOder);
            return works;
        }


        public bool UpdateWork(Work w)
        {
            try
            {
                Work work = _context.Works.FirstOrDefault(p => p.IdWork == w.IdWork);
                work.Name = w.Name;
                work.Description = w.Description;
                work.StartTime = w.StartTime;
                work.WorkTime = w.WorkTime;
                work.Status = w.Status;

                _context.Works.Update(work);
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
