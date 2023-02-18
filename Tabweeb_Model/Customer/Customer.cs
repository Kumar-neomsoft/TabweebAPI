using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
namespace Tabweeb_Model
{
    public class GetCustomerReq
    {
        public string? C_CODE { get; set; }
        public string? C_A_NAME { get; set; }
        public string? C_E_NAME { get; set; }
        public string? C_A_CODE { get; set; }
        public int? C_BRN_NO { get; set; }
        [DefaultValue(1)]
        public int RecordFrom { get; set; }
        [DefaultValue(10)]
        public int? RecordTo { get; set; }

    }
    public class GetCustomerRes
    {
        public string C_CODE { get; set; }
        public string C_A_NAME { get; set; }
        public string C_E_NAME { get; set; }
        public string C_A_CODE { get; set; }
        public Boolean INACTIVE_SALE { get; set; }
        public int C_BRN_NO { get; set; }
        public string C_TAX_NUMBER { get; set; }
        public string C_ID_NUMBER { get; set; }
        public float SELL_LIMIT { get; set; }
        public string BRN_LNAME { get; set; }
        public int TotalRowCount { get; set; }
    }
}
