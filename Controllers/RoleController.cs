using Kalbe.Library.Common.EntityFramework.Controllers;
using Kalbe.Library.Common.EntityFramework.Data;
using LogBookAPI.Models;
using LogBookAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;
namespace LogBookAPI.Controllers
{
    [Microsoft.AspNetCore.Authorization.AllowAnonymous]
    [Route("api/[controller]")]
    public class RoleController : SimpleBaseCrudController<Role>
    {
        public RoleController(IRoleServices roleServices) : base(roleServices)
        {
        }
    }
}