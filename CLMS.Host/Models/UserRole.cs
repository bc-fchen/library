using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CLMS.Models
{
    /// <summary>
    /// 用户-角色模型
    /// </summary>
    public class UserRole
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 角色ID
        /// </summary>
        public int RoleId { get; set; }
    }

    public class UserRoles
    {
        public int userId { get; set; }
        public string roleIds { get; set; }
    }
}
