// CopyRight https://github.com/fnaxi. All Rights Reserved.

using System.Diagnostics;
using InvaderZim.Misc;
using Newtonsoft.Json;

namespace InvaderZim.Config;

public class CConfig
{
	public string Token { get; set; } = string.Empty;
	public string Prefix { get; set; } = string.Empty;
}

public class CConfigParser
{
	private const string Name = "Config.json";

	public CConfig Parse()
	{
		StreamReader Stream = new StreamReader(Name);
		string Json = Stream.ReadToEnd();

		CConfig? Config = JsonConvert.DeserializeObject<CConfig>(Json);
		Debug.Assert(Config != null, nameof(Config) + " != null");
		
		CLog.Info("Parsed config");
		CLog.Info($"Token: {Config.Token}");
		CLog.Info($"Prefix: {Config.Prefix}");
		
		return Config;
	}
}
