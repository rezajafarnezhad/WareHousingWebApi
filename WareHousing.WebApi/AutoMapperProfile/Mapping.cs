using AutoMapper;
using WareHousing.WebApi.Tools;
using WareHousingWebApi.Data.Entities;
using WareHousingWebApi.Data.Models;
using WareHousingWebApi.Data.PublicTools;


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


            var date = DateTime.Now.ToString();
            this.CreateMap<CreateFiscalYear, FiscalYear>()

                .ForMember(c=>c.StartDate ,
                    op=>
                        op.MapFrom(x=>x.StartDate.ConvertShamsiToMiladi()))

                .ForMember(c => c.EndDate,
                    op =>
                        op.MapFrom(x => x.EndDate.ConvertShamsiToMiladi()))

                .ReverseMap();


            this.CreateMap<WareHouse,CreateWareHouse>().ReverseMap();
        }

    }
}
