using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TabweebAPI.Repository;
using Tabweeb_Model;
using Tabweeb_Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Tabweeb_Model.Common.commonclass;

namespace TabweebAPI.Common
{
    public class SaveStatusDetails
    {
        public int ErrorID { get; set; }
        public string ErrorName { get; set; }
        public string ErrorDescription { get; set; }
    }
    public class CommonController : ControllerBase
    {
        private List<SaveStatusDetails> SaveStatusList = new List<SaveStatusDetails>()
        {
            new SaveStatusDetails {ErrorID=0, ErrorName="Success",ErrorDescription="Success"},
            new SaveStatusDetails {ErrorID=-1, ErrorName="Failure",ErrorDescription="Failed"}
        };

        [NonAction]
        public IActionResult ProcessGetResponse<T>(List<T> result, string HasLimitResponse, CRUDAction CRUDAction)
        {
            try
            {
                ResponseObject<T> objCommonResponse = new ResponseObject<T>();

                if (result.Count > 0)
                {
                    objCommonResponse.Response = new CommonResponse<T>() { Message = string.Empty, Success = true, Details = result };
                    return Ok(JsonConvert.SerializeObject(objCommonResponse, Formatting.Indented));
                }
                else
                {
                    objCommonResponse.Response = new CommonResponse<T>() { Message = "Not Found", Success = false, Details = result };
                    return NotFound(objCommonResponse);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [NonAction]
        public IActionResult ProcessGetResponseBody<T>(List<T> result, string HasLimitResponse, CRUDAction CRUDAction)
        {
            try
            {
                ResponseObject<T> objCommonResponse = new ResponseObject<T>();

                if (result.Count > 0)
                {
                   
                    //objCommonResponse.Response = new CommonResponseBody<T>() { Success = true, Details = result};
                    return Ok(JsonConvert.SerializeObject( result, Formatting.Indented));
                }
                else
                {
                    objCommonResponse.Response = new CommonResponse<T>() { Message = "Not Found", Success = false, Details = result };
                    return NotFound(objCommonResponse);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [NonAction]
        public IActionResult ProcessGetResponseBody1<T>(List<T> result, string HasLimitResponse, CRUDAction CRUDAction)
        {
            try
            {
                ResponseObject<T> objCommonResponse = new ResponseObject<T>();

                if (result.Count > 0)
                {

                    return Ok("{" + JsonConvert.SerializeObject(GetObjectArray(result), Formatting.Indented).Replace("[", "").Replace("]", "") +"}");
                    //return Ok(JsonConvert.SerializeObject(result, Formatting.None));
                }
                else
                {
                    objCommonResponse.Response = new CommonResponse<T>() { Message = "Not Found", Success = false, Details = result };
                    return NotFound(objCommonResponse);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static IEnumerable<object> GetObjectArray<T>(IEnumerable<T> obj)
        {
            return obj.Select(o => o.GetType().GetProperties().Select(p => p.GetValue(o, null)));
        }

        [NonAction]
        public IActionResult ProcessGetRes<T>(List<T> result, string labelName, CRUDAction CRUDAction)
        {
            try
            {
                ResponseObject<T> objCommonResponse = new ResponseObject<T>();

                if (result.Count > 0)
                {
                    objCommonResponse.Response = new CommonResponse<T>() { Message = labelName, Success = true, Details = result };
                    return Ok(objCommonResponse);
                }
                else
                {
                    objCommonResponse.Response = new CommonResponse<T>() { Message = "Not Found", Success = false, Details = result };
                    return NotFound(objCommonResponse);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [NonAction]
        public IActionResult ProcessResponse<T>(saveStatus result, string labelName, CRUDAction operationCURD)
        {
            try
            {
                ResponseObject<T> objCommonResponseBO = new ResponseObject<T>();
                //if (operationCURD == CRUDAction.Insert)
                //{
                var Value = SaveStatusList.Where(a => a.ErrorID == (int)result).FirstOrDefault();
                if (Value != null)
                {
                   
                    /****************************Response Messages *********************************/
                    if ((int)saveStatus.success == Value.ErrorID && operationCURD == CRUDAction.CommonInsert)
                    {
                        objCommonResponseBO.Response = new CommonResponse<T>() { Message = labelName + " Created Successfully", Success = true };
                        return Ok(objCommonResponseBO);
                    }
                  
                    /****************************Response Messages - End*********************************/
                    else if ((int)saveStatus.success == Value.ErrorID && operationCURD == CRUDAction.Insert)
                    {
                        objCommonResponseBO.Response = new CommonResponse<T>() { Message = labelName + " Created Successfully", Success = true };
                        return Ok(objCommonResponseBO);
                    }
                    else if ((int)saveStatus.success == Value.ErrorID && operationCURD == CRUDAction.LoggedOut)
                    {
                        objCommonResponseBO.Response = new CommonResponse<T>() { Message = labelName + " Logged Out Successfully", Success = true };
                        return Ok(objCommonResponseBO);
                    }
                    else if ((int)saveStatus.success == Value.ErrorID && operationCURD == CRUDAction.Update)
                    {
                        objCommonResponseBO.Response = new CommonResponse<T>() { Message = labelName + " Updated Successfully", Success = true };
                        return Ok(objCommonResponseBO);
                    }
              
                    else if ((int)saveStatus.success == Value.ErrorID && operationCURD == CRUDAction.Rejected)
                    {
                        objCommonResponseBO.Response = new CommonResponse<T>() { Message = labelName + " Rejected Successfully", Success = true };
                        return Ok(objCommonResponseBO);
                    }
                    else if ((int)saveStatus.success == Value.ErrorID && operationCURD == CRUDAction.Delete)
                    {
                        objCommonResponseBO.Response = new CommonResponse<T>() { Message = labelName + " Deleted Successfully", Success = true };
                        return Ok(objCommonResponseBO);
                    }
                  
                    else
                    {
                        objCommonResponseBO.Response = new CommonResponse<T>() { Message = "Error Occured", Success = false };
                        return BadRequest(objCommonResponseBO);
                    }

                }
                else
                {
                    objCommonResponseBO.Response = new CommonResponse<T>() { Message = "Falied", Success = false };
                    return BadRequest(objCommonResponseBO);
                }
              
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
    public class ResponseObject<T>
    {
        public CommonResponse<T> Response { get; set; }

    }
    

    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class CommonResponse<T>
    {
        private bool success;
        private string message;
        private string haslimitvalue;
        private List<T> details;
       // private T modeldetails;
 
        private string button;
        //private string downloadFilePath;
        [JsonPropertyName("Success")]
        public bool Success
        {
            get { return this.success; }
            set { this.success = value; }
        }

        

        [JsonPropertyName("Message")]
        public string Message
        {
            get { return this.message ?? string.Empty; }
            set { this.message = value; }
        }
       

        public bool HasLimit
        {
            get; set;
        }
        // [JsonProperty("Details", NullValueHandling = NullValueHandling.Ignore)]
        [JsonProperty("Details", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("Details")]
        public List<T> Details
        {
            // using accessors
            get
            {
                return this.details;
            }
            set
            {
                this.details = value;
            }
        }
     

       
    }



    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class CommonResponseBody<T>
    {
        private bool success;
        private List<T> details;
        [JsonPropertyName("Success")]
        public bool Success
        {
            get { return this.success; }
            set { this.success = value; }
        }

        [JsonProperty("Details", NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("Details")]
        public List<T> Details
        {
            // using accessors
            get
            {
                return this.details;
            }
            set
            {
                this.details = value;
            }
        }

    }



    [Serializable, DataContract(Name = "MethodResultOf{0}")]
    public class MethodResult<T>
    {
        private string _briefDescription;
        private string _briefDetails;
        private string _errorDetails;
        private string _errorCode;
        private saveStatus _resultType;
        private T resultObject;
        private string uniqueIdStr;

        public MethodResult()
        {
        }

        public MethodResult(saveStatus resultType, string errorCode, string briefDescription)
        {
            this._resultType = resultType;
            this._errorCode = errorCode;
            this._briefDescription = briefDescription;
            this.resultObject = default(T);
        }

        public MethodResult(saveStatus resultType, string errorCode, string briefDescription, T ResultObject)
        {
            this._resultType = resultType;
            this._errorCode = errorCode;
            this._briefDescription = briefDescription;
            this.resultObject = ResultObject;
        }

        [DataMember]
        public string BriefDescription
        {
            get
            {
                return this._briefDescription;
            }
            set
            {
                this._briefDescription = value;
            }
        }
        [DataMember]
        public List<Int64> CustomerIdsList { get; set; }
        [DataMember]
        public string BriefDetails
        {
            get
            {
                return this._briefDetails;
            }
            set
            {
                this._briefDetails = value;
            }
        }
        [DataMember]
        public string ErrorDetails
        {
            get
            {
                return this._errorDetails;
            }
            set
            {
                this._errorDetails = value;
            }
        }
        [DataMember]
        public saveStatus ResultCode
        {
            get
            {
                return this._resultType;
            }
            set
            {
                this._resultType = value;
            }
        }

        [DataMember]
        public string ResultError
        {

            get
            {
                return this._errorCode;
            }
            set
            {
                this._errorCode = value;
            }
        }

        [DataMember]
        public string UniqueIdStr
        {
            get
            {
                return this.uniqueIdStr;
            }
            set
            {
                this.uniqueIdStr = value;
            }
        }


        [DataMember]
        public T ResultObject
        {
            get
            {
                return this.resultObject;
            }
            set
            {
                this.resultObject = value;
            }
        }
    }
}
