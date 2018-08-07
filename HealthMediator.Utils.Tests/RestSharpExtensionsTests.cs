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
		public void GetRestResponseException_GivenIRestResponseIsNotSuccessful_ShouldReturnExceptionWithDefaultMessages()
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
			Assert.Contains($"ResponseStatus: {defaultMessage}", result.Message);
			Assert.Contains($"ResponseUri: {defaultMessage}", result.Message);
			Assert.Contains($"ErrorException: {defaultMessage}", result.Message);
			Assert.Contains($"Stacktrace: {defaultMessage}", result.Message);
			Assert.Contains($"ExceptionType: {defaultMessage}", result.Message);
		}

		[Fact]
		public void GetRestResponseException_GivenIRestResponseIsNotSuccessful_ShouldReturnExceptionWithCustomMessages()
		{
			// Arrange
			var statusDescription = "statusDescription";
			var responseUri = new Uri("http://test.io");
			var errorMessage = "errorMessage";
			var exception = new Exception("testErrorMessage");
			var defaultMessage = "not specified";

			var mock = new Mock<IRestResponse>();
			mock.Setup(x => x.IsSuccessful).Returns(false);
			mock.Setup(x => x.StatusDescription).Returns(statusDescription);
			mock.Setup(x => x.ResponseUri).Returns(responseUri);
			mock.Setup(x => x.ErrorMessage).Returns(errorMessage);
			mock.Setup(x => x.ErrorException).Returns(exception);

			IRestResponse restRespone = mock.Object;

			// Act
			var result = restRespone.GetFailedRestResponseException();

			// Assert
			Assert.NotNull(result);
			Assert.Contains($"ResponseStatus: {statusDescription}", result.Message);
			Assert.Contains($"ResponseUri: {responseUri.ToString()}", result.Message);
			Assert.Contains($"ErrorException: {errorMessage}", result.Message);
			Assert.Contains($"Stacktrace: {defaultMessage}", result.Message);
			Assert.Contains($"ExceptionType: {exception.GetType().Name}", result.Message);
		}
	}
}
