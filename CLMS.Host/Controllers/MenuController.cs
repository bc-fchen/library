using CLMS.DAL;
using CLMS.Entity;
using CLMS.Host.Models;
using CLMS.Models;
using Microsoft.AspNetCore.Mvc;

namespace CLMS.Host.Controllers
{
    public class MenuController : Controller
    {
        private DataContext dataContext;

        public MenuController(DataContext context)
        {
            dataContext = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 新增Menu
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        [HttpPost]
        [Consumes("application/json")]
        public Msg AddMenu([FromBody]Menu menu)
        {
            Msg msg = new Msg();
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId < 1)
            {
                msg.code = 1;
                msg.message = "用户没有登录";
                return msg;
            }
            var entity = new MenuEntity()
            {
                Id = menu.Id,
                Name = menu.Name,
                Description = menu.Description,
                Icon = menu.Icon,
                ParentId = menu.ParentId,
                SortId = menu.SortId,
                Url = menu.Url,
                CreateTime = DateTime.Now,
                CreateUser = userId.Value,
            };
            dataContext.Menus.Add(entity);
            dataContext.SaveChanges();
            msg.code = 0;
            msg.message = "success";
            return msg;
        }

        /// <summary>
        /// 删除Menu
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Consumes("application/json")]
        public Msg Delete([FromBody] Menu menu)
        {
            Msg msg = new Msg();
            if (menu.Id < 1)
            {
                msg.code = 1;
                msg.message = "id 不对";
            }
            else
            {
                var entity = dataContext.Menus.FirstOrDefault(x => x.Id == menu.Id);
                if (entity != null)
                {
                    dataContext.Menus.Remove(entity);
                    dataContext.SaveChanges();
                    msg.code = 0;
                    msg.message = "success";
                }
                else {
                    msg.code = 1;
                    msg.message = "menu not found";
                }
                
            }
            return msg;
        }

        /// <summary>
        /// 获取单个menu
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public Menu? GetMenu(int id)
        {
            if (id < 1)
            {
                return null;
            }
            var entity = dataContext.Menus.FirstOrDefault(r => r.Id == id);
            if (entity != null)
            {
                return new Menu()
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    Description = entity.Description,
                    Icon = entity.Icon,
                    ParentId = entity.ParentId,
                    SortId = entity.SortId,
                    Url = entity.Url,
                };
            }
            return default(Menu);
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="menuName"></param>
        /// <param name="pageNum"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public PagedRequest<Menu> GetMenus(string? menuName, int pageNum, int pageSize)
        {
            menuName = string.IsNullOrEmpty(menuName) ? string.Empty : menuName;
            IQueryable<MenuEntity> menus = null;
            menus = dataContext.Menus.Where(r => r.Name.Contains(menuName)).OrderBy(r => r.Id);

            int count = menus.Count();
            List<Menu> items;
            if (pageSize > 0)
            {
                items = menus.Skip((pageNum - 1) * pageSize).Take(pageSize).Select(r => new Menu()
                {
                    Id = r.Id,
                    Name = r.Name,
                    Description = r.Description,
                    Icon = r.Icon,
                    ParentId = r.ParentId,
                    SortId = r.SortId,
                    Url = r.Url,
                    CreateTime = r.CreateTime,
                }).ToList();
            }
            else
            {
                items = menus.Select(r => new Menu()
                {
                    Id = r.Id,
                    Name = r.Name,
                    Description = r.Description,
                    Icon = r.Icon,
                    ParentId = r.ParentId,
                    SortId = r.SortId,
                    Url = r.Url,
                    CreateTime=r.CreateTime,
                }).ToList();
            }
            return new PagedRequest<Menu>()
            {
                count = count,
                items = items
            };
        }

        /// <summary>
        /// 修改Menu
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        [HttpPost]
        [Consumes("application/json")]
        public Msg UpdateMenu([FromBody]Menu menu)
        {
            Msg msg = new Msg();
            if (menu == null || menu.Id < 1)
            {
                msg.code = 1;
                msg.message = "id 不对";
            }
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId < 1)
            {
                msg.code = 1;
                msg.message = "用户没有登录";
                return msg;
            }
            var entity = dataContext.Menus.FirstOrDefault(r => r.Id == menu.Id);
            if (entity == null)
            {
                msg.code = 1;
                msg.message = "菜单不存在";
                return msg;
            }

            entity.Id = menu.Id;
            entity.Name = menu.Name;
            entity.Description = menu.Description;
            entity.Icon = menu.Icon;
            entity.ParentId = menu.ParentId;
            entity.SortId = menu.SortId;
            entity.Url = menu.Url;
            entity.LastEditTime = DateTime.Now;
            entity.LastEditUser = userId.Value;
            dataContext.Menus.Update(entity);
            this.dataContext.SaveChanges();
            msg.code = 0;
            msg.message = "success";
            return msg;
        }
    }
}
