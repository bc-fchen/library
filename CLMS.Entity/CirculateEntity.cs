using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLMS.Entity
{
	/// <summary>
	/// 借还记录
	/// </summary>
	public class CirculateEntity
	{
		/// <summary>
		/// 唯一标识
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// 图书标识
		/// </summary>
		public int BookId { get; set; }

		/// <summary>
		/// 是否归还
		/// </summary>
		public bool IsReturn { get; set; }

		/// <summary>
		/// 借阅人
		/// </summary>
		public string BorrowUser { get; set; }

		/// <summary>
		/// 借阅时间
		/// </summary>
		public DateTime BorrowTime { get; set; }

		/// <summary>
		/// 借阅确认人
		/// </summary>
		public string BorrowConfirmor { get; set; }

		/// <summary>
		/// 归还时间
		/// </summary>
		public DateTime? ReturnTime { get; set; }

		/// <summary>
		/// 归还确认人
		/// </summary>
		public string? ReturnConfirmor { get; set; }
	}
}
