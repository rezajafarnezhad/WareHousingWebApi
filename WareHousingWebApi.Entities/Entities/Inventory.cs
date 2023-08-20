using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WareHousingWebApi.Entities.Entities;

public class Inventory
{
    [Key]
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int WareHouseId { get; set; }
    public string UserId { get; set; }
    public int FiscalYearId { get; set; }

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
    /// </summary>
    public byte OperationType { get; set; }

    public DateTime ExpireData { get; set; }
    public string Description { get; set; }


    //rel

    [ForeignKey(nameof(WareHouseId))]
    public WareHouse WareHouse { get; set; }

    [ForeignKey(nameof(ProductId))]
    public Products Product { get; set; }

    [ForeignKey(nameof(FiscalYearId))]
    public FiscalYear FiscalYear { get; set; }


    [ForeignKey(nameof(UserId))]
    public Users Users { get; set; }
}