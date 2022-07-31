﻿
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using System;
using System.Threading.Tasks;

[Library]
public partial class DeathmatchHud : HudEntity<RootPanel>
{
	public DeathmatchHud()
	{
		if ( !IsClient )
			return;

		RootPanel.StyleSheet.Load( "/styles/hud.scss" );

		RootPanel.AddChild<Health>();
		//RootPanel.AddChild<Ammo>();

		RootPanel.AddChild<InventoryBar>();
		RootPanel.AddChild<PickupFeed>();
		
		RootPanel.AddChild<ChatBoxPlus>();
		RootPanel.AddChild<DamageIndicator>();
	//need to make our own now	///RootPanel.AddChild<KillFeed>();
		RootPanel.AddChild<Scoreboard>();
		RootPanel.AddChild<VoiceList>();
		RootPanel.AddChild<DevMenu>();
	}

	[ClientRpc]
	public void OnPlayerDied( string victim, string attacker = null )
	{
		Host.AssertClient();
	}

	[ClientRpc]
	public void ShowDeathScreen( string attackerName )
	{
		Host.AssertClient();
	}
}
