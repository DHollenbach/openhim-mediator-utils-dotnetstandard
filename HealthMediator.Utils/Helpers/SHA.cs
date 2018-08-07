using System;
using System.Security.Cryptography;
using System.Text;

namespace HealthMediator.Utils.Helpers
{
	public static class SHA
	{
		public static string EncryptUsingSHA512(string text)
		{
			string hex = "";
			SHA512 alg = SHA512Managed.Create();
			byte[] result = alg.ComputeHash(Encoding.Default.GetBytes(text));
			string hash = Encoding.UTF8.GetString(result);
			foreach (byte x in result)
			{
				hex += String.Format("{0:x2}", x);
			}
			return hex;
		}
	}
}
