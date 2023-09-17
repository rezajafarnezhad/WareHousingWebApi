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
           
            this.CreateMap<CrateUser, Users>()

                .ReverseMap();

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
            
            this.CreateMap<WareHouse, CreateWareHouse>().ReverseMap()
                
                ;
            this.CreateMap<WareHouse, EditWareHouse>().ReverseMap()

                ;


            this.CreateMap<Customer, CreateCustomerModel>()
                .ReverseMap()
                .ForMember(c => c.CreateDateTime,
                    op => op.MapFrom(x => DateTime.Now.ToString())).ReverseMap();
            ;


            this.CreateMap<Invoice, CreateInvoice>()
                .ReverseMap()
                .ForMember(c=>c.InvoiceNumber,
                    op=>op.MapFrom(x=>$"{DateTime.Now.ConvertMiladiToShamsi("yyyy/MM/dd HH:mm:ss")}_InvoiceNumber"))
               
                .ForMember(c => c.CreateDateTime,
                    op => op.MapFrom(x => DateTime.Now.ToString()))
                .ForMember(c => c.Date,
                    op => op.MapFrom(x => DateTime.Now))
                .ForMember(c => c.InvoiceType,
                    op => op.MapFrom(x => 1))
                .ForMember(c => c.InvoiceStatus,
                    op => op.MapFrom(x => 1))
                ;
            ;




            this.CreateMap<Invoice, InvoiceList>()
                .ReverseMap();
          
            this.CreateMap<InvoiceItems, InvoiceItemList>()
                .ReverseMap();

        }

    }
}
