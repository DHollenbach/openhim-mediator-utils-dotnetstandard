using HealthMediator.Utils.Helpers;
using HealthMediator.Utils.Models.MediatorConfig.Auth;
using HealthMediator.Utils.Models.MediatorConfig.MediatorCore;
using HealthMediator.Utils.Models.MediatorConfig.MediatorSetup;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Timers;

namespace HealthMediator.Utils
{
	public class HealthUtil : IHealthUtil
	{
		private OpenHIMAuth authSettings;
		private MediatorCoreConfig coreMediatorSettings;
		private MediatorSetup mediatorConfig;
		private RestClient restClient;
		private List<Parameter> authHeaders;
		private Timer heartbeatTimer = null;

		public HealthUtil Initialize(IConfiguration configuration)
		{
			var authSettings = new OpenHIMAuth();
			configuration.GetSection("mediatorConfig:openHimAuth").Bind(authSettings);

			var coreMediatorSettings = new MediatorCoreConfig();
			configuration.GetSection("mediatorConfig:mediatorCore").Bind(coreMediatorSettings);

			var mediatorConfig = new MediatorSetup();
			configuration.GetSection("mediatorConfig:mediatorSetup").Bind(mediatorConfig);

			this.authSettings = authSettings;
			this.coreMediatorSettings = coreMediatorSettings;
			this.mediatorConfig = mediatorConfig;

			restClient = new RestClient(coreMediatorSettings.OpenHimCoreHost);
			restClient.RemoteCertificateValidationCallback +=
				(sender, certificate, chain, sslPolicyErrors) => authSettings.TrustSelfSigned;

			return this;
		}

		public HealthUtil RegisterMediator()
		{
			// Authenticate with the HIM and get Authorization Token
			authHeaders = GetAuthenticationRequestParameters();

			var request = new RestRequest(coreMediatorSettings.OpenHIMRegisterMediatorPath, Method.POST);
			request.RequestFormat = DataFormat.Json;

			foreach(var header in authHeaders)
			{
				request.AddHeader(header.Name, header.Value.ToString());
			}

			var serlializedResult = JsonConvert.SerializeObject(
				mediatorConfig,
				new JsonSerializerSettings
				{
					ContractResolver = new CamelCasePropertyNamesContractResolver()
				});

			request.AddParameter("application/json", serlializedResult, ParameterType.RequestBody);
			var result = restClient.Execute(request);

			if (result.IsSuccessful && !coreMediatorSettings.IsHeartbeatDisabled)
			{
				InitializeHeartbeat();
			} else
			{
				throw new Exception(
					MethodBase.GetCurrentMethod()?.Name,
					result.GetRestResponseException()
				);
			}

			return this;
		}

		#region Private Helpers
		private AuthenticationResult AuthenticateWithHIM()
		{
			var request = new RestRequest(
				$"{coreMediatorSettings.OpenHIMCoreAuthPath}/{authSettings.Username}", 
				Method.GET
			);
			request.RequestFormat = DataFormat.Json;
			var result = restClient.Execute<AuthenticationResult>(request);

			if(!result.IsSuccessful)
			{
				throw new Exception(
					"AuthenticateWithHIM",
					result.GetRestResponseException()
				);
			}

			return result.Data;
		}

		private List<Parameter> GetAuthenticationRequestParameters()
		{
			var authResult = AuthenticateWithHIM();

			var parameters = new List<Parameter>();
			var date = DateTime.UtcNow;

			if (string.IsNullOrWhiteSpace(authResult.Salt))
			{
				throw new Exception($"{ authSettings.Username } has not been authenticated.");
			}

			// create passhash
			var passwordHash = 
				SHA.EncryptUsingSHA512($"{authResult.Salt}{authSettings.Password}");

			// create token
			var token =
				SHA.EncryptUsingSHA512($"{passwordHash}{authResult.Salt}{date}");

			// build return params
			parameters.Add(new Parameter
			{
				Name = "auth-username",
				Value = authSettings.Username
			});
			parameters.Add(new Parameter
			{
				Name = "auth-ts",
				Value = date
			});
			parameters.Add(new Parameter
			{
				Name = "auth-salt",
				Value = authResult.Salt
			});
			parameters.Add(new Parameter
			{
				Name = "auth-token",
				Value = token
			});

			return parameters;
		}

		private void InitializeHeartbeat()
		{
			// setup heartbeat
			heartbeatTimer = new Timer(1000 * coreMediatorSettings.HeartbeatInterval);
			heartbeatTimer.Enabled = true;
			heartbeatTimer.Elapsed += new ElapsedEventHandler(TimerFiredHeartbeat);
			heartbeatTimer.AutoReset = true;
			heartbeatTimer.Start();
		}

		private void TimerFiredHeartbeat(object Sender, ElapsedEventArgs e)
		{
			// Authenticate with the HIM and get Authorization Token
			authHeaders = GetAuthenticationRequestParameters();

			var request = new RestRequest($"{coreMediatorSettings.OpenHIMRegisterMediatorPath}/{mediatorConfig.Urn}/{coreMediatorSettings.OpenHIMHeartbeatPath}", Method.POST);
			request.RequestFormat = DataFormat.Json;

			foreach (var header in authHeaders)
			{
				request.AddHeader(header.Name, header.Value.ToString());
			}

			request.AddJsonBody(new { uptime = DateTime.UtcNow.Ticks });

			var result = restClient.Execute(request);
			if (!result.IsSuccessful)
			{
				throw new Exception(
					MethodBase.GetCurrentMethod()?.Name,
					result.GetRestResponseException()
				);
			}
		}
		#endregion
	}
}
