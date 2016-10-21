using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;
namespace EcShop.UI.Web.pay
{
	public class Core
	{
		public static System.Collections.Generic.Dictionary<string, string> FilterPara(System.Collections.Generic.SortedDictionary<string, string> dicArrayPre)
		{
			System.Collections.Generic.Dictionary<string, string> dictionary = new System.Collections.Generic.Dictionary<string, string>();
			foreach (System.Collections.Generic.KeyValuePair<string, string> current in dicArrayPre)
			{
				if (current.Key.ToLower() != "sign" && current.Key.ToLower() != "sign_type" && current.Value != "" && current.Value != null)
				{
					dictionary.Add(current.Key, current.Value);
				}
			}
			return dictionary;
		}
		public static string CreateLinkString(System.Collections.Generic.Dictionary<string, string> dicArray)
		{
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			foreach (System.Collections.Generic.KeyValuePair<string, string> current in dicArray)
			{
				stringBuilder.Append(current.Key + "=" + current.Value + "&");
			}
			int length = stringBuilder.Length;
			stringBuilder.Remove(length - 1, 1);
			return stringBuilder.ToString();
		}
		public static string CreateLinkStringUrlencode(System.Collections.Generic.Dictionary<string, string> dicArray, System.Text.Encoding code)
		{
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			foreach (System.Collections.Generic.KeyValuePair<string, string> current in dicArray)
			{
				stringBuilder.Append(current.Key + "=" + System.Web.HttpUtility.UrlEncode(current.Value, code) + "&");
			}
			int length = stringBuilder.Length;
			stringBuilder.Remove(length - 1, 1);
			return stringBuilder.ToString();
		}
		public static void LogResult(string sWord)
		{
			string text = System.Web.HttpContext.Current.Server.MapPath("log");
			text = text + "\\" + System.DateTime.Now.ToString().Replace(":", "") + ".txt";
			System.IO.StreamWriter streamWriter = new System.IO.StreamWriter(text, false, System.Text.Encoding.Default);
			streamWriter.Write(sWord);
			streamWriter.Close();
		}
		public static string GetAbstractToMD5(System.IO.Stream sFile)
		{
			System.Security.Cryptography.MD5 mD = new System.Security.Cryptography.MD5CryptoServiceProvider();
			byte[] array = mD.ComputeHash(sFile);
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder(32);
			for (int i = 0; i < array.Length; i++)
			{
				stringBuilder.Append(array[i].ToString("x").PadLeft(2, '0'));
			}
			return stringBuilder.ToString();
		}
		public static string GetAbstractToMD5(byte[] dataFile)
		{
			System.Security.Cryptography.MD5 mD = new System.Security.Cryptography.MD5CryptoServiceProvider();
			byte[] array = mD.ComputeHash(dataFile);
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder(32);
			for (int i = 0; i < array.Length; i++)
			{
				stringBuilder.Append(array[i].ToString("x").PadLeft(2, '0'));
			}
			return stringBuilder.ToString();
		}
	}
}
