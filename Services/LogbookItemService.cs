using LogBookAPI.Models;
using Kalbe.Library.Common.EntityFramework.Data;
using LogBookAPI.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LogBookAPI.Services
{
    public class LogbookItemService : SimpleBaseCrud<LogbookItem>, ILogbookItemService
    {
        private readonly ILogger _logger;
        private readonly LogbookContext _dbContext;

        public LogbookItemService(ILogger logger, LogbookContext dbContext) : base(logger, dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public int getSumWorkFromOffice(long id)
        {
            var tes = _dbContext.LogbookItems.Where(x => x.LogbookId == id && x.isWorkFromOffice == true && !x.Activity.Equals("OFF")).Count();
            return tes;
        }

        public int getSumWorkFromHome(long id)
        {
            var tes = _dbContext.LogbookItems.Where(x => x.LogbookId == id && x.isWorkFromOffice == false && !x.Activity.Equals("OFF")).Count();
            return tes;
        }
    }
}