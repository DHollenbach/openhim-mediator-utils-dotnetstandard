using HealthMediator.Utils.Helpers;
using System;
using Xunit;

namespace HealthMediator.Utils.Tests
{
	public class SHATests
    {
		[Fact]
		public void EncryptUsingSHA512_GivenNullInput_ShouldThrowArgumentNullException()
		{
			// Assert
			Assert.Throws<ArgumentNullException>(() => SHA.EncryptUsingSHA512(null));
		}

		[Fact]
		public void EncryptUsingSHA512_GivenEmptyInput_ShouldReturnEmptySHA512EncryptedString()
		{
			// Arrange
			var input = string.Empty;

			// Act
			var result = SHA.EncryptUsingSHA512(input);

			// Assert
			Assert.Equal(string.Empty, result);
		}

		[Fact]
		public void EncryptUsingSHA512_GivenValidInput_ShouldReturnSHA512EncryptedString()
		{
			// Arrange
			var input = "password";
			// generated at https://passwordsgenerator.net/sha512-hash-generator/
			var sha512Value = "b109f3bbbc244eb82441917ed06d618b9008dd09b3befd1b5e07394c706a8bb980b1d7785e5976ec049b46df5f1326af5a2ea6d103fd07c95385ffab0cacbc86";

			// Act
			var result = SHA.EncryptUsingSHA512(input);

			// Assert
			Assert.Equal(sha512Value, result);
		}
	}
}
