using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CLMS.Entity
{
    /// <summary>
    /// 用户-角色模型
    /// </summary>
    public class UserRoleEntity
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
        public DateTime? LastEditTime { get; set; }

        /// <summary>
        /// 最后修改人
        /// </summary>
        public int LastEditUser { get; set; }
    }
}
