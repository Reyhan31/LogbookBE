using LogBookAPI.Models;
using Kalbe.Library.Common.EntityFramework.Data;
using LogBookAPI.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace LogBookAPI.Services
{
    public class LogbookService : SimpleBaseCrud<Logbook>, ILogbookServices
    {
        private readonly ILogger _logger;
        private readonly LogbookContext _dbContext;
        public LogbookService(ILogger logger, LogbookContext dbContext) : base(logger, dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async override Task<Logbook> GetById(long id)
        {
            return await _dbContext.Logbooks
                 .AsNoTracking()
                 .Where(x => x.Id == id && !x.IsDeleted)
                 .Include(x => x.items)
                 .FirstOrDefaultAsync();
        }
    }
}