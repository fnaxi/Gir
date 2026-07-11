// CopyRight https://github.com/fnaxi. All Rights Reserved.

using DSharpPlus;

namespace Core;

public static class CServices // TODO: CServiceManager?
{
	public static TServiceType? Get<TServiceType>() where TServiceType : CServiceBase
	{
		foreach (CServiceBase ServiceBase in Services)
		{
			if( ServiceBase is TServiceType Service ) 
				return Service;
		}
		
		LogError($"Can't find {typeof(TServiceType).Name} service!");
		return null;
	}

	public static void Setup(List<CServiceBase> InServices)
	{
		Services = InServices;
		foreach (CServiceBase Service in Services)
		{
			LogInfo($"Registered service {Service.GetType()}");
		}
	}
	
	private static List<CServiceBase> Services = [];
}

public class CServiceBase(DiscordClient InClient)
{
	protected readonly DiscordClient Client = InClient;
}
