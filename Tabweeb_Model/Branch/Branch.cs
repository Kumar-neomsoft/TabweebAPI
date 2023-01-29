using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Tabweeb_Model
{
    public class BranchDetails
    {
        public int BranchId { get; set; }
        public int CompanyId { get; set; }
        public int GroupId { get; set; }
        public string BranchLName { get; set; }
        public string BranchFName { get; set; }
        public string BranchYear { get; set; }
        public string LocalCurrency { get; set; }
        public string BranchLAddress { get; set; }
        public string BranchFAddress { get; set; }
        public string BranchLNot { get; set; }
        public int TaxType { get; set; }
        public string SupplierCity { get; set; }
        public string SupplierStreet { get; set; }
        public string SupplierIdentificationCode { get; set; }
        public int Aduid { get; set; }
        public int Upuid { get; set; }
        public string AdTrmnlname { get; set; }
        public string UpTrmnlname { get; set; }
    }
}
