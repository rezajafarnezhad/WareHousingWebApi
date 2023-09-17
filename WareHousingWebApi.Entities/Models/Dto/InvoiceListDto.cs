namespace WareHousingWebApi.Entities.Models.Dto;

public class InvoiceListDto
{
    public int WareHouseId { get; set; }
    public int FiscalYearId { get; set; }
    public string FromDate { get; set; }
    public string ToDate { get; set; }
}