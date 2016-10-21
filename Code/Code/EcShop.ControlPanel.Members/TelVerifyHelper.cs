using EcShop.Entities.Members;
using EcShop.SqlDal.Members;
using Ecdev.Plugins;
using System;
using System.Collections.Generic;
namespace EcShop.ControlPanel.Members
{
    public class TelVerifyHelper
    {
        public static int CreateVerify(Verify verify)
        {
            return new TelVerifyDao().CreateVerify(verify);
        }

        public static bool CheckVerify(string telphone,string verifyCode)
        {
             bool result=true;
             Verify verifyInfo = new TelVerifyDao().GetVerify(telphone);

             if(verifyInfo==null)
             {
                 result = false;
                 return result;
             }
             if(verifyInfo.VerifyCode.ToLower() !=verifyCode)
             {
                 result = false;
                 return result;
             }

             return result;
        }
    }
}
