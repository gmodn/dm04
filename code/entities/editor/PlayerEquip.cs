
using System;
using System.Collections.Generic;
using Hammer;
using Sandbox;

[Library( "dm04_playerequip" )]
[Hammer.EntityTool( "Player Equip", "DM:04", "Gives The Player Weapons On Spawn" )]
[Hammer.EditorSprite( "materials/dev/editor/dm04playerequip.vmat" )]

public partial class PlayerSetup : Entity
{

	[Property( "EnableFlashlight" ), Description( "Is flashlight enabled or disabled on spawn" )]
	public bool Enable_Flashlight { get; set; } = true;

	[Property( "GiveCrowbar" ), Description( "Give the Crowbar on player spawn" )]
	public bool Should_Give_Crowbar { get; set; } = false;

	[Property( "GiveUSP" ), Description( "Give the USP on player spawn" )]
	public bool Should_Give_USP { get; set; } = false;

	[Property( "GiveRevolver" ), Description( "Give the Revolver on player spawn" )]
	public bool Should_Give_Revolver { get; set; } = false;

	[Property( "GiveSMG" ), Description( "Give the SMG on player spawn" )]
	public bool Should_Give_SMG { get; set; } = false;

	[Property( "GivePistolAmmo" ), Description( "How much Pistol9 ammo to give on player spawn (Limit is 150)" )]
	[MinMax( 0, 150 )]
	public int Pistol_Ammo_To_Give { get; set; } = 0;

	[Property( "GiveMagnumAmmo" ), Description( "How much Magnum ammo to give on player spawn (Limit is 12)" )]
	[MinMax( 0, 12 )]
	public int Magnum_Ammo_To_Give { get; set; } = 0;

	[Property( "GiveSMGAmmo" ), Description( "How much SMG ammo to give on player spawn (Limit is 225)" )]
	[MinMax( 0, 225 )]
	public int SMG_Ammo_To_Give { get; set; } = 0;

	private List<BaseDmWeapon> weaponsToGive;

	public override void Spawn()
	{
		base.Spawn();
		weaponsToGive = new List<BaseDmWeapon>();

		if ( Should_Give_Crowbar )
			weaponsToGive.Add( Library.Create<BaseDmWeapon>( "hl2_crowbar" ) );

		if ( Should_Give_USP )
			weaponsToGive.Add( Library.Create<BaseDmWeapon>( "hl2_uspmatch" ) );

		if ( Should_Give_Revolver )
			weaponsToGive.Add( Library.Create<BaseDmWeapon>( "hl2_357" ) );

		if ( Should_Give_SMG )
			weaponsToGive.Add( Library.Create<BaseDmWeapon>( "hl2_smg1" ) );
	}

	[Event.Tick.Server]
	public void GivePlayersInventory()
	{
		foreach ( var client in Client.All )
		{
			if (client.Pawn is DeathmatchPlayer player && player.DidRespawn )
			{
				foreach ( var weapon in weaponsToGive )
				{
					if( !player.Inventory.Contains(weapon) )
						player.Inventory.Add( Library.Create<BaseDmWeapon>(weapon.GetType()) );

					weapon.Delete();
				}

				player.SetAmmo( AmmoType.Pistol, Pistol_Ammo_To_Give );
				player.SetAmmo( AmmoType.Magnum, Magnum_Ammo_To_Give );
				player.SetAmmo( AmmoType.SMG, SMG_Ammo_To_Give );

				player.ActiveChild = player.Inventory.GetSlot( Rand.Int( 0, player.Inventory.Count() - 1 ) );
				player.DidRespawn = false;
			}
		}
	}

}

