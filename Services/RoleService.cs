using LogBookAPI.Models;
using Kalbe.Library.Common.EntityFramework.Data;
using Microsoft.EntityFrameworkCore;
using LogBookAPI.Interfaces;

namespace LogBookAPI.Services
{
    public class RoleService : SimpleBaseCrud<Role>, IRoleServices
    {
        private readonly ILogger _logger;
        private readonly LogbookContext _dbContext;

        public RoleService(ILogger logger, LogbookContext dbContext) : base(logger, dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }
    }
}