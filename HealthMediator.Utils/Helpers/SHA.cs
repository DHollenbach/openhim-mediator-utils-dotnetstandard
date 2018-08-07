using System;
using System.Security.Cryptography;
using System.Text;

namespace HealthMediator.Utils.Helpers
{
	public static class SHA
	{
		public static string EncryptUsingSHA512(string text)
		{
			if(text is null)
			{
				throw new ArgumentNullException("text");
			}

			if(string.IsNullOrWhiteSpace(text))
			{
				return text;
			}

			StringBuilder hex = new StringBuilder();
			SHA512 alg = SHA512Managed.Create();
			byte[] result = alg.ComputeHash(Encoding.Default.GetBytes(text));
			string hash = Encoding.UTF8.GetString(result);
			foreach (byte x in result)
			{
				hex.Append($"{x:x2}");
			}
			return hex.ToString();
		}
	}
}
