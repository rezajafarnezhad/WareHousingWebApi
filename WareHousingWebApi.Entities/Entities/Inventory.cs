using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Principal;
using WareHousingWebApi.Entities.Base;

namespace WareHousingWebApi.Entities.Entities;

public class Inventory : BaseEntity
{
    [Key]
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int WareHouseId { get; set; }
    public int FiscalYearId { get; set; }
    public int? InvoiceId { get; set; }
    public int ProductLocationId { get; set; }
    //تعداد تراکنش در انبار اصلی
    public int ProductCountMain { get; set; }

    //تعداد تراکنش در انبار ضایعات

    public int ProductWastage { get; set; }
    public DateTime OperationDate { get; set; }

    /// <summary>
    /// 1 = اضاغف به انبار اصلی
    /// 2= کسر از انبار اصبی
    /// 3=اضاف به انبار ضایعات
    /// 4=گسر ار انبار ضایعات
    /// 5 = فروش
    /// 6=مرجوغی
    /// 7=بالانس افزایشی
    /// 8=بالانس کاهشی
    /// 9=انتفالی ار سال مالی جذیذ
    /// </summary>
    public byte OperationType { get; set; }

    public DateTime ExpireData { get; set; }
    public string Description { get; set; }

    public int ReferenceId { get; set; }
    //rel

    [ForeignKey(nameof(WareHouseId))]
    public WareHouse WareHouse { get; set; }

    [ForeignKey(nameof(ProductId))]
    public Products Product { get; set; }

    [ForeignKey(nameof(FiscalYearId))]
    public FiscalYear FiscalYear { get; set; }

    [ForeignKey(nameof(ProductLocationId))]
    public ProductLocation ProductLocation { get; set; }
}