using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
namespace EcShop.UI.Web.pay
{
	public sealed class RSAFromPkcs8
	{
		public static string sign(string content, string privateKey, string input_charset)
		{
			System.Text.Encoding encoding = System.Text.Encoding.GetEncoding(input_charset);
			byte[] bytes = encoding.GetBytes(content);
			System.Security.Cryptography.RSACryptoServiceProvider rSACryptoServiceProvider = RSAFromPkcs8.DecodePemPrivateKey(privateKey);
			System.Security.Cryptography.SHA1 halg = new System.Security.Cryptography.SHA1CryptoServiceProvider();
			byte[] inArray = rSACryptoServiceProvider.SignData(bytes, halg);
			return System.Convert.ToBase64String(inArray);
		}
		public static bool verify(string content, string signedString, string publicKey, string input_charset)
		{
			System.Text.Encoding encoding = System.Text.Encoding.GetEncoding(input_charset);
			byte[] bytes = encoding.GetBytes(content);
			byte[] signature = System.Convert.FromBase64String(signedString);
			System.Security.Cryptography.RSAParameters parameters = RSAFromPkcs8.ConvertFromPublicKey(publicKey);
			System.Security.Cryptography.RSACryptoServiceProvider rSACryptoServiceProvider = new System.Security.Cryptography.RSACryptoServiceProvider();
			rSACryptoServiceProvider.ImportParameters(parameters);
			System.Security.Cryptography.SHA1 halg = new System.Security.Cryptography.SHA1CryptoServiceProvider();
			return rSACryptoServiceProvider.VerifyData(bytes, halg, signature);
		}
		public static string decryptData(string resData, string privateKey, string input_charset)
		{
			byte[] array = System.Convert.FromBase64String(resData);
			System.Collections.Generic.List<byte> list = new System.Collections.Generic.List<byte>();
			for (int i = 0; i < array.Length / 128; i++)
			{
				byte[] array2 = new byte[128];
				for (int j = 0; j < 128; j++)
				{
					array2[j] = array[j + 128 * i];
				}
				list.AddRange(RSAFromPkcs8.decrypt(array2, privateKey, input_charset));
			}
			byte[] array3 = list.ToArray();
			char[] array4 = new char[System.Text.Encoding.GetEncoding(input_charset).GetCharCount(array3, 0, array3.Length)];
			System.Text.Encoding.GetEncoding(input_charset).GetChars(array3, 0, array3.Length, array4, 0);
			return new string(array4);
		}
		private static byte[] decrypt(byte[] data, string privateKey, string input_charset)
		{
			System.Security.Cryptography.RSACryptoServiceProvider rSACryptoServiceProvider = RSAFromPkcs8.DecodePemPrivateKey(privateKey);
			new System.Security.Cryptography.SHA1CryptoServiceProvider();
			return rSACryptoServiceProvider.Decrypt(data, false);
		}
		private static System.Security.Cryptography.RSACryptoServiceProvider DecodePemPrivateKey(string pemstr)
		{
			byte[] array = System.Convert.FromBase64String(pemstr);
			if (array != null)
			{
				return RSAFromPkcs8.DecodePrivateKeyInfo(array);
			}
			return null;
		}
		private static System.Security.Cryptography.RSACryptoServiceProvider DecodePrivateKeyInfo(byte[] pkcs8)
		{
			byte[] b = new byte[]
			{
				48,
				13,
				6,
				9,
				42,
				134,
				72,
				134,
				247,
				13,
				1,
				1,
				1,
				5,
				0
			};
			byte[] a = new byte[15];
			System.IO.MemoryStream memoryStream = new System.IO.MemoryStream(pkcs8);
			int num = (int)memoryStream.Length;
			System.IO.BinaryReader binaryReader = new System.IO.BinaryReader(memoryStream);
			System.Security.Cryptography.RSACryptoServiceProvider result;
			try
			{
				ushort num2 = binaryReader.ReadUInt16();
				if (num2 == 33072)
				{
					binaryReader.ReadByte();
				}
				else
				{
					if (num2 != 33328)
					{
						result = null;
						return result;
					}
					binaryReader.ReadInt16();
				}
				byte b2 = binaryReader.ReadByte();
				if (b2 != 2)
				{
					result = null;
				}
				else
				{
					num2 = binaryReader.ReadUInt16();
					if (num2 != 1)
					{
						result = null;
					}
					else
					{
						a = binaryReader.ReadBytes(15);
						if (!RSAFromPkcs8.CompareBytearrays(a, b))
						{
							result = null;
						}
						else
						{
							b2 = binaryReader.ReadByte();
							if (b2 != 4)
							{
								result = null;
							}
							else
							{
								b2 = binaryReader.ReadByte();
								if (b2 == 129)
								{
									binaryReader.ReadByte();
								}
								else
								{
									if (b2 == 130)
									{
										binaryReader.ReadUInt16();
									}
								}
								byte[] privkey = binaryReader.ReadBytes((int)((long)num - memoryStream.Position));
								System.Security.Cryptography.RSACryptoServiceProvider rSACryptoServiceProvider = RSAFromPkcs8.DecodeRSAPrivateKey(privkey);
								result = rSACryptoServiceProvider;
							}
						}
					}
				}
			}
			catch (System.Exception)
			{
				result = null;
			}
			finally
			{
				binaryReader.Close();
			}
			return result;
		}
		private static bool CompareBytearrays(byte[] a, byte[] b)
		{
			if (a.Length != b.Length)
			{
				return false;
			}
			int num = 0;
			for (int i = 0; i < a.Length; i++)
			{
				byte b2 = a[i];
				if (b2 != b[num])
				{
					return false;
				}
				num++;
			}
			return true;
		}
		private static System.Security.Cryptography.RSACryptoServiceProvider DecodeRSAPrivateKey(byte[] privkey)
		{
			System.IO.MemoryStream input = new System.IO.MemoryStream(privkey);
			System.IO.BinaryReader binaryReader = new System.IO.BinaryReader(input);
			System.Security.Cryptography.RSACryptoServiceProvider result;
			try
			{
				ushort num = binaryReader.ReadUInt16();
				if (num == 33072)
				{
					binaryReader.ReadByte();
				}
				else
				{
					if (num != 33328)
					{
						result = null;
						return result;
					}
					binaryReader.ReadInt16();
				}
				num = binaryReader.ReadUInt16();
				if (num != 258)
				{
					result = null;
				}
				else
				{
					byte b = binaryReader.ReadByte();
					if (b != 0)
					{
						result = null;
					}
					else
					{
						int integerSize = RSAFromPkcs8.GetIntegerSize(binaryReader);
						byte[] modulus = binaryReader.ReadBytes(integerSize);
						integerSize = RSAFromPkcs8.GetIntegerSize(binaryReader);
						byte[] exponent = binaryReader.ReadBytes(integerSize);
						integerSize = RSAFromPkcs8.GetIntegerSize(binaryReader);
						byte[] d = binaryReader.ReadBytes(integerSize);
						integerSize = RSAFromPkcs8.GetIntegerSize(binaryReader);
						byte[] p = binaryReader.ReadBytes(integerSize);
						integerSize = RSAFromPkcs8.GetIntegerSize(binaryReader);
						byte[] q = binaryReader.ReadBytes(integerSize);
						integerSize = RSAFromPkcs8.GetIntegerSize(binaryReader);
						byte[] dP = binaryReader.ReadBytes(integerSize);
						integerSize = RSAFromPkcs8.GetIntegerSize(binaryReader);
						byte[] dQ = binaryReader.ReadBytes(integerSize);
						integerSize = RSAFromPkcs8.GetIntegerSize(binaryReader);
						byte[] inverseQ = binaryReader.ReadBytes(integerSize);
						System.Security.Cryptography.RSACryptoServiceProvider rSACryptoServiceProvider = new System.Security.Cryptography.RSACryptoServiceProvider();
						rSACryptoServiceProvider.ImportParameters(new System.Security.Cryptography.RSAParameters
						{
							Modulus = modulus,
							Exponent = exponent,
							D = d,
							P = p,
							Q = q,
							DP = dP,
							DQ = dQ,
							InverseQ = inverseQ
						});
						result = rSACryptoServiceProvider;
					}
				}
			}
			catch (System.Exception)
			{
				result = null;
			}
			finally
			{
				binaryReader.Close();
			}
			return result;
		}
		private static int GetIntegerSize(System.IO.BinaryReader binr)
		{
			byte b = binr.ReadByte();
			if (b != 2)
			{
				return 0;
			}
			b = binr.ReadByte();
			int num;
			if (b == 129)
			{
				num = (int)binr.ReadByte();
			}
			else
			{
				if (b == 130)
				{
					byte b2 = binr.ReadByte();
					byte b3 = binr.ReadByte();
					byte[] array = new byte[4];
					array[0] = b3;
					array[1] = b2;
					byte[] value = array;
					num = System.BitConverter.ToInt32(value, 0);
				}
				else
				{
					num = (int)b;
				}
			}
			while (binr.ReadByte() == 0)
			{
				num--;
			}
			binr.BaseStream.Seek(-1L, System.IO.SeekOrigin.Current);
			return num;
		}
		private static System.Security.Cryptography.RSAParameters ConvertFromPublicKey(string pemFileConent)
		{
			byte[] array = System.Convert.FromBase64String(pemFileConent);
			if (array.Length < 162)
			{
				throw new System.ArgumentException("pem file content is incorrect.");
			}
			byte[] array2 = new byte[128];
			byte[] array3 = new byte[3];
			System.Array.Copy(array, 29, array2, 0, 128);
			System.Array.Copy(array, 159, array3, 0, 3);
			return new System.Security.Cryptography.RSAParameters
			{
				Modulus = array2,
				Exponent = array3
			};
		}
		private static System.Security.Cryptography.RSAParameters ConvertFromPrivateKey(string pemFileConent)
		{
			byte[] array = System.Convert.FromBase64String(pemFileConent);
			if (array.Length < 609)
			{
				throw new System.ArgumentException("pem file content is incorrect.");
			}
			int num = 11;
			byte[] array2 = new byte[128];
			System.Array.Copy(array, num, array2, 0, 128);
			num += 128;
			num += 2;
			byte[] array3 = new byte[3];
			System.Array.Copy(array, num, array3, 0, 3);
			num += 3;
			num += 4;
			byte[] array4 = new byte[128];
			System.Array.Copy(array, num, array4, 0, 128);
			num += 128;
			num += ((array[num + 1] == 64) ? 2 : 3);
			byte[] array5 = new byte[64];
			System.Array.Copy(array, num, array5, 0, 64);
			num += 64;
			num += ((array[num + 1] == 64) ? 2 : 3);
			byte[] array6 = new byte[64];
			System.Array.Copy(array, num, array6, 0, 64);
			num += 64;
			num += ((array[num + 1] == 64) ? 2 : 3);
			byte[] array7 = new byte[64];
			System.Array.Copy(array, num, array7, 0, 64);
			num += 64;
			num += ((array[num + 1] == 64) ? 2 : 3);
			byte[] array8 = new byte[64];
			System.Array.Copy(array, num, array8, 0, 64);
			num += 64;
			num += ((array[num + 1] == 64) ? 2 : 3);
			byte[] array9 = new byte[64];
			System.Array.Copy(array, num, array9, 0, 64);
			return new System.Security.Cryptography.RSAParameters
			{
				Modulus = array2,
				Exponent = array3,
				D = array4,
				P = array5,
				Q = array6,
				DP = array7,
				DQ = array8,
				InverseQ = array9
			};
		}
	}
}
