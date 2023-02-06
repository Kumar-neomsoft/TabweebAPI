using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tabweeb_Model
{
    public class ProductSearch
    {
        public string I_CODE { get; set; }
        public string I_NAME { get; set; }
        public string ITM_UNT { get; set; }
        public decimal I_PRICE { get; set; }
        public double AVL_QTY { get; set; }
        public int W_CODE { get; set; }
    }
    public class ProductSearchReq
    {
        public string? I_CODE { get; set; }
        public int? W_CODE { get; set; }
    }
}
