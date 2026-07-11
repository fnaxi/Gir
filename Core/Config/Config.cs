// CopyRight https://github.com/fnaxi. All Rights Reserved.

using System.Diagnostics;
using Newtonsoft.Json;

namespace Core.Config;

public class CConfig(string InMainToken, string InMusicToken, string InMainPrefix, string InMusicPrefix)
{
	public string MainToken = InMainToken;
	public string MusicToken = InMusicToken;

	public string MainPrefix = InMainPrefix;
	public string MusicPrefix = InMusicPrefix;
	
	private const string Name = "Config.json";
	
	public static CConfig Parse(string InPath) // TODO: Move IDs to Config.json
	{
		StreamReader Stream = new StreamReader(Path.Combine(InPath, Name));
		
		string Json = Stream.ReadToEnd();
		Stream.Close();
		
		CConfig? Config = JsonConvert.DeserializeObject<CConfig>(Json);
		Debug.Assert(Config != null);
		
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
