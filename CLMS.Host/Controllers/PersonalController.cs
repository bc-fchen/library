using CLMS.DAL;
using CLMS.Host.Models;
using Microsoft.AspNetCore.Mvc;

namespace CLMS.Host.Controllers
{
    public class PersonalController : Controller
    {
        private DataContext dataContext;

        public PersonalController(DataContext context)
        {
            dataContext = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Consumes("application/json")]
        public Msg UpdatePassword([FromBody] Personal personal)
        {
            Msg msg = new Msg();
            if (personal == null || string.IsNullOrEmpty(personal.UserName))
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
            var entity = dataContext.Users.FirstOrDefault(r => r.UserName == personal.UserName);
            if (entity != null)
            {

                entity.Password = personal.NewPassword;
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
