using System;
using System.Diagnostics;
using System.IO;
namespace EcShop.UI.Web.App_Code
{
	public class RsaKeyHelper
	{
		public static string CreateRSAKeyFile(string generatorPath, string keyDirectory)
		{
			System.Diagnostics.Process process = new System.Diagnostics.Process();
			process.StartInfo = new System.Diagnostics.ProcessStartInfo
			{
				FileName = generatorPath,
				UseShellExecute = false,
				RedirectStandardInput = true,
				RedirectStandardOutput = true,
				Arguments = "\"" + keyDirectory + "\""
			};
			process.Start();
			process.WaitForExit();
			return RsaKeyHelper.GetRSAKeyContent(keyDirectory + "/rsa_public_key.pem", true);
		}
		public static string GetRSAKeyContent(string path, bool isPubKey)
		{
			string text = string.Empty;
			string arg = isPubKey ? "PUBLIC KEY" : "RSA PRIVATE KEY";
			using (System.IO.StreamReader streamReader = new System.IO.StreamReader(path))
			{
				text = streamReader.ReadToEnd();
				streamReader.Close();
			}
			string text2 = string.Format("-----BEGIN {0}-----\\n", arg);
			string value = string.Format("-----END {0}-----", arg);
			int num = text.IndexOf(text2) + text2.Length;
			int num2 = text.IndexOf(value, num);
			text = text.Substring(num, num2 - num);
			return text.Replace("\r", "").Replace("\n", "");
		}
	}
}
