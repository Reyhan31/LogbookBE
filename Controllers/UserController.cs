using Kalbe.Library.Common.EntityFramework.Controllers;
using Kalbe.Library.Common.EntityFramework.Data;
using LogBookAPI.Models;
using LogBookAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LogBookAPI.Controllers
{
    [Microsoft.AspNetCore.Authorization.AllowAnonymous]
    [Route("api/[controller]")]

    public class UserController : SimpleBaseCrudController<User>
    {
        private readonly IUserServices _userService;
        private readonly ILogbookServices _logbookServices;
        private readonly ILogbookItemService _logbookItemService;
        public UserController(IUserServices userService, ILogbookServices logbookServices, ILogbookItemService logbookItemService) : base(userService)
        {
            _userService = userService;
            _logbookServices = logbookServices;
            _logbookItemService = logbookItemService;
        }

        public async override Task<IActionResult> Post([FromBody] User data)
        {
            var allData = await _userService.GetAll();
            var sameEmail = allData.Where(x => x.Email == data.Email).FirstOrDefault();

            if (sameEmail != null)
            {
                return BadRequest("Email already registered");
            }

            if(data.RoleId == (int)UserRole.Intern && data.EntryDate > data.EndDate){
                return BadRequest("Entry Date must be smaller than End Date");
            }

            var response = await _userService.Save(data);

            if (response == null)
            {
                return BadRequest();
            }

            if (data.RoleId == (int)UserRole.Intern)
            {
                var month = data.EntryDate.Month;
                var year = data.EntryDate.Year;

                while (true)
                {
                    if (month > 12)
                    {
                        year++;
                        month = 1;
                    }

                    if(year > data.EndDate.Year){
                        break;
                    }

                    if(year == data.EndDate.Year && month > data.EndDate.Month){
                        break;
                    }

                    var stringMonth = getMonth(month);
                    var newLogbook = await _logbookServices.Save(new Logbook
                    {
                        UserId = response.Id,
                        MentorId = (long)response.MentorId,
                        Status = (int)LogbookStatus.Draft,
                        StatusText = Models.Constant.DefaultStatusText,
                        monthLogbook = stringMonth,
                        yearLogbook = year,
                    });

                    //insert logbook item
                    List<DateTime> dateList = getDates(year, month);
                    foreach (var dateItem in dateList)
                    {
                        if(DateTime.Compare(dateItem, response.EndDate) > 0 ){
                            break;
                        }

                        if (dateItem < response.EntryDate)
                        {
                            continue;
                        }
                        await _logbookItemService.Save(new LogbookItem
                        {
                            LogbookId = newLogbook.Id,
                            Date = dateItem,
                        });
                    }

                    month++;
                }

            }
            return Ok(response);

        }

        private string getMonth(int idx)
        {
            string[] months = {"January", "February", "March", "April", "May",
    "June", "July", "August", "September", "October", "November", "December"};

            return months[idx - 1];

        }

        private List<DateTime> getDates(int year, int month)
        {
            return Enumerable.Range(1, DateTime.DaysInMonth(year, month))  // Days: 1, 2 ... 31 etc.
                             .Select(day => new DateTime(year, month, day)) // Map each day to a date
                             .Where(day => day.DayOfWeek != DayOfWeek.Sunday && day.DayOfWeek != DayOfWeek.Saturday)
                             .ToList(); // Load dates into a list
        }

        public async override Task<IActionResult> Put(long id, [FromBody] User data)
        {
            if (data.Password == null)
            {
                return BadRequest("Password must be filled");
            }

            if (id != data.Id)
            {
                return BadRequest();
            }

            if (await _userService.GetById(data.Id) == null)
            {
                return NotFound();
            }

            await _userService.Update(data);

            return Ok(await _userService.GetById(data.Id));
        }

        [HttpGet("Mentor")]
        public async Task<IActionResult> GetMentor()
        {
            var pagedOptions = Kalbe.Library.Common.EntityFramework.Utils.GetPagedOptions(Request);

            return Ok(await _userService.GetMentorByFilter(pagedOptions));
        }

        [HttpGet("Intern")]
        public async Task<IActionResult> GetIntern()
        {
            var pagedOptions = Kalbe.Library.Common.EntityFramework.Utils.GetPagedOptions(Request);

            return Ok(await _userService.GetIntern(pagedOptions));
        }


    }
}