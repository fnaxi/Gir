// CopyRight https://github.com/fnaxi. All Rights Reserved.

global using static Core.CUtils;
global using static Core.CLog;
using Core;
using Kuromi.Services.Client;

namespace Kuromi;

public class CKuromiBot : CBotBase
{
	protected override void SetupServices()
	{
		CServices.Setup(
		[
			new CMusicPlayingService(Client)
		]);
	}

	protected override void RegisterCommandModules()
	{
	}
}

internal abstract class CEntryPoint
{
	private static Task Main(string[] Args)
	{
		return new CKuromiBot().Start(EBotType.Music);
	}
}
