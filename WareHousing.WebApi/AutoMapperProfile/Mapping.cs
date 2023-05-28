using AutoMapper;
using WareHousingWebApi.Data.Entities;
using WareHousingWebApi.Data.Models;

namespace WareHousing.WebApi.AutoMapperProfile
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            this.CreateMap<EditUser,Users>().ReverseMap();
            this.CreateMap<CrateUser, Users>().ReverseMap();
            this.CreateMap<ProductEditModel, Products>().ReverseMap();
            this.CreateMap<ProductCreatModel, Products>().ReverseMap();



        }

    }
}
