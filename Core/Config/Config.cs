// CopyRight https://github.com/fnaxi. All Rights Reserved.

using Microsoft.Extensions.Configuration;

namespace Core.Config;

public class CConfig
{
	public string MainToken { get; init; } = "";
    public string MusicToken { get; init; } = "";
    
    public string MainPrefix { get; init; } = "";
    public string MusicPrefix { get; init; } = "";
	
	private const string Name = "Config.ini";
	
	public static CConfig Parse(string InPath) // TODO: Move IDs to Config.json
	{
		IConfiguration Configuration = new ConfigurationBuilder()
			.SetBasePath(InPath)
			.AddIniFile(Name, optional: false, reloadOnChange: false)
			.Build();

		CConfig Config = new CConfig
		{
			MainToken = Configuration["MainBot:Token"] ?? "",
			MainPrefix = Configuration["MainBot:Prefix"] ?? "",
			
			MusicToken = Configuration["MusicBot:Token"] ?? "",
			MusicPrefix = Configuration["MusicBot:Prefix"] ?? ""
		};

		LogInfo("Parsed config");
		LogDebug($"Tokens: '{Config.MainToken}' / '{Config.MusicToken}'");
		LogInfo($"Prefixes: '{Config.MainPrefix}' / '{Config.MusicPrefix}'");

		return Config;
	}

	public string GetToken(EBotType BotType)
	{
		switch (BotType)
		{
			case EBotType.Main: return MainToken;
			case EBotType.Music: return MusicToken;
			
			default: return MainToken;
		}
	}
	
	public string GetPrefix(EBotType BotType)
	{
		switch (BotType)
		{
			case EBotType.Main: return MainPrefix;
			case EBotType.Music: return MusicPrefix;
			
			default: return MainPrefix;
		}
	}
}
