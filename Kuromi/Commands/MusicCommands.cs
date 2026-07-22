// CopyRight https://github.com/fnaxi. All Rights Reserved.

using System.Diagnostics;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Lavalink;
using Kuromi.ID;

namespace Kuromi.Commands;

public class CMusicCommands : BaseCommandModule
{
	// TODO: queue
	
	// TODO: Embeds
	
	private async Task<LavalinkGuildConnection?> GetLavalinkEstablishedConnection(CommandContext Context)
	{
		LavalinkNodeConnection? Node = await GetLavalinkNodeConnection(Context);
		if (Node == null) return null;
		
		LavalinkGuildConnection? Connection = Node.GetGuildConnection(Context.Guild);
		if (Connection == null)
		{
			await BadLavalinkConnection(Context);
			return null;
		}

		if (Connection.CurrentState.CurrentTrack == null)
		{
			await NothingIsStreaming(Context);
			return null;
		}

		return Connection;
	}
	
	private async Task<LavalinkNodeConnection?> GetLavalinkNodeConnection(CommandContext Context)
	{
		Debug.Assert(Context.Member != null);

		DiscordChannel? Channel = Context.Member.VoiceState.Channel;
		if (!IsVoiceChannel(Channel))
		{
			await NotInVoiceChannel(Context);
			return null;
		}

		LavalinkExtension Lavalink = Context.Client.GetLavalink();
		if (!IsLavalinkValid(Lavalink))
		{
			await BadLavalinkConnection(Context);
			return null;
		}

		return Lavalink.ConnectedNodes.Values.First();
	}
	
	[Command("stream")] [Aliases("play")]
	public async Task Stream(CommandContext Context, 
		[Description("A song name or a link to one of these platforms: Spotify, Youtube, Soundcloud, etc.")] [RemainingText] string Query)
	{
		// TODO: check that the command is executed only in bot channel and chat of the VC
		// if (SentInBotChannel(Context.Channel))

		Debug.Assert(Context.Member != null);
		DiscordChannel? Channel = Context.Member.VoiceState.Channel;

		LavalinkNodeConnection? Node = await GetLavalinkNodeConnection(Context);
		if (Node == null) return;
		
		await Node.ConnectAsync(Channel);

		LavalinkGuildConnection Connection = Node.GetGuildConnection(Context.Guild);
		if (Connection == null)
		{
			await Context.RespondAsync($"Failed to connect to the Lavalink! {CEmoji.Chunibyo}");
			return;
		}
		
		LavalinkLoadResult LoadResult = await Node.Rest.GetTracksAsync(Query);
		if (LoadResult.LoadResultType is LavalinkLoadResultType.NoMatches or LavalinkLoadResultType.LoadFailed)
		{
			await Context.RespondAsync($"Can't find music with query: {Query}! {CEmoji.Chunibyo}");
			return;
		}
		
		LavalinkTrack Song = LoadResult.Tracks.First();
		await Connection.PlayAsync(Song);
		
		DiscordEmbedBuilder Embed = new DiscordEmbedBuilder()
		{
			Title = $"Joined {Channel.Name} channel",
			Description = 
				$"Now streaming: {Song.Title} {CEmoji.Chunibyo}. \n" +
				$"Author: {Song.Author}. \n" +
				$"URL: {Song.Uri}",
			Color = CutePink
		};
		await Context.RespondAsync(Embed);
	}
	
	[Command("stop")]
	public async Task Stop(CommandContext Context)
	{
		LavalinkGuildConnection? Connection = await GetLavalinkEstablishedConnection(Context);
		if (Connection == null) return;
		
		await Connection.StopAsync();

		DiscordEmbedBuilder Embed = new DiscordEmbedBuilder()
		{
			Title = $"Streaming was stopped",
			Description = $"",
			Color = CutePink
		};
		await Context.RespondAsync(Embed);
	}

	[Command("resume")]
	public async Task Resume(CommandContext Context)
	{
		LavalinkGuildConnection? Connection = await GetLavalinkEstablishedConnection(Context);
		if (Connection == null) return;

		await Connection.ResumeAsync();

		DiscordEmbedBuilder Embed = new DiscordEmbedBuilder()
		{
			Title = $"Streaming was resumed",
			Description = $"",
			Color = CutePink
		};
		await Context.RespondAsync(Embed);
	}
	
	[Command("pause")]
	public async Task Pause(CommandContext Context)
	{
		LavalinkGuildConnection? Connection = await GetLavalinkEstablishedConnection(Context);
		if (Connection == null) return;
		
		await Connection.PauseAsync();
		
		DiscordEmbedBuilder Embed = new DiscordEmbedBuilder()
		{
			Title = $"Streaming was paused",
			Description = $"",
			Color = CutePink
		};
		await Context.RespondAsync(Embed);
	}
	
	private async Task NotInVoiceChannel(CommandContext Context)
	{
		await Context.RespondAsync($"You should enter the voice channel first! {CEmoji.Chunibyo}");
	}

	private async Task BadLavalinkConnection(CommandContext Context)
	{
		await Context.RespondAsync($"Connection with the Lavalink is not established! {CEmoji.Chunibyo}");
	}
	private async Task NothingIsStreaming(CommandContext Context)
	{
		await Context.RespondAsync($"Nothing is streaming right now! {CEmoji.Chunibyo}");
	}
	
	private bool IsLavalinkValid(LavalinkExtension? Lavalink)
	{
		return Lavalink != null && Lavalink.ConnectedNodes.Any();
	}
}
