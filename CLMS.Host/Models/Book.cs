namespace CLMS.Host.Models
{
    public class Book
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 图书编号
        /// </summary>
        public string ISBN { get; set; }

        /// <summary>
        /// 图书名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 图书作者
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// 图书出版社
        /// </summary>
        public string Publisher { get; set; }

        public DateTime PublishDate { get; set; }

        /// <summary>
        /// 图书类型
        /// </summary>
        public string BookType { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 书架ID
        /// </summary>
        public long BookRackId { get; set; }

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

        public string? LibraryName { get; set; }

        /// <summary>
        /// 图书室名称
        /// </summary>
        public string? LibrarySubName { get; set; }

        /// <summary>
        /// 排
        /// </summary>
        public int Row { get; set; }

        /// <summary>
        /// 列
        /// </summary>
        public int Column { get; set; }
    }
}
