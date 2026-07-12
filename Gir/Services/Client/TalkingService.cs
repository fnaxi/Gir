// CopyRight https://github.com/fnaxi. All Rights Reserved.

using System.Text.RegularExpressions;
using Core.ID;
using Core.Services;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using Gir.Misc;

namespace Gir.Services.Client;

public class CTalkingService : CServiceBase
{
	public CTalkingService(DiscordClient Client) : base(Client)
	{
		Client.Ready += Client_OnReady;
		Client.MessageCreated += Client_OnMessageCreated;
	}

	private const bool bEnableRandomMessages = true;
	private const UInt16 RandomMessageInterval = 3600;

	private async Task Client_OnReady(DiscordClient Sender, ReadyEventArgs Args)
	{
		if (bEnableRandomMessages)
		{
			await StartRandomMessageLoop(Sender);
		}
	}

	private async Task Client_OnMessageCreated(DiscordClient Sender, MessageCreateEventArgs Args)
	{
		if (Args.Author.IsBot) return;

		// TODO: Bot should react to mentioning "gir" in a message
		
		if (Args.Message.MentionedUsers.Any(uz => uz.Id == Sender.CurrentUser.Id))
		{
			if (Regex.IsMatch(Args.Message.Content, @"\b(hi|hey|hello)\b", RegexOptions.IgnoreCase))
			{
				await Args.Message.RespondAsync(RandomString(CQuote.Hello));
			}
			else
			{
				await Args.Message.RespondAsync(RandomString(CQuote.Mention));
			}
		}
	}

	private async Task StartRandomMessageLoop(DiscordClient Sender)
	{
		// TODO: duplicates structure from CActivityService
		while ( IsBotAlive(Sender) )
		{
			// Wait first because otherwise bot will send message everytime it runs blowing out the chat
			await Task.Delay(TimeSpan.FromSeconds(RandomMessageInterval));
			await SendRandomMessage(Sender);
		}
	}

	private async Task SendRandomMessage(DiscordClient Sender)
	{
		DiscordChannel ChatChannel = await Sender.GetChannelAsync(CChannel.GeneralChat);
		
		// TODO: revisit CQuote.Mention here
		await ChatChannel.SendMessageAsync(RandomString(CQuote.Mention));
	}
}
