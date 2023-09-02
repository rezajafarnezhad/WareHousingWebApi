using System.ComponentModel.DataAnnotations;

public class AddProductStock
{
    public int ProductId { get; set; }
    public int WareHouseId { get; set; }
    public int FiscalYearId { get; set; }
    public int ProductLocationId { get; set; }
    public string UserId { get; set; }

    [Required(AllowEmptyStrings =false,ErrorMessage ="تعداد وارد شود")]
    [Range(1,int.MaxValue,ErrorMessage ="تعداد کالا به درستی وارد شود")]
    public int ProductCountMain { get; set; }
    [Required(AllowEmptyStrings = false, ErrorMessage = "تاریخ انجام عملیات وارد شود")]

    public string OperationDate { get; set; }
    public byte OperationType { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "تاریخ انقضا وارد شود")]
    public string ExpireData { get; set; }
    public string Description { get; set; }
}

public class ExitStockModel
{
    public int ProductId { get; set; }
    public int WareHouseId { get; set; }
    public int FiscalYearId { get; set; }
    public string UserId { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "تعداد وارد شود")]
    [Range(1, int.MaxValue, ErrorMessage = "تعداد کالا به درستی وارد شود")]
    public int ProductCountMain { get; set; }
    [Required(AllowEmptyStrings = false, ErrorMessage = "تاریخ انجام عملیات وارد شود")]

    public string OperationDate { get; set; }
    public byte OperationType { get; set; }
    public string Description { get; set; }
    public int ReferenceId { get; set; }
    public int ProductLocationId { get; set; }
}


public class WastageStockModel
{
    public int ProductId { get; set; }
    public int WareHouseId { get; set; }
    public int FiscalYearId { get; set; }
    public string UserId { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "تعداد وارد شود")]
    [Range(1, int.MaxValue, ErrorMessage = "تعداد کالا به درستی وارد شود")]
    public int ProductWastage { get; set; }
    [Required(AllowEmptyStrings = false, ErrorMessage = "تاریخ انجام عملیات وارد شود")]

    public string OperationDate { get; set; }
    public byte OperationType { get; set; }
    public string Description { get; set; }
    public int ReferenceId { get; set; }
    public int ProductLocationId { get; set; }
}

public class BackWastageStockModel
{
    public int ProductId { get; set; }
    public int WareHouseId { get; set; }
    public int FiscalYearId { get; set; }
    public string UserId { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "تعداد وارد شود")]
    [Range(1, int.MaxValue, ErrorMessage = "تعداد کالا به درستی وارد شود")]
    public int ProductWastage { get; set; }
    [Required(AllowEmptyStrings = false, ErrorMessage = "تاریخ انجام عملیات وارد شود")]

    public string OperationDate { get; set; }
    public byte OperationType { get; set; }
    public string Description { get; set; }
    public int ReferenceId { get; set; }
    public int ProductLocationId { get; set; }
}