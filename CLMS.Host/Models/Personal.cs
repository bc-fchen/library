namespace CLMS.Host.Models
{
    public class Personal
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public string NewPassword { get; set; }
    }
}
