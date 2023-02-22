using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Kalbe.Library.Common.EntityFramework.Models;

namespace LogBookAPI.Models
{
    [Table("Logbook")]
    public class Logbook : Base
    {
        [Required]
        public long UserId { get; set; }
        [Required]
        public long MentorId { get; set; }
        public long? HRId { get; set; }
        public long? GajiWFO { get; set; }
        public long? GajiWFH { get; set; }
        public long? GajiTotal { get; set; }
        public int Status { get; set; }
        public string? StatusText { get; set; }
        public string? approve_by_mentor { get; set; }
        public string? approve_by_hr { get; set; }

        [Required]
        public string monthLogbook { get; set; }
        [Required]
        public int yearLogbook { get; set; }
        public string? cancelReason { get; set; }
        public List<LogbookItem> items { get; set; } = new List<LogbookItem>();

        public Logbook()
        {
        }

        public Logbook(Logbook obj)
        {
            Id = obj.Id;
            UserId = obj.UserId;
            MentorId = obj.MentorId;
            GajiWFO = obj.GajiWFO;
            GajiWFH = obj.GajiWFH;
            GajiTotal = obj.GajiTotal;
            Status = obj.Status;
            StatusText = obj.StatusText;
            approve_by_mentor = obj.approve_by_mentor;
            approve_by_hr = obj.approve_by_hr;
            monthLogbook = obj.monthLogbook;
            yearLogbook = obj.yearLogbook;
            cancelReason = obj.cancelReason;
            items = obj.items;
        }
    }

    public enum LogbookStatus
    {
        Draft = 0,
        Submitted = 1,
        ApprovedByMentor = 2,
        ApprovedByHR = 3,
        ReviseByMentor = 4,
        ReviseByHR = 5,
    }
}