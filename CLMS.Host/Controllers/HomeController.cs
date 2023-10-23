using CLMS.DAL;
using CLMS.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CLMS.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private DataContext dataContext;

        public HomeController(ILogger<HomeController> logger, DataContext context)
        {
            _logger = logger;
            dataContext = context;
        }

        public IActionResult Index()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            //判断是否登录
            if (userId != null)
            {

                var user = dataContext.Users.FirstOrDefault(u => u.Id == userId);
                if (user != null)
                {
                    ViewBag.NickName = user.NickName;
                    ViewBag.UserRights = GetUserRights();
                }
                return View();
            }
            else
            {
                return Redirect("/Login");
            }

        }

        public IActionResult Welcome()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public List<UserRight> GetUserRights()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId != null)
            {
                var query = from u in dataContext.UserRoles
                            join r in dataContext.Roles on u.RoleId equals r.Id
                            join x in dataContext.RoleMenus on r.Id equals x.RoleId
                            join m in dataContext.Menus on x.MenuId equals m.Id
                            where u.UserId == userId
                            select new UserRight { Id = m.Id, RoleName = r.Name, MenuName = m.Name, Url = m.Url, ParentId = m.ParentId, SortId = m.SortId };

                return query.ToList();
            }
            return null;
        }

        /// <summary>
        /// 退出
        /// </summary>
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Redirect("/Login");
        }
    }
}