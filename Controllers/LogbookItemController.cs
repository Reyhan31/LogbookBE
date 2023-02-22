using Kalbe.Library.Common.EntityFramework.Controllers;
using Kalbe.Library.Common.EntityFramework.Data;
using LogBookAPI.Models;
using LogBookAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LogBookAPI.Controllers
{
    [Route("api/[controller]")]
    public class LogbookItemController : SimpleBaseCrudController<LogbookItem>
    {
        private readonly ILogbookItemService _logbookItemService;
        public LogbookItemController(ILogbookItemService logbookItemService) : base(logbookItemService)
        {
            _logbookItemService = logbookItemService;
        }
    }
}