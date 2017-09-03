using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Facebook;
using Newtonsoft.Json.Linq;

namespace _200_OK
{
   public class Operation
    {
      public  readonly string Token;
      public static FacebookClient fbC = null;
       public Operation(string token)
      {
          Token = token;
          fbC = new FacebookClient(Token);
      }

       /// <summary>
       /// Get Json Object
       /// </summary>
       /// <param name="command">The command to query Facebook API</param>
       /// <returns>Json Object in form of Dynamic Data Type</returns>
       public dynamic Get(string command)
      {
          try
          {
              dynamic data = fbC.Get(command);//Get Json Object
              return data;
          }
          catch { return null; }
      }
    }
}
