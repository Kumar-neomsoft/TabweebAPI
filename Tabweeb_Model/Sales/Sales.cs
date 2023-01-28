using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Tabweeb_Model.Sales
{
    public class Currency
    {
        public int CurrNo { get; set; }
        public string CurrName { get; set; }
        public decimal CurrRate { get; set; }
        public int CurrFracNo { get; set; }
 
    }
    public class WareHouse
    {
        public int WCode { get; set; }
        public string WName { get; set; }
        public int WHGCode { get; set; }
        public int BranchId { get; set; }

    }
    public class BillType
    {
        public int LangId { get; set; }
        public int FlagValue { get; set; }
        public string FlagCode { get; set; }
        public string FlagDesc { get; set; }
        public int DocType { get; set; }

    }
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string FlagCode { get; set; }
        public string FlagDesc { get; set; }
        public int DocType { get; set; }

    }
}
