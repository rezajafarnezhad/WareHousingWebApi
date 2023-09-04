using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WareHousingWebApi.Entities.Models.Dto;

public class RialStockDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public int ProductCode { get; set; }
    //تعداد موجودی
    /// <summary>
    /// تعداد موجودی
    /// </summary>
    public int TotalProductCount { get; set; }

    //
    /// <summary>
    /// موجودی ریالی کل کالا - خرید
    /// </summary>
    public int TotalPurchasePrice { get; set; }

    //
    /// <summary>
    /// موجودی ریالی کل کالا - فروش
    /// </summary>
    public int TotalSalePrice { get; set; }

   

    /// <summary>
    /// موجودی ریالی کل کالا - مصرف کننده
    /// </summary>
    public int TotalCoverPrice { get; set; }

}