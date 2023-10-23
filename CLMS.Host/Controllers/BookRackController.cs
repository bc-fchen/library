using CLMS.DAL;
using CLMS.Entity;
using CLMS.Host.Models;
using Microsoft.AspNetCore.Mvc;

namespace CLMS.Host.Controllers
{
    public class BookRackController : Controller
    {
        private DataContext dataContext;

        public BookRackController(DataContext context)
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
        public PagedRequest<BookRack> Query(string Name, string SubName, int pageNum, int pageSize)
        {
            Name = string.IsNullOrEmpty(Name) ? string.Empty : Name;
            SubName = string.IsNullOrEmpty(SubName) ? string.Empty : SubName;

            var entities = dataContext.Librarys.Where(r => r.Name.Contains(Name) && r.SubName.Contains(SubName)).Join(dataContext.BookRacks,l=>l.Id,b=>b.LibraryId,(l,b) =>new BookRack() {Name=l.Name,SubName=l.SubName,Location=l.Location,LibraryId=b.LibraryId,Id=b.Id,Row=b.Row,Column=b.Column,Description=b.Description,CreateTime=b.CreateTime });
            var total = entities.Count();
            var dtos = entities.Skip((pageNum - 1) * pageSize).Take(pageSize).ToList();
            return new PagedRequest<BookRack>()
            {
                count = total,
                items = dtos,
            };
        }

        [Consumes("application/json")]
        [HttpPost]
        public Msg Add([FromBody] BookRack bookRack)
        {
            Msg msg = new Msg();
            if (bookRack == null)
            {
                msg.code = 1;
                msg.message = "对象为空";
                return msg;
            }
            else
            {
                var userId = HttpContext.Session.GetInt32("UserId");

                if (bookRack.Id > 0)
                {
                    //更新
                    var entity = dataContext.BookRacks.Where(r => r.Id == bookRack.Id).FirstOrDefault();
                    if (entity != null)
                    {
                        entity.LibraryId = bookRack.LibraryId;
                        entity.Row = bookRack.Row;
                        entity.Column = bookRack.Column;
                        entity.Description = bookRack.Description;
                        entity.LastEditUser = userId.GetValueOrDefault();
                        entity.LastEditTime = DateTime.Now;
                        dataContext.BookRacks.Update(entity);
                        dataContext.SaveChanges();
                    }
                    else
                    {
                        msg.code = 1;
                        msg.message = "修改失败";
                        return msg;
                    }
                }
                else
                {
                    //新增
                    var entity = new BookRackEntity()
                    {
                        Id = bookRack.Id,
                        LibraryId = bookRack.LibraryId,
                        Row = bookRack.Row,
                        Column = bookRack.Column,
                        Description = bookRack.Description,
                        CreateTime = DateTime.Now,
                        CreateUser = userId.GetValueOrDefault(),
                        LastEditTime = DateTime.Now,
                        LastEditUser = userId.GetValueOrDefault(),
                    };
                    dataContext.BookRacks.Add(entity);
                    dataContext.SaveChanges();
                }
                msg.code = 0;
                msg.message = "success";
                return msg;
            }
        }

        [Consumes("application/json")]
        [HttpPost]
        public Msg Delete([FromBody] BookRack bookRack)
        {
            Msg msg = new Msg();
            if (bookRack == null)
            {
                msg.code = 1;
                msg.message = "对象为空";
                return msg;
            }
            else
            {
                if (bookRack.Id > 0)
                {
                    var entity = dataContext.BookRacks.Where(r => r.Id == bookRack.Id).FirstOrDefault();
                    if (entity != null)
                    {
                        dataContext.BookRacks.Remove(entity);
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
