using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Tabweeb_Model
{
    public class WareHouse
    {
        public int WCode { get; set; }
        public string WName { get; set; }
        public int WHGCode { get; set; }
        public int BranchId { get; set; }

    }
}
