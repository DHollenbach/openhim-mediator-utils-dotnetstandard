using System;
using System.Collections.Generic;

namespace HealthMediator.Utils.Models.MediatorConfig.MediatorSetup
{
	[Serializable]
	public class MediatorSetup
    {
		public string Urn { get; set; }
		public string Version { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public IEnumerable<DefaultChannelConfig> DefaultChannelConfig { get; set; }
		public IEnumerable<Endpoint> Endpoints { get; set; }
	}
}
