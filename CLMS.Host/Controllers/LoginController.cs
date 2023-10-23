using CLMS.DAL;
using CLMS.Host.Models;
using CLMS.Models;
using Microsoft.AspNetCore.Mvc;

namespace CLMS.Host.Controllers
{
    public class LoginController : Controller
    {
        private DataContext dataContext;

        public LoginController(DataContext context)
        {
            dataContext = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [Consumes("application/json")]
        [HttpPost]
        public Msg Login([FromBody]User user)
        {
            Msg msg = new Msg();
            if (string.IsNullOrEmpty(user.UserName) || string.IsNullOrEmpty(user.Password))
            {
                msg.message = "用户名或密码为空";
                msg.code = 1;
                return msg;
            }
            else
            {
                var item = dataContext.Users.FirstOrDefault(i => i.UserName == user.UserName && i.Password == user.Password);
                if (item != null)
                {
                    HttpContext.Session.SetInt32("UserId", item.Id);
                    msg.message = "success";
                    msg.code = 0;
                    return msg;
                }
                else
                {
                    msg.message = "用户名或密码验证错误";
                    msg.code = 1;
                    return msg;
                }

            }
        }
    }
}
