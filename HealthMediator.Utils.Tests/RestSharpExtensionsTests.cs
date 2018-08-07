using HealthMediator.Utils.Helpers;
using Moq;
using RestSharp;
using System;
using Xunit;

namespace HealthMediator.Utils.Tests
{
	public class RestSharpExtensionsTests
    {
		[Fact]
		public void GetRestResponseException_GivenNullIRestResponse_ShouldThrowArgumentNullException()
		{
			IRestResponse restRespone = null;
			Assert.Throws<ArgumentNullException>(() => restRespone.GetFailedRestResponseException());
		}

		[Fact]
		public void GetRestResponseException_GivenIRestResponseIsSuccessful_ShouldReturnNullException()
		{
			// Arrange
			var mock = new Mock<IRestResponse>();
			mock.Setup(x => x.IsSuccessful).Returns(true);
			IRestResponse restRespone = mock.Object;

			// Act
			var result = restRespone.GetFailedRestResponseException();

			// Assert
			Assert.Null(result);
		}

		[Fact]
		public void GetRestResponseException_GivenIRestResponseIsNotSuccessful_ShouldReturnException()
		{
			// Arrange
			var defaultMessage = "not specified";
			var mock = new Mock<IRestResponse>();
			mock.Setup(x => x.IsSuccessful).Returns(false);

			IRestResponse restRespone = mock.Object;

			// Act
			var result = restRespone.GetFailedRestResponseException();

			// Assert
			Assert.NotNull(result);
		}
	}
}
