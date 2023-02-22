using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Kalbe.Library.Common.EntityFramework.Models;

namespace LogBookAPI.Models
{
    [Table("Users")]
    public class User : Base
    {
        public long RoleId { get; set; }
        [ForeignKey("RoleId")]
        public Role? Role { get; set; }

        [Required]
        [StringLength(Constant.Length255)]
        public string UserName { get; set; }

        [Required]
        [StringLength(Constant.Length255)]
        public string Email { get; set; }

        [Required]
        [StringLength(Constant.Length255)]
        public string Password { get; set; }

        public string? Department { get; set; }

        public string? Position { get; set; }

        public string? Rekening { get; set; }

        public long? MentorId { get; set; }

        [ForeignKey("MentorId")]
        public User? Mentor { get; set; }

        public string? mobileNumber { get; set; }

        public string? University { get; set; }

        public DateTime EntryDate { get; set; }
        public DateTime EndDate { get; set; }

        public List<Logbook> Logbooks { get; set; } = new List<Logbook>();

        public User()
        {
        }

        public User(User obj)
        {
            Id = obj.Id;
            RoleId = obj.RoleId;
            Role = obj.Role;
            UserName = obj.UserName;
            Email = obj.Email;
            Department = obj.Department;
            Position = obj.Position;
            Rekening = obj.Rekening;
            MentorId = obj.MentorId;
            Mentor = obj.Mentor;
            mobileNumber = obj.mobileNumber;
            University = obj.University;
            EntryDate = obj.EntryDate;
            EndDate = obj.EndDate;
            Logbooks = obj.Logbooks;
            CreatedBy = obj.CreatedBy;
            CreatedByName = obj.CreatedByName;
            CreatedDate = obj.CreatedDate;
            UpdatedBy = obj.UpdatedBy;
            UpdatedByName = obj.UpdatedByName;
            UpdatedDate = obj.UpdatedDate;
        }
    }

    public enum UserRole
    {
        HR = 1,
        Mentor = 2,
        Intern = 3,
    }
}