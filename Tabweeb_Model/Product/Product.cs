using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
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

    public class ProductGetReq
    {
        public string? I_CODE { get; set; }
        public int? RecordFrom { get; set; }
        public int? RecordTo { get; set; }
    }
    public class ProductGetRes
    {
        public string I_CODE { get; set; }
        public string I_NAME { get; set; }
        public decimal VAT_PER { get; set; }
        public int VAT_TYPE { get; set; }
        public string ITM_UNT { get; set; }
        public string P_SIZE { get; set; }
        public int TotalRowCount { get; set; }
    }
    public class BarcodeGetReq
    {
        [Required(ErrorMessage = "Enter Product I_Code")]
        public string I_CODE { get; set; }
        public string? ITM_UNT { get; set; }
        public int? RecordFrom { get; set; }
        public int? RecordTo { get; set; }
    }
    public class BarcodeGetRes
    {
        
        public string BARCODE { get; set; }
        public string ITM_UNT { get; set; }
        public string P_SIZE { get; set; }
        public int TotalRowCount { get; set; }
    }
}
