using Kalbe.Library.Common.EntityFramework.Data;
using LogBookAPI.Models;
namespace LogBookAPI.Interfaces
{
    public interface ILogbookItemService : ISimpleBaseCrud<LogbookItem>
    {
        public int getSumWorkFromOffice(long id);
        public int getSumWorkFromHome(long id);
    }
}