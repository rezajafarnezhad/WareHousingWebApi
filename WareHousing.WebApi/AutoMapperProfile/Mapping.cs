using AutoMapper;
using WareHousingWebApi.Common.PublicTools;
using WareHousingWebApi.Entities.Entities;
using WareHousingWebApi.Entities.Models;


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

            this.CreateMap<ProductPrice,CreateProductPrice>().ReverseMap()
                .ForMember(c => c.ActionDate,
                    op =>
                        op.MapFrom(x => x.ActionDate.ConvertShamsiToMiladi()))

                ;


            this.CreateMap<CreateProductLocation,ProductLocation>().ReverseMap();
            this.CreateMap<EditProductLocation, ProductLocation>().ReverseMap();



        }

    }
}
