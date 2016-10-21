using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcShop.Core
{
   public static class StringExtension
    {
       public static string ClearForJson(this string text)
       {
           if (string.IsNullOrWhiteSpace(text))
           {
               return string.Empty;
           }

           //return text.Replace((char)13, (char)0).Replace((char)10, (char)0);
           return text.Replace("\r", " ").Replace("\n", " ").Replace("\"", "");
       }

    }
}
