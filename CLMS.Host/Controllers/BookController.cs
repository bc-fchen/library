using CLMS.DAL;
using CLMS.Entity;
using CLMS.Host.Models;
using Microsoft.AspNetCore.Mvc;

namespace CLMS.Host.Controllers
{
    public class BookController : Controller
    {
        private DataContext dataContext;

        public BookController(DataContext context) { 
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
        public PagedRequest<Book> Query(string Name, string Publisher, int pageNum, int pageSize)
        {
            Name = string.IsNullOrEmpty(Name) ? string.Empty : Name;
            Publisher = string.IsNullOrEmpty(Publisher) ? string.Empty : Publisher;
            var bookEntities = dataContext.Books.Where(r => r.Name.Contains(Name) && r.Publisher.Contains(Publisher));
            var total = bookEntities.Count();
            var bookDtos = bookEntities.Skip((pageNum - 1) * pageSize).Take(pageSize).Select(r => new Book() { Id = r.Id, ISBN = r.ISBN, Name = r.Name, Author = r.Author, Publisher = r.Publisher, BookType = r.BookType,BookRackId=r.BookRackId,PublishDate=r.PublishDate, CreateTime = r.CreateTime,Description=r.Description }).ToList();

            //位置
            var bookRackIds = bookDtos.Select(r => r.BookRackId).ToList();
            var locations = dataContext.BookRacks.Where(r => bookRackIds.Contains(r.Id)).Join(dataContext.Librarys, b => b.LibraryId, l => l.Id, (b, l) => new BookRack() { Name = l.Name, SubName = l.SubName, Location = l.Location, LibraryId = b.LibraryId, Id = b.Id, Row = b.Row, Column = b.Column, Description = b.Description, CreateTime = b.CreateTime }).ToList();

            bookDtos.ForEach(r => {
                var location = locations.FirstOrDefault(l => l.Id == r.BookRackId);
                if (location != null) {
                    r.LibraryName = location.Name;
                    r.LibrarySubName=location.SubName;
                    r.Row=location.Row;
                    r.Column=location.Column;
                }
            });
            //
            return new PagedRequest<Book>()
            {
                count = total,
                items = bookDtos,
            };
        }

        [Consumes("application/json")]
        [HttpPost]
        public Msg Add([FromBody] Book book)
        {
            Msg msg = new Msg();
            if (book == null)
            {
                msg.code = 1;
                msg.message = "对象为空";
                return msg;
            }
            else
            {
                var userId = HttpContext.Session.GetInt32("UserId");

                if (book.Id > 0)
                {
                    //更新
                    var entity = dataContext.Books.Where(r => r.Id == book.Id).FirstOrDefault();
                    if (entity != null)
                    {
                        entity.BookRackId = book.BookRackId;
                        entity.Author = book.Author;
                        entity.Publisher = book.Publisher;
                        entity.Description = book.Description;
                        entity.BookType = book.BookType;
                        entity.ISBN = book.ISBN;
                        entity.Name = book.Name;
                        entity.LastEditUser = userId.GetValueOrDefault();
                        entity.LastEditTime = DateTime.Now;
                        dataContext.Books.Update(entity);
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
                    var entity = new BookEntity()
                    {
                        BookRackId = book.BookRackId,
                        Author = book.Author,
                        Publisher = book.Publisher,
                        PublishDate = book.PublishDate,
                        Description = book.Description,
                        BookType = book.BookType,
                        ISBN = book.ISBN,
                        Name = book.Name,
                        CreateTime = DateTime.Now,
                        CreateUser = userId.GetValueOrDefault(),
                        LastEditTime = DateTime.Now,
                        LastEditUser = userId.GetValueOrDefault(),
                    };
                    dataContext.Books.Add(entity);
                    dataContext.SaveChanges();
                }
                msg.code = 0;
                msg.message = "success";
                return msg;
            }
        }

        [Consumes("application/json")]
        [HttpPost]
        public Msg Delete([FromBody] Book book) {
            Msg msg = new Msg();
            if (book == null)
            {
                msg.code = 1;
                msg.message = "对象为空";
                return msg;
            }
            else
            {
                if (book.Id > 0)
                {
                    var entity = dataContext.Books.Where(r => r.Id == book.Id).FirstOrDefault();
                    if (entity != null)
                    {
                        dataContext.Books.Remove(entity);
                        dataContext.SaveChanges();
                        msg.code = 0;
                        msg.message = "success";
                    }
                    else { 
                        msg.code = 1;
                        msg.message = "对象不存在或已被删除";
                    }
                }
                else {

                    msg.code = 1;
                    msg.message = "对象Id小于0";
                }
                return msg;
            }
        }
    }
}
