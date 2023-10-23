using CLMS.DAL;
using CLMS.Entity;
using CLMS.Host.Models;
using CLMS.Models;
using Microsoft.AspNetCore.Mvc;

namespace CLMS.Host.Controllers
{
    public class UserController : Controller
    {
        private DataContext dataContext;

        public UserController(DataContext context)
        {
            dataContext = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 新增user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        [Consumes("application/json")]
        public int AddUser([FromBody]User user)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId < 1) {
                return -1;
            }
            var entity = new UserEntity()
            {
                Id = user.Id,
                NickName = user.NickName,
                Password = user.Password,
                UserName = user.UserName,
                CreateTime = DateTime.Now,
                CreateUser = userId.Value
            };
            dataContext.Users.Add(entity);
            dataContext.SaveChanges();
            return 0;
        }

        /// <summary>
        /// 删除User
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Consumes("application/json")]
        public Msg Delete([FromBody] User user)
        {
            Msg msg = new Msg();
            if (user.Id < 1)
            {
                msg.code = 1;
                msg.message = "id 不对";
            }
            else
            {
                var entity = dataContext.Users.FirstOrDefault(x => x.Id == user.Id);
                if (entity != null)
                {
                    dataContext.Users.Remove(entity);
                    msg.code = 0;
                    msg.message = "success";
                }
                else
                {
                    msg.code = 1;
                    msg.message = "user not found";
                }
            }
            return msg;
        }

        [HttpGet]
        public User? GetPersonalInfo()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId > 0)
            {
                return GetUser(userId.Value);
            }
            else
            {
                return default(User);
            }
        }

        /// <summary>
        /// 获取单个user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public User? GetUser(int id)
        {
            if (id < 1)
            {
                return null;
            }
            var entity = dataContext.Users.FirstOrDefault(r => r.Id == id);
            if (entity != null)
            {
                return new User()
                {
                    Id = entity.Id,
                    NickName = entity.NickName,
                    Password = entity.Password,
                    UserName = entity.UserName,
                    CreateTime = entity.CreateTime,
                };
            }
            return null;
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="pageNum"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public PagedRequest<User> GetUsers(int pageNum, int pageSize, string? userName)
        {
            userName = string.IsNullOrEmpty(userName) ? string.Empty : userName;
            IQueryable<UserEntity> users = null;
            users = dataContext.Users.Where(r => r.UserName.Contains(userName)).OrderBy(r => r.Id);
            
            int count = users.Count();
            List<User> items;
            if (pageSize > 0)
            {
                items = users.Skip((pageNum - 1) * pageSize).Take(pageSize).Select(r=>new User() {
                    Id = r.Id,
                    NickName = r.NickName,
                    Password = r.Password,
                    UserName = r.UserName,
                    CreateTime=r.CreateTime,
                }).ToList();
            }
            else
            {
                items = users.Select(r => new User()
                {
                    Id = r.Id,
                    NickName = r.NickName,
                    Password = r.Password,
                    UserName = r.UserName,
                    CreateTime = r.CreateTime,
                    
                }).ToList();
            }
            return new PagedRequest<User>()
            {
                count = count,
                items = items
            };
        }

        /// <summary>
        /// 获取用户角色列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        public PagedRequest<UserRole> GetUserRoles(int? userId)
        {
            if (userId != null)
            {
                var userRoles = this.dataContext.UserRoles.Where(x => x.UserId == userId).Select(r => new UserRole()
                {
                    Id = r.Id,
                    RoleId = r.RoleId,
                    UserId = r.UserId
                });
                return new PagedRequest<UserRole>()
                {
                    count = userRoles.Count(),
                    items = userRoles.ToList()
                };
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 设置角色
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpPost]
        [Consumes("application/json")]
        public Msg SetUserRoles([FromBody] UserRoles userRoles)
        {
            Msg msg = new Msg();
            var editUserId = userRoles.userId;
            string[] roles = userRoles.roleIds.Split(',');
            if (roles.Length == 0)
            {
                msg.code = 1;
                msg.message = "权限为空";
                return msg;//权限为空
            }
            if (editUserId < 1)
            {
                msg.code = 1;
                msg.message = "用户ID不存在";
                return msg;
            }
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId < 1)
            {
                msg.code = 1;
                msg.message = "用户没有登录";
                return msg;
            }
            var oldUserRoles = dataContext.UserRoles.Where(r => r.UserId == editUserId);
            if (oldUserRoles.Count() > 0)
            {
                this.dataContext.UserRoles.RemoveRange(oldUserRoles);
            }
            var entities = roles.Select(r => new UserRoleEntity()
            {
                UserId = editUserId,
                RoleId = int.Parse(r),
                CreateTime = DateTime.Now,
                LastEditTime = DateTime.Now,
                CreateUser = userId.Value,
                LastEditUser = userId.Value,
            });
            this.dataContext.UserRoles.AddRange(entities);
            this.dataContext.SaveChanges();
            msg.code = 0;
            msg.message = "success";
            return msg;
        }

        /// <summary>
        /// 修改User
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        [Consumes("application/json")]
        public Msg UpdateUser([FromBody]User user)
        {
            Msg msg = new Msg();
            if (user == null || user.Id < 1)
            {
                msg.code = 1;
                msg.message = "用户ID不存在";
                return msg;
            }
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId < 1)
            {
                msg.code = 1;
                msg.message = "用户没有登录";
                return msg;
            }
            var entity = dataContext.Users.FirstOrDefault(r => r.Id == user.Id);
            if (entity != null)
            {
                entity.NickName = user.NickName;
                entity.Password = user.Password;
                entity.UserName = user.UserName;
                entity.LastEditUser = userId.Value;
                entity.LastEditTime = DateTime.Now;
                dataContext.Users.Update(entity);
                this.dataContext.SaveChanges();
                msg.code = 0;
                msg.message = "success";
            }
            else
            {
                msg.code = 1;
                msg.message = "用户不存在";
            }
            return msg;
        }
    }
}
