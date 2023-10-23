namespace CLMS.Host.Models
{
    public class Msg
    {
        /// <summary>
        /// 0:成功，1:失败
        /// </summary>
        public int code { get; set; }

        public string message { get; set; }

        public object data { get; set; }
    }
}
