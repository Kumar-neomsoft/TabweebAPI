using Tabweeb_Model;
using TabweebAPI.Repository;
using TabweebAPI.Common;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;
using System.Xml;
using System.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using System.Web;
using Microsoft.Extensions.Primitives;
using TabweebAPI.DBHelper;
using TabweebAPI.IRepository;
using System.Reflection.Metadata;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static Tabweeb_Model.Common.commonclass;

namespace TabweebAPI.Middleware
{
    public class JwtMiddleware 
    {
        #region "Declarations"
        private readonly ILoginRepository _loginRepository;
        private readonly CommonController _commonController;
        private string _Secretkey = string.Empty;
        private string _issuer = string.Empty;
        private string _audience = string.Empty;
        #endregion
        #region "Constructor"
        public JwtMiddleware(IConfiguration iconfig)
        {
            _loginRepository = new LoginRepository(iconfig);
         
        }
        #endregion
        [System.Serializable]
        public class HttpException : System.Runtime.InteropServices.ExternalException
        {
            private int v1;
            private string v2;

            public HttpException(int v1, string v2)
            {
                this.v1 = v1;
                this.v2 = v2;
            }
        }
        public async Task<List<LoginResponse>> AuthenticateUser(LoginRequest LoginReq)
        {
            
            _Secretkey = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("Jwt")["SecretKey"];
            _issuer= new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("Jwt")["issuer"];
            _audience= new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("Jwt")["audience"];
            List<LoginResponse> loginResponse = new List<LoginResponse>();
            loginResponse =  _loginRepository.ValidateUser(LoginReq);
            
            if (loginResponse == null )
            {
                return loginResponse;
            }
            if(loginResponse.Count == 0)
            {
                return loginResponse;
            }
            if (LoginReq.UserName == loginResponse[0].UserID.ToString())
            {
                var SecretKey = _Secretkey;
                var SymmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
                var siginigcredentials = new SigningCredentials(SymmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);
                var JWToken = new JwtSecurityToken(
                   issuer: _issuer,
                   audience: _audience,
                   claims: GetUserClaims(loginResponse[0]),
                   expires: new DateTimeOffset(DateTime.Now.AddMinutes(30)).DateTime,
                   //Using HS256 Algorithm to encrypt Token - JRozario
                   //signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                   signingCredentials: siginigcredentials
               );
                int UserID = (Int32)loginResponse[0].UserID;
                var token = new JwtSecurityTokenHandler().WriteToken(JWToken);
                //Token = token;
                loginResponse[0].Token = Convert.ToString(token);
                long UPresult = 0;
                UPresult =  _loginRepository.UpdateLoginSession(UserID, token);
                return loginResponse;
            }
            else
            {
                return loginResponse;
            }


            throw new NotImplementedException();
        }

        public async Task<List<string>> AuthenticateUser1(LoginRequest LoginReq)
        {

            _Secretkey = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("Jwt")["SecretKey"];
            _issuer = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("Jwt")["issuer"];
            _audience = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("Jwt")["audience"];
            List<LoginResponse> loginResponse = new List<LoginResponse>();
            loginResponse = _loginRepository.ValidateUser(LoginReq);

            if (loginResponse == null)
            {
                return null;
            }
            if (loginResponse.Count == 0)
            {
                return null;
            }
            if (LoginReq.UserName == loginResponse[0].UserID.ToString())
            {
                var SecretKey = _Secretkey;
                var SymmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
                var siginigcredentials = new SigningCredentials(SymmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);
                var JWToken = new JwtSecurityToken(
                   issuer: _issuer,
                   audience: _audience,
                   claims: GetUserClaims(loginResponse[0]),
                   expires: new DateTimeOffset(DateTime.Now.AddMinutes(30)).DateTime,
                   //Using HS256 Algorithm to encrypt Token - JRozario
                   //signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                   signingCredentials: siginigcredentials
               );
                int UserID = (Int32)loginResponse[0].UserID;
                var token = new JwtSecurityTokenHandler().WriteToken(JWToken);
                //Token = token;
                loginResponse[0].Token = Convert.ToString(token);
                long UPresult = 0;
                UPresult = _loginRepository.UpdateLoginSession(UserID, token);

                //var list1 = ((IEnumerable)loginResponse).OfType<object>();
                //List<string> strings = list1.Select(x => x.ToString()).ToList();

                List<string> list = new List<string>();
                list = loginResponse.Cast<string>().ToList();
               // List<string> list = (string)loginResponse.ToList();
                return list;
            }
            else
            {
                return null;
            }


            throw new NotImplementedException();
        }
        public string ValidateJWTToken(List<KeyValuePair<string, StringValues>> name)
        {
            string vAccessToken = "";
            String token = "";

            foreach (var header in name)
            {
                if (header.Key.Equals("Authorization"))
                {
                    token = header.Value;
                }
              
            }

            if (token == "No Token" || token =="")
            {
                //throw new HttpException(401, "Unauthorized access");
                return "unauthorized";
            }

            int index1 = token.IndexOf("Bearer ");
            if (index1 != -1)
            {
                vAccessToken = token.Remove(0, 7);
            }
            SessionResponse sessionResponse = new SessionResponse();
            var handler = new JwtSecurityTokenHandler();
            var tokenS = handler.ReadToken(vAccessToken) as JwtSecurityToken;
            var userId = tokenS.Claims.First(claim => claim.Type == "USER_ID").Value;
            int U_id = Convert.ToInt32(userId);
            sessionResponse = _loginRepository.SessionResponse(U_id);
            var sessiondatetime = Convert.ToDateTime(sessionResponse.SessionTimeOut).AddHours(8);
            if (vAccessToken != null && vAccessToken == sessionResponse.Token && sessiondatetime >= DateTime.Now)
            {
                _loginRepository.RefreshLoginSession(U_id);
                return userId;
            }
            else
            {
                return "unauthorized";
            }
        }
        private IEnumerable<Claim> GetUserClaims(LoginResponse user)
        {
            IEnumerable<Claim> claims = new Claim[]
                    {
                        new Claim(ClaimTypes.Name, user.Name),
                        new Claim("USER_ID",Convert.ToString(user.UserID)),
                        new Claim("UserName",Convert.ToString(user.Name)),


                    };
            return claims;
        }
    }
}
