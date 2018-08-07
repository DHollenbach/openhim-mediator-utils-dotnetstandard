using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.IO;
using Xunit;

namespace HealthMediator.Utils.Tests
{
	public class HealthUtilTests
    {
		[Fact]
		public void Initialize_GivenIConfigurationWithNoOpenHimAuthSection_ShouldThrowArgumentNullException()
		{
			// Arrange
			var mock = new Mock<IConfiguration>();

			// Act
			var result = Assert.Throws<ArgumentNullException>(() => new HealthUtil().Initialize(mock.Object));

			// Assert
			Assert.Equal("Value cannot be null.\r\nParameter name: configuration", result.Message);
		}

		[Fact]
		public void Initialize_GivenIConfigurationSectionsMocked_ShouldThrowArgumentNullExceptionForOpenHimCoreHost()
		{
			// Arrange
			var configuration = new ConfigurationBuilder()
			   .SetBasePath(Directory.GetCurrentDirectory())
			   .AddJsonFile("config/emptyAppSettings.json", false, true)
			   .Build();

			// Act
			var result = Assert.Throws<ArgumentNullException>(() => new HealthUtil().Initialize(configuration));

			// Assert
			Assert.Equal("Value cannot be null.\r\nParameter name: OpenHimCoreHost", result.Message);
		}

		[Fact]
		public void Initialize_GivenIConfigurationSectionsMocked_ShouldReturnHealthUtilObjectSuccess()
		{
			// Arrange
			var configuration = new ConfigurationBuilder()
			   .SetBasePath(Directory.GetCurrentDirectory())
			   .AddJsonFile("config/validAppSettings.json", false, true)
			   .Build();

			// Act
			var result = new HealthUtil().Initialize(configuration);

			// Assert
			Assert.NotNull(result);
			Assert.True(result.IsInitialized);
		}

		[Fact]
		public void RegisterMediator_GivenIConfigurationSectionsMocked_ShouldThrowInvalidOperationExceptionInitializeRequired()
		{
			// Arrange
			var configuration = new ConfigurationBuilder()
			   .SetBasePath(Directory.GetCurrentDirectory())
			   .AddJsonFile("config/emptyAppSettings.json", false, true)
			   .Build();

			// Act
			var result = Assert.Throws<InvalidOperationException>(() => new HealthUtil().RegisterMediator());

			// Assert
			Assert.Equal("You must call Initialize before calling RegisterMediator.", result.Message);
		}

	}
}
