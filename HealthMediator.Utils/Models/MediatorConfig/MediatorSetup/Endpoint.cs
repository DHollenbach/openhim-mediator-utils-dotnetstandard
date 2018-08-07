using System;

namespace HealthMediator.Utils.Models.MediatorConfig.MediatorSetup
{
	[Serializable]
	public class Endpoint : RouteConfig
    {
		public string Path { get; set; }
    }
}
