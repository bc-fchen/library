namespace CLMS.Host.Models
{
    public class PagedRequest<T>
    {
        public int count { get; set; }
        public List<T> items { get; set; }
    }
}
