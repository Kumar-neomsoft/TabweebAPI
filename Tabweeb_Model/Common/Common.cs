
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Tabweeb_Model.Common.commonclass;

namespace Tabweeb_Model
{
    public class ErrorLog
    {
        public Int32 Id { get; set; }
        public String UserName { get; set; }
        public String DateTime { get; set; }
        public String ErrorLocation { get; set; }
        public String ErrorType { get; set; }
        public String ErrorDescription { get; set; }
    }
    public class sqlError
    {
        public string ErrorNumber { get; set; }
        public string ErrorValue { get; set; }
    }
}
