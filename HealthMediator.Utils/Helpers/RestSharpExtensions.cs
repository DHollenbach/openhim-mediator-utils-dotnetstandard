using RestSharp;
using System;

namespace HealthMediator.Utils.Helpers
{
	public static class RestSharpExtensions
    {
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
