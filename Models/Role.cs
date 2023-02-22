using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Kalbe.Library.Common.EntityFramework.Models;

namespace LogBookAPI.Models
{
    [Table("Roles")]
    public class Role : Base
    {
        [Required]
        [StringLength(Constant.Length255)]
        public string roleName { get; set; }
    }
}