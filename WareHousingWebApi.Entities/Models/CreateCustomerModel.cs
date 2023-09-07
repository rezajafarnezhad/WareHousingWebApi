using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WareHousingWebApi.Entities.Models;

public class CreateCustomerModel
{
    public string CustomerFullName { get; set; }
    public string EconomicCode { get; set; }
    public string CustomerTell { get; set; }
    public string CustomerAddress { get; set; }
    public int WareHouseId { get; set; }
    public string UserId { get; set; }
}


public class EditCustomerModel : CreateCustomerModel
{
    public int Id { get; set; }

}