using Kalbe.Library.Common.EntityFramework.Controllers;
using Kalbe.Library.Common.EntityFramework.Data;
using LogBookAPI.Models;
using LogBookAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LogBookAPI.Controllers
{
    [Route("api/[controller]")]
    public class LogbookController : SimpleBaseCrudController<Logbook>
    {
        private readonly ILogbookServices _logbookServices;
        private readonly ILogbookItemService _logbookItemServices;
        public LogbookController(ILogbookServices logbookServices, ILogbookItemService logbookItemService) : base(logbookServices)
        {
            _logbookServices = logbookServices;
            _logbookItemServices = logbookItemService;
        }

        private Approver GetCurrentUser()
        {
            var claims = HttpContext?.User?.Claims;
            if (claims == null || !claims.Any())
            {
                return null;
            }

            return new Approver
            {
                Id =  int.Parse(claims.FirstOrDefault(x => x.Type.Equals("UserId", StringComparison.InvariantCulture))?.Value),
                RoleId = int.Parse(claims.FirstOrDefault(x => x.Type.Equals("RoleId", StringComparison.InvariantCulture))?.Value),
                UserName = claims.FirstOrDefault(x => x.Type.Equals("UserName", StringComparison.InvariantCulture))?.Value,
                Email = claims.FirstOrDefault(x => x.Type.Equals("Email", StringComparison.InvariantCulture))?.Value,
            };
        }

        [HttpPost("{id}/submit")]
        public async Task<IActionResult> SubmitLogbook(long id)
        {
            var data = await _logbookServices.GetById(id);
            var user = GetCurrentUser();
            if (data == null)
            {
                return NotFound();
            }

            if (user == null)
            {
                return Unauthorized();
            }

            if(user.RoleId != (int) UserRole.Intern){
                return BadRequest("You cannot submit logbook");
            }

            if ((data.Status != (int)LogbookStatus.Draft) && (data.Status != (int)LogbookStatus.ReviseByMentor) && (data.Status != (int)LogbookStatus.ReviseByHR))
            {
                return BadRequest("Logbook Already Submitted");
            }

            //harus check semua logbook item harus diisi (belum dibuat)
            foreach(LogbookItem item in data.items){
                if(item.Activity == null){
                    return BadRequest("All Logbook must be filled");
                }
            }

            if (data.Status == (int)LogbookStatus.ReviseByHR)
            {
                data.Status = (int)LogbookStatus.ApprovedByMentor;
                data.StatusText = Models.Constant.Submitted;
            }
            else
            {
                data.Status = (int)LogbookStatus.Submitted;
                data.StatusText = Models.Constant.Submitted;
            }

            return await base.Put(id, data);
        }

        [HttpPost("{id}/approve")]
        public async Task<IActionResult> ApproveLogbook(long id, [FromBody] Gaji gaji)
        {
            var data = await _logbookServices.GetById(id);
            var user = GetCurrentUser();
            if (data == null)
            {
                return NotFound();
            }

            if (user == null)
            {
                return Unauthorized();
            }

            if(user.RoleId != (int) UserRole.HR && user.RoleId != (int) UserRole.Mentor){
                return BadRequest("You are not allowed to approve this logbook");
            }

            if (data.Status != (int)LogbookStatus.Submitted && data.Status != (int)LogbookStatus.ApprovedByMentor)
            {
                return BadRequest("Logbook must be submitted first");
            }

            // if(user.RoleId == (int) UserRole.HR){
            //     if(gaji == null){

            //     }
            // }

            if (data.Status == (int)LogbookStatus.Submitted)
            {
                if (user.RoleId == (int)UserRole.Mentor && user.Id == data.MentorId)
                {
                    data.Status = (int)LogbookStatus.ApprovedByMentor;
                    data.StatusText = Models.Constant.Approve;
                    data.approve_by_mentor = user.UserName;
                    return await base.Put(id, data);
                }
            }

            if (data.Status == (int)LogbookStatus.ApprovedByMentor)
            {
                if (user.RoleId == (int)UserRole.HR)
                {
                    data.Status = (int)LogbookStatus.ApprovedByHR;
                    data.StatusText = Models.Constant.Approve;
                    data.HRId = user.Id;
                    data.approve_by_hr = user.UserName;
                    data.GajiWFH = gaji.GajiWFH;
                    data.GajiWFO = gaji.GajiWFO;
                    data.GajiTotal = (_logbookItemServices.getSumWorkFromHome(id) * gaji.GajiWFH) + (_logbookItemServices.getSumWorkFromOffice(id) * gaji.GajiWFO);
                    return await base.Put(id, data);
                }
            }

            return BadRequest();

        }

        [HttpPost("{id}/revise")]
        public async Task<IActionResult> reviseLogbook(long id, [FromBody] Revise obj)
        {
            var data = await _logbookServices.GetById(id);
            var user = GetCurrentUser();
            if (data == null)
            {
                return NotFound();
            }

            if (user == null)
            {
                return Unauthorized();
            }

            if(user.RoleId != (int) UserRole.HR && user.RoleId != (int) UserRole.Mentor){
                return BadRequest("You are not allowed to revise this logbook");
            }

            if (data.Status != (int)LogbookStatus.Submitted && data.Status != (int)LogbookStatus.ApprovedByMentor)
            {
                return BadRequest("Logbook must be submitted first");
            }

            if (data.Status == (int)LogbookStatus.Submitted)
            {
                if (user.RoleId != (int)UserRole.Mentor || user.Id != data.MentorId)
                {
                    return BadRequest("You are not allowed to revise this logbook");
                }
                data.Status = (int)LogbookStatus.ReviseByMentor;
                data.StatusText = "Rejected by " + user.UserName;
                data.approve_by_mentor = null;
                data.cancelReason = obj.cancelReason;
                return await base.Put(id, data);
            }

            if (data.Status == (int)LogbookStatus.ApprovedByMentor)
            {
                if (user.RoleId != (int)UserRole.HR)
                {
                    return BadRequest("You are not allowed to revise this logbook");
                }
                data.Status = (int)LogbookStatus.ReviseByHR;
                data.StatusText = "Rejected by " + user.UserName;
                data.approve_by_hr = null;
                data.cancelReason = obj.cancelReason;
                return await base.Put(id, data);
            }

            return BadRequest();
        }

        [HttpPost("{id}/reject")]
        public async Task<IActionResult> rejectLogbook(long id, [FromBody] Revise obj)
        {
            var data = await _logbookServices.GetById(id);
            var user = GetCurrentUser();
            if (data == null)
            {
                return NotFound();
            }

            if (user == null)
            {
                return BadRequest();
            }

            if (data.Status != (int)LogbookStatus.Submitted && data.Status != (int)LogbookStatus.ApprovedByMentor)
            {
                return BadRequest("Logbook must be submitted first");
            }

            if(user.RoleId != (int) UserRole.HR && user.RoleId != (int) UserRole.Mentor){
                return BadRequest("You are not allowed to reject this logbook");
            }

            data.Status = (int) LogbookStatus.Draft;
            data.StatusText = Models.Constant.Reject;
            data.cancelReason = obj.cancelReason;
            data.approve_by_hr = null;
            data.approve_by_mentor = null;

            return await base.Put(id, data);
        }
    }
}