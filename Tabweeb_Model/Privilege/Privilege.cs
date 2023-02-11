using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace Tabweeb_Model
{
    public class PrivilegeReq
    {
        public int? U_ID { get; set; }
        public int? FORM_NO { get; set; }
    }
    public class PrivilegeRes
    {
        public int U_ID { get; set; }
        public int FORM_NO { get; set; }
        public Boolean INCLUDE_FLAG { get; set; }
        public Boolean AD_FLAG { get; set; }
        public Boolean DEL_FLAG { get; set; }
        public Boolean MOD_FLAG { get; set; }
        public Boolean VIEW_FLAG { get; set; }
        public Boolean PRINT_FLAG { get; set; }
        public Boolean VWREP_FLAG { get; set; }
        public Boolean VRFY_FLAG { get; set; }
        public Boolean PST_FLAG { get; set; }
        public int F_ORDER_NO { get; set; }
        public int SCR_TYP { get; set; }
        public Boolean PERIOD_CLOSE { get; set; }
        public Boolean PAYMENT_DONE { get; set; }
        public Boolean AD_FLAG_FROM { get; set; }
        public Boolean? HUNG { get; set; }
        public Boolean? CONSTRAINT_REVIEW { get; set; }
        public Boolean? BARCODE_PRINTING { get; set; }
        public Boolean? SAVE_FLAG { get; set; }
        public Boolean? EXPORT_FLAG { get; set; }
        public Boolean? IMPORT_FLAG { get; set; }
        public Boolean? FIRST_FLAG { get; set; }
        public Boolean? NEXT_FLAG { get; set; }
        public Boolean? PREVIOUS_FLAG { get; set; }
        public Boolean? LAST_FLAG { get; set; }
        public Boolean? ARCHIVE_FLAG { get; set; }

    }
}
