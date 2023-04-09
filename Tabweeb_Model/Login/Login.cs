using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace Tabweeb_Model
{
    public class LoginRequest
    {
        //public string ServerName { get; set; }
        //public string AccountYear { get; set; }
        //public string AccountUnit { get; set; }
        //public int Language { get; set; }
        //public string Company { get; set; }
        [Required(ErrorMessage = "Enter UserName")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Enter Password")]
        public string Password { get; set; }
        //[Required(ErrorMessage = "Select Branch Name")]
        //public string BranchName { get; set; }

    }
    public class LoginResponse
    {
        public int UserID { get; set; }

        public string Name { get; set; }
        public string Password { get; set; }
        public int UMngr { get; set; }
        public bool InActive { get; set; }

        public int GroupNo { get; set; }
        public bool LoginMethod { get; set; }
        public int BranchID { get; set; }
        public int CompanyID { get; set; }
        public string UnitYear { get; set; }
        public string UnitNumber { get; set; }
        public int LangID { get; set; }
        public string Token { get; set; }
    }

    public class TokenResponse
    {
        public string Token { get; set; }
    }
    public class SessionResponse
    {
        public int UserId { get; set; }
        public string Token { get; set; }
        public string SessionTimeOut { get; set; }
    }
}
