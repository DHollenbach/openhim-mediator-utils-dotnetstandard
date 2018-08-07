using RestSharp;
using System;

namespace HealthMediator.Utils.Helpers
{
	public static class RestSharpExtensions
    {
		public static Exception GetRestResponseException(this IRestResponse result)
		{
			if(result is null)
			{
				return null;
			}

			var defaultMessage = "not specified";
			return new Exception(
					$"ResponseStatus: {result.StatusDescription}.\n" +
					$"ResponseUri: {result.ResponseUri}.\n" +
					$"ErrorException: {result.ErrorMessage ?? defaultMessage}.\n" +
					$"Stacktrace: {result.ErrorException?.StackTrace ?? defaultMessage}.\n" +
					$"ExceptionType: {result.ErrorException?.GetType()?.Name ?? defaultMessage}."
				);
		}
    }
}
