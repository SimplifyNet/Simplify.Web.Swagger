using Simplify.DI;
using Simplify.Web.Bootstrapper;
using Simplify.Web.Json;

namespace TesterApp.Setup
{
	public static class IocRegistrations
	{
		public static IDIContainerProvider RegisterAll(this IDIContainerProvider containerProvider)
		{
			containerProvider.RegisterSimplifyWeb()
				.RegisterJsonModelBinder();

			return containerProvider;
		}
	}
}