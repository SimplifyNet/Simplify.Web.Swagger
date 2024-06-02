using Simplify.DI;
using Simplify.Web;

namespace TesterApp.Setup;

public static class IocRegistrations
{
	public static IDIContainerProvider RegisterAll(this IDIContainerProvider containerProvider)
	{
		containerProvider.RegisterSimplifyWeb();

		return containerProvider;
	}
}