// CopyRight https://github.com/fnaxi. All Rights Reserved.

using System.Diagnostics;
using DSharpPlus;

namespace Core.Services;

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
	}
	
	public static async Task Connect()
	{
		foreach (CServiceBase Service in Services)
		{
			if (!await Service.Connect()) continue;
			
			LogInfo($"Connected service {Service.GetType()}");
		}
	}
	
	private static List<CServiceBase> Services = [];
}

public class CServiceBase
{
	public CServiceBase(DiscordClient InClient)
	{
		Client = InClient;
		
		LogInfo($"Initialized service {GetType()}");
	}
	
	protected readonly DiscordClient Client;

	public virtual Task<bool> Connect()
	{
		return Task.FromResult(false);
	}
}
