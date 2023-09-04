using AutoMapper;
using WareHousingWebApi.Common.PublicTools;
using WareHousingWebApi.Entities.Entities;
using WareHousingWebApi.Entities.Models;
using WareHousingWebApi.Entities.Models.Dto;


namespace WareHousing.WebApi.AutoMapperProfile
{
    public class Mapping : Profile
    {
        public Mapping()
        {

            var date = DateTime.Now.ToString();
            this.CreateMap<EditUser,Users>().ReverseMap();
            this.CreateMap<CrateUser, Users>().ReverseMap();

            this.CreateMap<ProductEditModel, Products>().ReverseMap();

            
            this.CreateMap<ProductCreatModel, Products>()
                .ForMember(c=>c.CreateDateTime , 
                    op=> op.MapFrom(x=>DateTime.Now.ToString())).ReverseMap();
               

            this.CreateMap<CreateFiscalYear, FiscalYear>()

                .ForMember(c=>c.StartDate ,
                    op=>
                        op.MapFrom(x=>x.StartDate.ConvertShamsiToMiladi()))

                .ForMember(c => c.EndDate,
                    op =>
                        op.MapFrom(x => x.EndDate.ConvertShamsiToMiladi()))

                .ReverseMap();



            this.CreateMap<ProductPrice,CreateProductPrice>().ReverseMap()
                .ForMember(c => c.ActionDate,
                    op =>
                        op.MapFrom(x => x.ActionDate.ConvertShamsiToMiladi()))

                ;


            this.CreateMap<CreateProductLocation,ProductLocation>().ReverseMap();
            this.CreateMap<EditProductLocation, ProductLocation>().ReverseMap();
            
            this.CreateMap<Inventory,AddProductStock>().ReverseMap()
                .ForMember(c=>c.ExpireData,
                    op=>op.MapFrom(x=>x.ExpireData.ConvertShamsiToMiladi()))
                
                .ForMember(c => c.OperationDate,
                    op => op.MapFrom(x => x.OperationDate.ConvertShamsiToMiladi()))
                ;
          
            
            this.CreateMap<Inventory,ExitStockModel>().ReverseMap()
                .ForMember(c => c.OperationDate,
                    op => op.MapFrom(x => x.OperationDate.ConvertShamsiToMiladi()))
                ;

            this.CreateMap<Inventory, WastageStockModel>().ReverseMap()
                .ForMember(c => c.OperationDate,
                    op => op.MapFrom(x => x.OperationDate.ConvertShamsiToMiladi()))
   
                ;
            
            this.CreateMap<Inventory, BackWastageStockModel>().ReverseMap()
                .ForMember(c => c.OperationDate,
                    op => op.MapFrom(x => x.OperationDate.ConvertShamsiToMiladi()))
   
                ;

            this.CreateMap<Inventory, ProductFlowReplyDto>().ReverseMap()
                
                ;
        }

    }
}
