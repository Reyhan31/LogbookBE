namespace LogBookAPI.Models
{
    public class Approver
    {
        public string UserName { get; set; }
        public string Email { get; set; }

        public long RoleId { get; set; }
        public long Id { get; set; }
    }
}