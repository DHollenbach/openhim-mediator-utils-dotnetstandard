using System;
using System.Collections.Generic;

namespace HealthMediator.Utils.Models.MediatorConfig.MediatorSetup
{
	[Serializable]
	public class DefaultChannelConfig
	{
		public string Name { get; set; }
		public string UrlPattern { get; set; }
		public string Type { get; set; }
		public IEnumerable<Route> Routes { get; set; }
		public IEnumerable<string> Allow { get; set; }
	}
}
