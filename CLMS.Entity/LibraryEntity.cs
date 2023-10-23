namespace CLMS.Entity
{
    /// <summary>
    /// 图书馆实体
    /// </summary>
    public class LibraryEntity
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 图书馆名称
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 图书室名称
        /// </summary>
        public string? SubName { get; set; }

        /// <summary>
        /// 位置
        /// </summary>
        public string? Location { get; set; }

        /// <summary>
        /// 管理员
        /// </summary>
        public string? Manager { get; set; }

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