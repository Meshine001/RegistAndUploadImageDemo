using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RegistAndUploadImageDemo.Models
{
    public class TenResult
	{
        public static int STATUS_OK = 0;
        public static int STATUS_ERROR = 1;

        public Dictionary<string, object> CreateSuccess(string tenuserId)
        {
            return Message(STATUS_OK, tenuserId, "success", null);
        }

        public Dictionary<string, object> DetailsSuccess(TenUser tenuser)
        {
            
            List<FilePath> files = new List<FilePath>();
            foreach (var f in tenuser.FilePaths)
            {
                f.TenUser = null;
                files.Add(f);
            }
            tenuser.FilePaths = null;

            Dictionary<string, object> result = new Dictionary<string, object>();
            result.Add("tenuser", tenuser);
            result.Add("files", files);

            return Message(STATUS_OK, null, "success", result);
        }

        public Dictionary<string, object> Error(string message)
        {
            return Message(STATUS_ERROR, null, message, null);
        }

        public Dictionary<string, object> Error()
        {
            return Message(STATUS_ERROR, null, "error", null);
        }

         public Dictionary<string, object> Ok(string message)
        {
            return Message(STATUS_OK, null, message, null);
        }

         public Dictionary<string, object> Ok()
         {
             return Message(STATUS_OK, null, "success", null);
         }



        private Dictionary<string, object> Message(int status, string tenUserId, string message,object result)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("status", status);

            if (tenUserId != null) dictionary.Add("tenUserId", tenUserId);
            
            dictionary.Add("message", message);
            
            if (result != null) dictionary.Add("result", result);        
            
            return dictionary;
        }
	}
   
}