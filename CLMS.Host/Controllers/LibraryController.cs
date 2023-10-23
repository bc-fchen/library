using CLMS.DAL;
using CLMS.Entity;
using CLMS.Host.Models;
using Microsoft.AspNetCore.Mvc;

namespace CLMS.Host.Controllers
{
    public class LibraryController : Controller
    {
        private DataContext dataContext;

        public LibraryController(DataContext context)
        {
            dataContext = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 获取符合条件的查询
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Publisher"></param>
        /// <param name="pageNum"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public PagedRequest<Library> Query(string Name, string SubName, int pageNum, int pageSize)
        {
            Name = string.IsNullOrEmpty(Name) ? string.Empty : Name;
            SubName = string.IsNullOrEmpty( SubName) ? string.Empty:SubName;

            var entities = dataContext.Librarys.Where(r => r.Name.Contains(Name) && r.SubName.Contains(SubName));
            var total = entities.Count();
            var dtos = entities.Skip((pageNum - 1) * pageSize).Take(pageSize).Select(r => new Library() { Id = r.Id,  Name = r.Name, SubName=r.SubName,Location=r.Location,Manager=r.Manager, CreateTime = r.CreateTime }).ToList();
            return new PagedRequest<Library>()
            {
                count = total,
                items = dtos,
            };
        }

        /// <summary>
        /// 查询所有信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<Library> QueryAll() { 
            var dtos = dataContext.Librarys.Select(r=> new Library() { Id = r.Id, Name = r.Name, SubName = r.SubName}).ToList();
            return dtos;
        }

        [Consumes("application/json")]
        [HttpPost]
        public Msg Add([FromBody] Library library)
        {
            Msg msg = new Msg();
            if (library == null)
            {
                msg.code = 1;
                msg.message = "对象为空";
                return msg;
            }
            else
            {
                var userId= HttpContext.Session.GetInt32("UserId");
                
                if (library.Id > 0)
                {
                    //更新
                    var entity = dataContext.Librarys.Where(r => r.Id == library.Id).FirstOrDefault();
                    if (entity != null)
                    {
                        entity.Name = library.Name;
                        entity.SubName = library.SubName;
                        entity.Location = library.Location;
                        entity.Manager= library.Manager;
                        entity.LastEditUser = userId.GetValueOrDefault();
                        entity.LastEditTime = DateTime.Now;
                        dataContext.Librarys.Update(entity);
                        dataContext.SaveChanges();
                    }
                    else {
                        msg.code = 1;
                        msg.message = "修改失败";
                        return msg;
                    }
                }
                else
                {
                    //新增
                    var entity = new LibraryEntity()
                    {
                        Id = library.Id,
                        Name = library.Name,
                        SubName = library.SubName,
                        Location = library.Location,
                        Manager = library.Manager,
                        CreateTime = DateTime.Now,
                        CreateUser = userId.GetValueOrDefault(),
                        LastEditTime = DateTime.Now,
                        LastEditUser = userId.GetValueOrDefault(),
                    };
                    dataContext.Librarys.Add(entity);
                    dataContext.SaveChanges();
                }
                msg.code = 0;
                msg.message = "success";
                return msg;
            }
        }

        [Consumes("application/json")]
        [HttpPost]
        public Msg Delete([FromBody] Library library)
        {
            Msg msg = new Msg();
            if (library == null)
            {
                msg.code = 1;
                msg.message = "对象为空";
                return msg;
            }
            else
            {
                if (library.Id > 0)
                {
                    var entity = dataContext.Librarys.Where(r => r.Id == library.Id).FirstOrDefault();
                    if (entity != null)
                    {
                        dataContext.Librarys.Remove(entity);
                        dataContext.SaveChanges();
                        msg.code = 0;
                        msg.message = "success";
                    }
                    else
                    {
                        msg.code = 1;
                        msg.message = "对象不存在或已被删除";
                    }
                }
                else
                {

                    msg.code = 1;
                    msg.message = "对象Id小于0";
                }
                return msg;
            }
        }
    }
}
