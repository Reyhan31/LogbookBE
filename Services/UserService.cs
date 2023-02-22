using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Kalbe.Library.Common.EntityFramework.Data;
using LogBookAPI.Interfaces;
using LogBookAPI.Models;
using LogBookAPI.Models.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace LogBookAPI.Services
{
    public class UserService : SimpleBaseCrud<User>, IUserServices
    {
        private readonly ILogger _logger;
        private readonly LogbookContext _dbContext;
        private readonly IConfiguration _configuration;
        public UserService(ILogger logger, LogbookContext dbContext, IConfiguration configuration) : base(logger, dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
            _configuration = configuration;
        }

        public async override Task<User> Save(User data)
        {
            if (data.Password != null)
            {
                data.Password = EncodePasswordToBase64(data.Password);
            }
            else
            {
                data.Password = EncodePasswordToBase64(_configuration.GetValue<string>("AppSettings:DefaultPassword")); // default password 12345678 (base 64 decyption)
            }

            _logger.LogDebug("Creating new " + typeof(User).Name + " data to database");
            _dbContext.Set<User>().Add(data);
            await _dbContext.SaveChangesAsync();

            return data;
        }

        public async override Task<User> Update(User data)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                var user = _dbContext.Users.AsNoTracking().Where(x => x.Id == data.Id).FirstOrDefault();
                if (data.Password != null)
                {
                    if (!user.Password.Equals(data.Password))
                    {
                        data.Password = EncodePasswordToBase64(data.Password);
                    }else{
                        data.Password = user.Password;
                    }
                }
                else
                {
                    data.Password = user.Password;
                }

                _dbContext.Users.Update(data);

                await _dbContext.SaveChangesAsync();

                if (data.RoleId == 3)
                {
                    var response = _dbContext.Logbooks.Where(x => x.UserId == data.Id);

                    foreach (var item in response)
                    {
                        if (item.Status == (int)LogbookStatus.Draft && item.StatusText.Equals(Models.Constant.DefaultStatusText))
                        {
                            item.MentorId = (long)data.MentorId;
                        }
                        _dbContext.Logbooks.Update(item);
                    }
                    await _dbContext.SaveChangesAsync();
                }

                await transaction.CommitAsync();

                return data;

            }
            catch (System.Exception)
            {
                transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<User> Authenticate(AuthenticationModel model)
        {
            var user = await _dbContext.Users.Include(x => x.Role).Include(x => x.Logbooks).Include(x => x.Mentor)
                .FirstOrDefaultAsync(x => x.Email == model.UserName);

            if (user != null && user.UserName != null && user.Password != null)
            {
                if (EncodePasswordToBase64(model.Password).Equals(user.Password)) // encode password dari request, kemudian compare dengan password user di db (khusus jika password di encrypt)
                {
                    //create claims details based on the user information
                    var token = GenerateJwtToken(user);

                    if (token != null && !token.Equals(string.Empty))
                    {
                        return new AuthenticationResponse(user, token);
                    }
                }
            }

            return null;
        }

        public async Task<PagedList<User>> GetIntern(PagedOptions pagedOptions)
        {
            if (pagedOptions == null)
            {
                pagedOptions = new PagedOptions
                {
                    Page = 1,
                    PageSize = 10,
                    SearchString = "",
                    FilterString = ""
                };
            }
            var datas = _dbContext.Users.AsNoTracking().Where(x => x.RoleId == 3 && x.IsDeleted == false).Include(x => x.Mentor);

            return await PagedList<User>.GetPagedList(datas, pagedOptions);
        }

        public async Task<Models.MentorList> GetMentorByFilter(PagedOptions pagedOptions)
        {
            if (pagedOptions == null)
            {
                pagedOptions = new PagedOptions
                {
                    Page = 1,
                    PageSize = 10,
                    SearchString = "",
                    FilterString = ""
                };
            }
            var datas = _dbContext.Users.AsNoTracking().Where(x => x.RoleId == 2 && x.IsDeleted == false);

            List<MentorItem> mentors = new List<MentorItem>();
            var dataList = await PagedList<User>.GetPagedList(datas, pagedOptions);

            foreach (var item in dataList.Data)
            {
                var count = _dbContext.Users.Where(x => x.MentorId == item.Id && x.IsDeleted == false).Count();
                mentors.Add(new MentorItem(item, count));
            }

            return new MentorList
            {
                Data = mentors,
                CurrentPage = dataList.CurrentPage,
                PageSize = dataList.PageSize,
                TotalPage = dataList.TotalPage,
                TotalData = dataList.TotalData,
                HasNext = dataList.HasNext,
                HasPrevious = dataList.HasPrevious
            };
        }

        public async override Task<User> GetById(long id)
        {
            return await _dbContext.Users
                 .AsNoTracking()
                 .Where(x => x.Id == id && !x.IsDeleted)
                 .Include(x => x.Logbooks)
                 .Include(x => x.Role)
                 .FirstOrDefaultAsync();
        }

        private string GenerateJwtToken(User user)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("LogbookJwtSecret"));
                var tokenDescriptor = GenerateTokenDescriptor(key, user);
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
            catch
            {
                return string.Empty;
            }
        }

        private static SecurityTokenDescriptor GenerateTokenDescriptor(byte[] key, User user)
        {
            return new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim("UserId", user.Id.ToString()),
                    new Claim("RoleId", user.RoleId.ToString()),
                    new Claim("UserName", user.UserName),
                    new Claim("Email", user.Email),
                }),
                Expires = DateTime.Now.AddDays(Models.Constant.TokenExpiryDate),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
        }

        private string EncodePasswordToBase64(string password)
        {
            try
            {
                byte[] encData_byte = new byte[password.Length];
                encData_byte = Encoding.UTF8.GetBytes(password);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }
    }
}