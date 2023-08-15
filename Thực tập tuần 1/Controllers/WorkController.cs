using Microsoft.AspNetCore.Mvc;
using Thực_tập_tuần_1.IServices;
using Thực_tập_tuần_1.Models;
using Thực_tập_tuần_1.Services;
using System.Linq;
using static System.Reflection.Metadata.BlobBuilder;
using Humanizer;
using System.Net.WebSockets;
using Microsoft.AspNetCore.Mvc.RazorPages;
using X.PagedList;
using System.Collections.Generic;
using System.Drawing.Printing;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;

namespace Thực_tập_tuần_1.Controllers
{
    public class WorkController : Controller
    {
        private readonly IWorkServices _workServices;
        public WorkController()
        {
            _workServices = new WorkServices();
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Work work)
        {
            _workServices.CreateWork(work);
            return RedirectToAction("Home");
        }
        [HttpGet]
        public IActionResult Update(Guid id)
        {
            var work = _workServices.GetWorkById(id);
            return View (work);
        }
        public IActionResult Update(Work work)
        {
            _workServices.UpdateWork(work);
            if (_workServices.UpdateWork(work) == false)
            {
                return BadRequest();
            }
            return RedirectToAction("Home");
        }
        public IActionResult Delete(Guid id)
        {
            _workServices.DeleteWork(id);
            return RedirectToAction("Home");
        }
        [HttpGet]
        public IActionResult Home(string name, int? page,string? status)
        {
            var listWork = _workServices.GetAllWork();
            if (!string.IsNullOrEmpty(name))
            {
                listWork = listWork.Where(p => p.Name.ToLower().Contains(name.ToLower())).ToList();
            }
            if (status != null)
            {
                switch (status)
                {
                    case "Cần làm":
                        listWork = listWork.Where(p => p.Status == 1).ToList();
                        break;
                    case "Đang làm":
                        listWork = listWork.Where(p => p.Status == 2).ToList();
                        break;
                    default:
                        listWork = listWork.Where(p => p.Status != 1 && p.Status != 2).ToList();
                        break;
                }
            }
            //listWork = listWork.OrderByDescending(p => p.Name).ToList();
            int pageSize = 3;
            // nếu page null hoặc = 0 thì chuyển page Number -> 1, nếu ko thì lấy theo giá trị của page
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            PagedList<Work> lst = new PagedList<Work>(listWork, pageNumber, pageSize);
            return View(lst);
        }
        [HttpGet]
        public async Task<IActionResult> TestFilterdata(string? sortOder, string? CurrentFilter, string? searchString, int? page, int InputPageSize)
        {
            ViewData["NameSortParm"] = sortOder == "name" ? "name_desc" : "name"; // ktra nếu từ khóa null thì trả về name-desc, ngược lại thì k trả về j hết
            ViewData["TimeWorkSortParm"] = sortOder == "time" ? "time_desc" : "time";// ktra nếu từ khóa null thì trả về time-desc, ngược lại thì trả về nó
            ViewData["CurrentFilter"] = searchString; // Bộ lọc hiện tại
            ViewData["CurrentSort"] = sortOder; // Thứ tự sắp xếp hiện tại
            ViewBag.PageSize = InputPageSize;
            // nếu chuỗi tìm kiếm bị thay đổi khi phân trang, trang phải đặt lại thành 1, chuỗi tìm kiếm thay đổi 
            // --> Searchstring k rỗng
            var listWork = from s in _workServices.GetAllWork()
                           select s;
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = CurrentFilter;
            }
            if (!String.IsNullOrEmpty(searchString))
            {
                listWork = listWork.Where(p => p.Name.ToLower().Contains(searchString.ToLower()));
            }
            if (sortOder != null)
            {
                switch (sortOder)
                {
                    case "name_desc":
                        listWork = listWork.OrderBy(p => p.Name);
                        break;
                    case "time":
                        listWork = listWork.OrderBy(p => p.WorkTime); ;
                        break;
                    case "time_desc":
                        listWork = listWork.OrderByDescending(p => p.WorkTime); ;
                        break;
                    default:
                        listWork = listWork.OrderByDescending(p => p.Name);
                        break;
                }
            }
            int pageSize = 3;
            /*pageSize = ViewBag.PageSize;*/
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            PagedList<Work> lst = new PagedList<Work>(listWork, pageNumber, pageSize);
            return View(lst);
        }




        private SortModel ApplySort(string sortExpresstion)
        {
            ViewData["SortParamName"] = "name";
            ViewData["SortParamWorkTime"] = "worktime";

            SortModel sortModel = new SortModel();
            switch (sortExpresstion.ToLower())
            {
                case "name_desc":
                    sortModel.SortOrder = SortOrder.Descending;
                    sortModel.SortedProperty = "name";
                    ViewData["SortParamName"] = "name";
                    break;
                case "worktime":
                    sortModel.SortOrder = SortOrder.Ascending;
                    sortModel.SortedProperty = "worktime";
                    ViewData["SortParamWorkTime"] = "worktime_desc";
                    break;
                case "worktime_desc":
                    sortModel.SortOrder = SortOrder.Descending;
                    sortModel.SortedProperty = "worktime";
                    ViewData["SortParamWorkTime"] = "worktime";
                    break;
                default:
                    sortModel.SortOrder = SortOrder.Ascending;
                    sortModel.SortedProperty = "name";
                    ViewData["SortParamName"] = "name_desc";
                    break;
            }
            return sortModel;
        }
        public IActionResult FinalTest(string sortExpresstion="",string searchString="", int pageSize = 5,int pg = 1, int status = 0)
        {
            SortModel sortModel = ApplySort(sortExpresstion);
            if (searchString == null)
            {
                searchString = "";
            }
            ViewBag.SearchString = searchString;
            List<Work> works = _workServices.GetWorksPage(sortModel.SortedProperty, sortModel.SortOrder,searchString,status);

            var pager = new PagerModel(works.Count,pg,pageSize);
            pager.Sortexpression = sortExpresstion;
            pager.SearchString = searchString;
            pager.Status = status;
            this.ViewBag.Pager = pager;

            pg = pager.CurrentPage;
            works = works.Skip((pg - 1) * pageSize).Take(pageSize).ToList();
            return View(works);
        }


    }
}
