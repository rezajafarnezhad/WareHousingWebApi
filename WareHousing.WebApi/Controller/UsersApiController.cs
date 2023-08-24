using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WareHousingWebApi.Common.PublicTools;
using WareHousingWebApi.Data.Services.Interface;
using WareHousingWebApi.Entities.Entities;
using WareHousingWebApi.Entities.Models;
using WareHousingWebApi.WebFramework.ApiResult;

namespace WareHousing.WebApi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersApiController : ControllerBase
    {
        private readonly IUnitOfWork _context;
        private readonly UserManager<Users> _userManager;
        private readonly IMapper _mapper;
        public UsersApiController(IUnitOfWork unitOfWork, UserManager<Users> userManager, IMapper mapper)
        {
            _context = unitOfWork;
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ApiResponse> Get()
        {
            var data =  await _context.usersUw.Get();
            return new ApiResponse<IEnumerable<Users>>()
            {
                flag = true,
                Data = data,
                StatusCode = ApiStatusCode.Success,
                Message = ApiStatusCode.Success.GetEnumDisplayName(),

            };
        }

        [HttpGet("{userId}")]
        public async Task<ApiResponse> GetById([FromRoute] string userId)
        {
            var user = await _context.usersUw.GetById(userId);
            return user != null
                ? new ApiResponse<Users>
                {
                    flag = true,
                    Data = user,
                    StatusCode = ApiStatusCode.Success,
                    Message = ApiStatusCode.Success.GetEnumDisplayName()
                }
                : new ApiResponse()
                {
                    flag = false,
                    StatusCode = ApiStatusCode.NotFound,
                    Message = ApiStatusCode.NotFound.GetEnumDisplayName()
                };
        }



        [HttpPost]
        public async Task<ApiResponse> Create([FromForm] CrateUser model)
        {
            if (!ModelState.IsValid) return new ApiResponse()
            {
                flag = false,
                StatusCode = ApiStatusCode.BadRequest,
                Message = ApiStatusCode.BadRequest.GetEnumDisplayName(),

            };

            var user = await _context.usersUw.Get();
            if (user.Any(c => c.PhoneNumber == model.PhoneNumber || c.MelliCode == model.MelliCode))
                return new ApiResponse()
                {
                    flag = false,
                    StatusCode = ApiStatusCode.DuplicateInformation,
                    Message = ApiStatusCode.DuplicateInformation.GetEnumDisplayName(),

                };

            try
            {

                model.BirthDayDate = model.BirthDayDate.ConvertShamsiToMiladi().ToString();
                var mUser = _mapper.Map(model,new Users());

                var result = await _userManager.CreateAsync(mUser, "123456");

                //دادن نقش
                if (result.Succeeded)
                {
                    if (mUser.UserType == 1)
                        await _userManager.AddToRoleAsync(mUser, "admin");
                    else
                        await _userManager.AddToRoleAsync(mUser, "user");

                    await _context.SaveAsync();
                    return new ApiResponse<Users>()
                    {
                        flag = true,
                        Data = mUser,
                        StatusCode = ApiStatusCode.Success,
                        Message = ApiStatusCode.Success.GetEnumDisplayName(),

                    };
                }
                else
                {
                    return new ApiResponse()
                    {
                        flag = false,
                        StatusCode = ApiStatusCode.BadRequest,
                        Message = ApiStatusCode.BadRequest.GetEnumDisplayName(),

                    };
                }

            }
            catch
            {
                return new ApiResponse()
                {
                    flag = false,
                    StatusCode = ApiStatusCode.ServerError,
                    Message = ApiStatusCode.ServerError.GetEnumDisplayName(),

                };
            }

        }

        [HttpPut]
        public async Task<ApiResponse> Edit([FromForm] EditUser model)
        {
            if (string.IsNullOrWhiteSpace(model.Id)) return new ApiResponse()
            {
                flag = false,
                StatusCode = ApiStatusCode.NotFound,
                Message = ApiStatusCode.NotFound.GetEnumDisplayName(),

            };

            if (!ModelState.IsValid) return new ApiResponse()
            {
                flag = false,
                StatusCode = ApiStatusCode.BadRequest,
                Message = ApiStatusCode.BadRequest.GetEnumDisplayName(),

            };

            try
            {
                var getUser = await _userManager.FindByIdAsync(model.Id);
                if (getUser != null)
                {
                    model.BirthDayDate = model.BirthDayDate.ConvertShamsiToMiladi().ToString();
                    var mUser = _mapper.Map(model, getUser);
                    var result = await _userManager.UpdateAsync(mUser);
                    if (result.Succeeded)
                        return new ApiResponse()
                        {
                            flag = true,
                            StatusCode = ApiStatusCode.Success,
                            Message = ApiStatusCode.Success.GetEnumDisplayName(),

                        };
                }

            }
            catch (Exception)
            {
                return new ApiResponse()
                {
                    flag = false,
                    StatusCode = ApiStatusCode.ServerError,
                    Message = ApiStatusCode.ServerError.GetEnumDisplayName(),

                };
            }
            return new ApiResponse()
            {
                flag = false,
                StatusCode = ApiStatusCode.ServerError,
                Message = ApiStatusCode.ServerError.GetEnumDisplayName(),

            };

        }



    }
}






