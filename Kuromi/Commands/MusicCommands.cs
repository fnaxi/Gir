// CopyRight https://github.com/fnaxi. All Rights Reserved.

using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace Kuromi.Commands;

public class CMusicCommands : BaseCommandModule
{
	[Command("play")]
	public Task Play(CommandContext Context)
	{
		LogInfo("Playing some sick music outta here");
		
		return Task.CompletedTask;
	}
	
	// TODO: queue, music streaming
}
