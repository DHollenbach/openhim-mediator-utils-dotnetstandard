using RestSharp;
using System;

namespace HealthMediator.Utils.Helpers
{
	public static class RestSharpExtensions
    {
		/// <summary>
		/// A reusable method that can be called on a IRestResponse instance to
		/// build an meaningful exception
		/// </summary>
		/// <param name="result">IRestResponse</param>
		/// <returns>ArgumentNullException when IRestResponse is null</returns>
		/// <returns>null when IRestResponse.IsSuccessful is true</returns>
		/// <returns>Exception object when IRestResponse.IsSuccessful is false</returns>
		public static Exception GetFailedRestResponseException(this IRestResponse result)
		{
			if(result is null)
			{
				throw new ArgumentNullException(nameof(result));
			}

			if (result.IsSuccessful)
			{
				return null;
			}
			else
			{
				var defaultMessage = "not specified";
				return new Exception(
						$"ResponseStatus: {result.StatusDescription ?? defaultMessage}.\n" +
						$"ResponseUri: {result.ResponseUri?.ToString() ?? defaultMessage}.\n" +
						$"ErrorException: {result.ErrorMessage ?? defaultMessage}.\n" +
						$"Stacktrace: {result.ErrorException?.StackTrace ?? defaultMessage}.\n" +
						$"ExceptionType: {result.ErrorException?.GetType()?.Name ?? defaultMessage}."
					);
			}
		}
    }
}
