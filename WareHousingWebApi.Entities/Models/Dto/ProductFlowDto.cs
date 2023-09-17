using System.ComponentModel.DataAnnotations;

namespace WareHousingWebApi.Entities.Models.Dto;

public class ProductFlowDto
{
    public int WareHouseId { get; set; }
    public int FiscalYearId { get; set; }
    public int ProductId { get; set; }
    public string FromDate { get; set; }
    public string ToDate { get; set; }
}
public class ProductFlowReplyDto
{
    public byte OperationType { get; set; }
    public DateTime OperationDate { get; set; }
    public DateTime ExpireData { get; set; }
    
    /// <summary>
    /// تعداد تراکنش کالا در انبار اصلی
    /// </summary>
    public int ProductCountMain { get; set; }

    /// <summary>
    /// تعداد تراکنش کالا در انبار ضایعات
    /// </summary>

    public int ProductWastage { get; set; }
    public string Description { get; set; }
    public string UsersFullName { get; set; }

}