using CLMS.DAL;
using CLMS.Entity;
using CLMS.Host.Models;
using CLMS.Models;
using Microsoft.AspNetCore.Mvc;

namespace CLMS.Host.Controllers
{
    public class RoleController : Controller
    {
        private DataContext dataContext;

        public RoleController(DataContext context)
        {
            dataContext = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 新增role
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpPost]
        [Consumes("application/json")]
        public Msg AddRole([FromBody]Role role)
        {
            Msg msg = new Msg();
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId < 1)
            {
                msg.code = 1;
                msg.message = "用户没有登录";
                return msg;
            }
            if (role != null && !string.IsNullOrEmpty(role.Name))
            {
                var entity = new RoleEntity()
                {
                    Id = role.Id,
                    Name = role.Name,
                    Description = role.Description,
                    CreateTime = DateTime.Now,
                    CreateUser = userId.Value,
                    LastEditTime = DateTime.Now,
                    LastEditUser = userId.Value,
                };
                dataContext.Roles.Add(entity);
                dataContext.SaveChanges();
                msg.code = 0;
                msg.message = "success";
            }
            else { 
                msg.code = 1;
                msg.message = "角色名称为空";
            }
            return msg;
        }

        /// <summary>
        /// 删除Role
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Consumes("application/json")]
        public Msg Delete([FromBody]Role role)
        {
            Msg msg = new Msg();
            if (role.Id < 1)
            {
                msg.code = 1;
                msg.message = "id 不对";
            }
            else
            {
                var entity = dataContext.Roles.FirstOrDefault(x => x.Id == role.Id);
                if (entity != null)
                {
                    dataContext.Roles.Remove(entity);
                    msg.code = 0;
                    msg.message = "success";
                }
                else
                {
                    msg.code = 1;
                    msg.message = "role not found";
                }
            }
            return msg;
        }

        /// <summary>
        /// 获取单个role
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public Role? GetRole(int id)
        {
            if (id < 1)
            {
                return null;
            }
            var entity = dataContext.Roles.FirstOrDefault(r => r.Id == id);
            if (entity != null)
            {
                return new Role()
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    Description = entity.Description,
                };
            }
            return null;
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="roleName"></param>
        /// <param name="pageNum"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public PagedRequest<Role> GetRoles(string? roleName, int pageNum, int pageSize)
        {
            roleName = string.IsNullOrEmpty(roleName) ? string.Empty : roleName;
            var entities = dataContext.Roles.Where(r => r.Name.Contains(roleName));
            var total = entities.Count();
            List<Role> dtos=new List<Role>();
            if (pageSize > 0)
            {
                dtos = entities.Skip((pageNum - 1) * pageSize).Take(pageSize).Select(r => new Role() { Id = r.Id, Name = r.Name, Description = r.Description, CreateTime = r.CreateTime }).ToList();
            }
            else {
                dtos = entities.Select(r => new Role() { Id = r.Id, Name = r.Name, Description = r.Description, CreateTime = r.CreateTime }).ToList();
            }
            return new PagedRequest<Role>()
            {
                count = total,
                items = dtos,
            };
        }

        /// <summary>
        /// 修改Role
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpPost]
        [Consumes("application/json")]
        public Msg UpdateRole([FromBody] Role role)
        {
            Msg msg = new Msg();
            if (role == null || role.Id < 1 || string.IsNullOrEmpty(role.Name))
            {
                msg.code = 1;
                msg.message = "信息为空";
                return msg;
            }
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId < 1)
            {
                msg.code = 1;
                msg.message = "用户没有登录";
                return msg;
            }
            var entity = dataContext.Roles.FirstOrDefault(r => r.Id == role.Id);
            if (entity != null)
            {
                entity.Name = role.Name;
                entity.Description = role.Description;
                entity.LastEditUser = userId.Value;
                entity.LastEditTime = DateTime.Now;
                dataContext.Roles.Update(entity);
                this.dataContext.SaveChanges();
                msg.code = 0;
                msg.message = "success";
            }
            else
            {
                msg.code = 1;
                msg.message = "角色不存在";
            }
            return msg;
        }

        [HttpGet]
        public PagedRequest<UserRight> GetUserRights(int? userId)
        {
            if (userId != null)
            {
                var query = from u in dataContext.UserRoles
                            join r in dataContext.Roles on u.RoleId equals r.Id
                            join x in dataContext.RoleMenus on r.Id equals x.RoleId
                            join m in dataContext.Menus on x.MenuId equals m.Id
                            where u.UserId == userId
                            select new UserRight { Id = m.Id, RoleName = r.Name, MenuName = m.Name, Url = m.Url, Icon = m.Icon, ParentId = m.ParentId, SortId = m.SortId };
                return new PagedRequest<UserRight>()
                {
                    count = query.Count(),
                    items = query.ToList()
                };
            }
            return null;
        }

        /// <summary>
        /// 获取角色菜单列表
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpGet]
        public PagedRequest<RoleMenuEntity> GetRoleMenus(int? roleId)
        {
            if (roleId != null)
            {
                var roleMenus = this.dataContext.RoleMenus.Where(x => x.RoleId == roleId);
                return new PagedRequest<RoleMenuEntity>()
                {
                    count = roleMenus.Count(),
                    items = roleMenus.ToList()
                };
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 设置角色菜单列表
        /// </summary>
        /// <param name="menuIds"></param>
        /// <returns></returns>
        [HttpPost]
        [Consumes("application/json")]
        public Msg SetRoleMenus([FromBody] RoleMenus roleMenus)
        {
            Msg msg = new Msg();
            int roleId = roleMenus.roleId;
            string[] menus = roleMenus.menuIds.Split(',');
            if (menus.Length == 0)
            {
                msg.code = 1;
                msg.message = "权限为空";
                return msg;//权限为空
            }
            if (roleId < 1)
            {
                msg.code = 1;
                msg.message = "角色ID不存在";
                return msg;
            }
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId < 1)
            {
                msg.code = 1;
                msg.message = "用户没有登录";
                return msg;
            }
            var oldRoleMenus = dataContext.RoleMenus.Where(r => r.RoleId == roleId);
            if (oldRoleMenus.Count() > 0)
            {
                this.dataContext.RoleMenus.RemoveRange(oldRoleMenus);
            }
            var entities = menus.Select(r => new RoleMenuEntity()
            {
                RoleId = roleId,
                MenuId = int.Parse(r),
                CreateTime = DateTime.Now,
                LastEditTime = DateTime.Now,
                CreateUser = userId.Value,
                LastEditUser = userId.Value,
            });
            this.dataContext.RoleMenus.AddRange(entities);
            this.dataContext.SaveChanges();
            msg.code = 0;
            msg.message = "success";
            return msg;
        }
    }
}
