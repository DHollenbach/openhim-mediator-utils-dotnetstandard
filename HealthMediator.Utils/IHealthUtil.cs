using Microsoft.Extensions.Configuration;

namespace HealthMediator.Utils
{
	public interface IHealthUtil
    {
		HealthUtil Initialize(IConfiguration configuration);
		HealthUtil RegisterMediator();
    }
}
