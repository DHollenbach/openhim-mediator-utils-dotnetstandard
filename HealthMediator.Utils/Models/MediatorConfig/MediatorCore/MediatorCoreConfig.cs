namespace HealthMediator.Utils.Models.MediatorConfig.MediatorCore
{
	public class MediatorCoreConfig
    {
		public string OpenHimCoreHost { get; set; }
		public string OpenHIMCoreAuthPath { get; set; }
		public string OpenHIMRegisterMediatorPath { get; set; }
		public string OpenHIMHeartbeatPath { get; set; }
		public int HeartbeatInterval { get; set; }
		public bool IsHeartbeatDisabled { get; set; }
	}
}
