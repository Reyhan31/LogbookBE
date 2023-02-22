using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Kalbe.Library.Common.EntityFramework.Models;

namespace LogBookAPI.Models
{
    [Table("LogbookItem")]
    public class LogbookItem : Base
    {
        public long LogbookId;
        [ForeignKey("LogbookId")]
        public Logbook? Logbook { get; set; }
        public DateTime Date { get; set; }
        public string? Activity { get; set; }
        public DateTime? JamMasuk { get; set; }
        public DateTime? JamKeluar { get; set; }
        public bool isWorkFromOffice { get; set; } = false;
        public LogbookItem(){
        }

        public LogbookItem(LogbookItem obj){
            LogbookId = obj.LogbookId;
            Logbook = obj.Logbook;
            Date = obj.Date;
            Activity = obj.Activity;
            JamMasuk = obj.JamMasuk;
            JamKeluar = obj.JamKeluar;
            isWorkFromOffice = obj.isWorkFromOffice;
        }
    }
}