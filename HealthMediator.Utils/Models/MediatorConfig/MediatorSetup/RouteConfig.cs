using System;

namespace HealthMediator.Utils.Models.MediatorConfig.MediatorSetup
{
	[Serializable]
	public abstract class RouteConfig
    {
		public string Name { get; set; }
		public string Host { get; set; }
		public string Port { get; set; }
		public bool Primary { get; set; }
		public string Type { get; set; }
    }
}
