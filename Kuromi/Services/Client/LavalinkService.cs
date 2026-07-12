// CopyRight https://github.com/fnaxi. All Rights Reserved.

using Core.Services;
using DSharpPlus;
using DSharpPlus.Lavalink;
using DSharpPlus.Net;

namespace Kuromi.Services.Client;

public class CLavalinkService : CServiceBase
{
	public CLavalinkService(DiscordClient InClient) : base(InClient)
	{
		Lavalink = Client.UseLavalink();
	}

	private LavalinkExtension Lavalink;
	
	public override async Task<bool> Connect()
	{
		// TODO: revisit this
		return false;
		
		ConnectionEndpoint ConnectionInfo = new ConnectionEndpoint()
		{
			Hostname = "127.0.0.1",
			Port = 2333,
			Secured = false
		};

		LavalinkConfiguration Configuration = new LavalinkConfiguration()
		{
			Password = "KUROMI_MUSIC",
			RestEndpoint = ConnectionInfo,
			SocketEndpoint = ConnectionInfo
		};
		
		await Lavalink.ConnectAsync(Configuration);

		return true;
	}
}
