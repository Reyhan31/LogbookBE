using Kalbe.Library.Common.EntityFramework.Data;
using LogBookAPI.Models;
using LogBookAPI.Models.Authentication;

namespace LogBookAPI.Interfaces
{
    public interface IUserServices : ISimpleBaseCrud<User>
    {
        Task<User> Authenticate(AuthenticationModel model);
        Task<MentorList> GetMentorByFilter(PagedOptions pagedOptions);
        Task<PagedList<User>> GetIntern(PagedOptions pagedOptions);
    }
}