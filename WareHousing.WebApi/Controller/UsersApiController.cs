using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WareHousingWebApi.Common.PublicTools;
using WareHousingWebApi.Data.Services.Interface;
using WareHousingWebApi.Entities.Entities;
using WareHousingWebApi.Entities.Models;
using WareHousingWebApi.WebFramework.ApiResult;

namespace WareHousing.WebApi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

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
            var data = _context.usersUw.Get();
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

            var user =  _context.usersUw.Get();
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
                var mUser = _mapper.Map(model, new Users());

                var result = await _userManager.CreateAsync(mUser, "123456");

                //دادن نقش
                if (result.Succeeded)
                {
                    if (mUser.UserType == 1)
                        await _userManager.AddToRoleAsync(mUser, "admin");
                    else
                        await _userManager.AddToRoleAsync(mUser, "user");


                    // ثبت انبار
                    foreach (var wareHouseIds in model.WareHouseIds)
                    {
                        var userInWareHouseId = new UserInWareHouse()
                        {
                            WareHouseId = wareHouseIds,
                            UserIdInWareHouse = mUser.Id,
                            CreateDateTime = DateTime.Now.ToString(),
                            //کاربر ثبت کننده
                            UserId = model.UserId
                        };

                        await _context.userInWareHouseUW.Create(userInWareHouseId);
                    }


                    await _context.SaveAsync();

                    return new ApiResponse<Users>()
                    {
                        flag = true,
                        Data = mUser,
                        StatusCode = ApiStatusCode.Success,
                        Message = ApiStatusCode.Success.GetEnumDisplayName()
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
                    //edit UserwareHouse

                    _context.userInWareHouseUW.DeleteByRange(_context.userInWareHouseUW.Get(c => c.UserIdInWareHouse == model.Id));

                    foreach (var wareHouseId in model.WareHouseIds)
                    {
                        var _userInwareHouse = new UserInWareHouse()
                        {
                            UserIdInWareHouse = getUser.Id,
                            WareHouseId = wareHouseId, 
                            CreateDateTime = DateTime.Now.ToString(),
                            UserId = model.UserId,
                        };
                        await _context.userInWareHouseUW.Create(_userInwareHouse);
                    }


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

        /// <summary>
        /// با دریافت بوز ای دی لست انیار هایی که کاربر دسترسی دارد را در قالب لیست از انبار ای دی برمیگرداند
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        [HttpGet("UserWareHouseDropDown/{userid}")]
        public async Task<ApiResponse> GetUserWareHouseInDropDown([FromRoute] string userid)
        {

            if (userid is null)
                return new ApiResponse()
                {
                    flag = false,
                    StatusCode = ApiStatusCode.NotFound,
                    Message = ApiStatusCode.NotFound.GetEnumDisplayName(),
                };


            // list<int> warehouseId

            var _wareHouseIds = await _context.userInWareHouseUW
                .GetEn
                .Where(c => c.UserIdInWareHouse == userid)
                .Select(c => c.WareHouseId)
                .ToListAsync();

            return new ApiResponse<List<int>>()
            {
                flag = true,
                Data = _wareHouseIds,
                StatusCode = ApiStatusCode.Success,
                Message = ApiStatusCode.Success.GetEnumDisplayName(),
            };
        }

        [HttpGet("GetAccessUser")]
        public async Task<ApiResponse> GetAccessUser([FromQuery] string userId)
        {
            if(string.IsNullOrWhiteSpace(userId))
                return new ApiResponse()
                {
                    flag = false,
                    StatusCode = ApiStatusCode.NotFound,
                    Message = ApiStatusCode.NotFound.GetEnumDisplayName(),
                };

            try
            {
                var _user= await _userManager.FindByIdAsync(userId);
                var _role = await _userManager.GetRolesAsync(_user);
                return new ApiResponse<List<string>>()
                {
                    flag = true,
                    Data = _role.ToList(),
                    StatusCode = ApiStatusCode.Success,
                    Message = ApiStatusCode.Success.GetEnumDisplayName(),
                };

            }
            catch (Exception)
            {
                return new ApiResponse()
                {
                    flag = false,
                    StatusCode = ApiStatusCode.NotFound,
                    Message = ApiStatusCode.NotFound.GetEnumDisplayName(),
                };
            }

        }


        [HttpPost("InsertAccess")]
        public async Task<ApiResponse> InsertAccess([FromForm] UserAccess model)
        {
            if(string.IsNullOrWhiteSpace(model.UserIdAs))
                return new ApiResponse()
                {
                    flag = false,
                    StatusCode = ApiStatusCode.ServerError,
                    Message = ApiStatusCode.ServerError.GetEnumDisplayName(),
                };

            try
            {
                // دریافت اطلاعات user
                var _user = await _userManager.FindByIdAsync(model.UserIdAs);

                //تقش های کاربر
                var _userRoles = await _userManager.GetRolesAsync(_user);

                //حدف همه نقش ها
               var _result = await _userManager.RemoveFromRolesAsync(_user,_userRoles);

               //ثبت جدید دسترسی
               if (model.CreateInvoice) await _userManager.AddToRoleAsync(_user, "CreateInvoice");
               if (model.InvoiceList) await _userManager.AddToRoleAsync(_user, "InvoiceList");
               if (model.Inventory) await _userManager.AddToRoleAsync(_user, "Inventory");
               if (model.InvoiceSeparation) await _userManager.AddToRoleAsync(_user, "InvoiceSeparation");
               if (model.WareHousingHandle) await _userManager.AddToRoleAsync(_user, "WareHousingHandle");
               if (model.ProductFlow) await _userManager.AddToRoleAsync(_user, "ProductFlow");
               if (model.AllProductInvoiced) await _userManager.AddToRoleAsync(_user, "AllProductInvoiced");
               if (model.RiallyStock) await _userManager.AddToRoleAsync(_user, "RiallyStock");
               if (model.WastageRiallyStock) await _userManager.AddToRoleAsync(_user, "WastageRiallyStock");
               if (model.ProductLocation) await _userManager.AddToRoleAsync(_user, "ProductLocation");
               if (model.ProductPrice) await _userManager.AddToRoleAsync(_user, "ProductPrice");

               await _userManager.AddToRoleAsync(_user, "user");
               return new ApiResponse()
               {
                   flag = true,
                   StatusCode = ApiStatusCode.Success,
                   Message = ApiStatusCode.Success.GetEnumDisplayName(),
               };
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
        }

    }
}






