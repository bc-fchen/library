using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CLMS.Entity
{
    /// <summary>
    /// 菜单管理
    /// </summary>
    public class MenuEntity
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 菜单名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 菜单描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 菜单路径
        /// </summary>
        public string? Url { get; set; }

        public string? Icon { get; set; }

        /// <summary>
        /// 父ID
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int? SortId { get; set; }

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
