using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CLMS.Models
{
    public class User
    {
        /// <summary>
        /// 用户唯一标识
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 登录账号
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 显示名称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 当前登录的账号的ID
        /// </summary>
        public int CreateUser { get; set; }

        /// <summary>
        /// 最后编辑时间
        /// </summary>
        public DateTime LastEditTime { get; set; }

        /// <summary>
        /// 最后修改人
        /// </summary>
        public int LastEditUser { get; set; }
    }
}
