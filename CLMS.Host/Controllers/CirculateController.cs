using CLMS.DAL;
using CLMS.Entity;
using CLMS.Host.Models;
using Microsoft.AspNetCore.Mvc;

namespace CLMS.Host.Controllers
{
    /// <summary>
    /// 借还管理
    /// </summary>
    public class CirculateController : Controller
    {
        private DataContext dataContext;

        public CirculateController(DataContext context)
        {
            dataContext = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public PagedRequest<Circulate> Query(string Name, int pageNum, int pageSize)
        {
            Name = string.IsNullOrEmpty(Name) ? string.Empty : Name;
            var dtos = dataContext.Circulates.Join(dataContext.Books, c => c.BookId, b => b.Id, (c, b) => new Circulate()
            {
                Id = c.Id,
                Name = b.Name,
                BookId = c.BookId,
                BorrowConfirmor = c.BorrowConfirmor,
                BorrowTime = c.BorrowTime,
                BorrowUser = c.BorrowUser,
                ISBN = b.ISBN,
                IsReturn = c.IsReturn,
                ReturnConfirmor = c.ReturnConfirmor,
                ReturnTime = c.ReturnTime,
            }).Where(r=>r.Name.Contains(Name));
            var total = dtos.Count();
            var dtos2 = dtos.Skip((pageNum - 1) * pageSize).Take(pageSize).ToList();
            //
            return new PagedRequest<Circulate>()
            {
                count = total,
                items = dtos2,
            };
        }

        [Consumes("application/json")]
        [HttpPost]
        public Msg Borrow([FromBody]Borrow borrow) { 
            Msg msg = new Msg();
            if (borrow == null || string.IsNullOrEmpty(borrow.ISBN))
            {
                msg.code = 1;
                msg.message = "书籍为空";
                return msg;
            }
            var book = dataContext.Books.FirstOrDefault(r => r.ISBN == borrow.ISBN);
            if (book == null)
            {
                msg.code = 1;
                msg.message = "ISBN有误";
                return msg;
            }
            var entity = dataContext.Circulates.FirstOrDefault(r => r.BookId == book.Id && r.IsReturn == false);
            if (entity != null)
            {
                msg.code = 1;
                msg.message = "书籍已被借阅";
                return msg;
            }
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId < 0) {
                msg.code = 1;
                msg.message = "尚未登录";
                return msg;
            }
            var borrorConfirmor = dataContext.Users.FirstOrDefault(r => r.Id == userId)?.NickName;
            var entity2  = new CirculateEntity()
            {
                Id = 0,
                BookId = book.Id,
                IsReturn = false,
                BorrowTime = DateTime.Now,
                BorrowUser=borrow.BorrowUser,
                BorrowConfirmor= borrorConfirmor,
            };
            this.dataContext.Circulates.Add(entity2);
            this.dataContext.SaveChanges();
            msg.code = 0;
            msg.message = "success";
            return msg;
        }

        /// <summary>
        /// 归还
        /// </summary>
        /// <param name="returns"></param>
        /// <returns></returns>
        [Consumes("application/json")]
        [HttpPost]
        public Msg Return([FromBody] Return returns) { 
            Msg msg = new Msg();
            if (returns == null || string.IsNullOrEmpty(returns.ISBN))
            {
                msg.code = 1;
                msg.message = "书籍为空";
                return msg;
            }
            var book = dataContext.Books.FirstOrDefault(r => r.ISBN == returns.ISBN);
            if (book == null)
            {
                msg.code = 1;
                msg.message = "ISBN有误";
                return msg;
            }
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId < 0)
            {
                msg.code = 1;
                msg.message = "尚未登录";
                return msg;
            }
            var returnConfirmor = dataContext.Users.FirstOrDefault(r => r.Id == userId)?.NickName;
            var entity = dataContext.Circulates.FirstOrDefault(r => r.BookId == book.Id && r.IsReturn == false);
            if (entity != null)
            {
                entity.IsReturn = true;
                entity.ReturnTime = DateTime.Now;
                entity.ReturnConfirmor=returnConfirmor;
                dataContext.Circulates.Update(entity);
                dataContext.SaveChanges();
                msg.code = 0;
                msg.message = "success";
            }
            else {
                msg.code = 1;
                msg.message = "书籍已归还";
            }
            return msg;
        }
    }
}
