
using System;
using System.Collections.Generic;
using Sandbox;

[Library( "dm04_playerequip" ), HammerEntity]
[Title( "Player Start" ), Category( "Hammer" ), EditorSprite( "materials/dev/editor/dm04playerequip.vmat" ), Icon( "construction" )]

public partial class PlayerSetup : Entity
{

	//[Property( "EnableFlashlight" ), Description( "Is flashlight enabled or disabled on spawn" )]
	//public bool Enable_Flashlight { get; set; } = true;
	//Will do something. Someday.

	[Property("GiveUSP"), Description("Give the USP on player spawn")]
	public bool Should_Give_USP { get; set; } = false;

	[Property("GiveRevolver"), Description("Give the Revolver on player spawn")]
	public bool Should_Give_Revolver { get; set; } = false;

	[Property("GiveSMG"), Description("Give the SMG on player spawn")]
	public bool Should_Give_SMG { get; set; } = false;

	[Property("GiveShotgun"), Description("Give the Shotgun on player spawn")]
	public bool Should_Give_Shotgun { get; set; } = false;

	[Property("GiveAR2"), Description("Give the AR2 on player spawn")]
	public bool Should_Give_AR2 { get; set; } = false;

	[Property("GiveCrossbow"), Description("Give the Crossbow on player spawn")]
	public bool Should_Give_Bow { get; set; } = false;

	[Property("GiveGrenade"), Description("Give the Grenades on player spawn")]
	public bool Should_Give_Grenade { get; set; } = false;

	[Property("GiveRPG"), Description("Give the RPG on player spawn")]
	public bool Should_Give_RPG { get; set; } = false;

	[Property("GiveSlam"), Description("Give the SLAM on player spawn")]
	public bool Should_Give_Slam { get; set; } = false;

	[Property("GiveBugbaitWIP"), Description("Give the Bugbait on player spawn")]
	public bool Should_Give_Bugbait { get; set; } = false;

	[Property("GivePistolAmmo"), Description("How much Pistol ammo to give on player spawn (Limit is 150)")]
	[MinMax(0, 150)]
	public int Pistol_Ammo_To_Give { get; set; } = 0;

	[Property("GiveMagnumAmmo"), Description("How much Magnum ammo to give on player spawn (Limit is 12)")]
	[MinMax(0, 12)]
	public int Magnum_Ammo_To_Give { get; set; } = 0;

	[Property("GiveSMGAmmo"), Description("How much SMG ammo to give on player spawn (Limit is 225)")]
	[MinMax(0, 225)]
	public int SMG_Ammo_To_Give { get; set; } = 0;

	[Property("GiveSMGAltAmmo"), Description("How much SMG alt ammo to give on player spawn (Limit is 3)")]
	[MinMax(0, 3)]
	public int SMG_AltAmmo_To_Give { get; set; } = 0;

	[Property("GiveShotgunAmmo"), Description("How much Shotgun ammo to give on player spawn (Limit is 30)")]
	[MinMax(0, 30)]
	public int Shotgun_Ammo_To_Give { get; set; } = 0;

	[Property("GiveAR2Ammo"), Description("How much AR2 ammo to give on player spawn (Limit is 60)")]
	[MinMax(0, 60)]
	public int AR2_Ammo_To_Give { get; set; } = 0;

	[Property("GiveAR2AltAmmo"), Description("How much AR2 alt ammo to give on player spawn (Limit is 3)")]
	[MinMax(0, 3)]
	public int AR2_AltAmmo_To_Give { get; set; } = 0;

	[Property("GiveCrossbowBolts"), Description("How much Crossbow bolts to give on player spawn (Limit is 4)")]
	[MinMax(0, 4)]
	public int Crossbow_Ammo_To_Give { get; set; } = 0;

	[Property("GiveGrenades"), Description("How many Grenades to give on player spawn (Limit is 5)")]
	[MinMax(0, 5)]
	public int Grenades_To_Give { get; set; } = 0;

	[Property("GiveRPGRockets"), Description("How many RPG rockets to give on player spawn (Limit is 3)")]
	[MinMax(0, 3)]
	public int RPG_Ammo_To_Give { get; set; } = 0;

	[Property("GiveSlams"), Description("How many SLAMs to give on player spawn (Limit is 5)")]
	[MinMax(0, 5)]
	public int Slams_To_Give { get; set; } = 0;

	[Property("GiveBugbait"), Description("How many Bugbaits to give on player spawn (Limit is 5)")]
	[MinMax(0, 5)]
	public int Bugbaits_To_Give { get; set; } = 0;

	private List<HLDMWeapon> weaponsToGive;

	public override void Spawn()
	{
		base.Spawn();
		weaponsToGive = new List<HLDMWeapon>();

		if (Should_Give_USP)
			weaponsToGive.Add( TypeLibrary.Create<HLDMWeapon>("dm_pistol"));

		if (Should_Give_Revolver)
			weaponsToGive.Add( TypeLibrary.Create<HLDMWeapon>("dm_357"));

		if (Should_Give_SMG)
			weaponsToGive.Add( TypeLibrary.Create<HLDMWeapon>("dm_smg"));

		if (Should_Give_Shotgun)
			weaponsToGive.Add( TypeLibrary.Create<HLDMWeapon>("dm_shotgun"));

		if (Should_Give_AR2)
			weaponsToGive.Add( TypeLibrary.Create<HLDMWeapon>("dm_ar2"));

		if (Should_Give_Bow)
			weaponsToGive.Add( TypeLibrary.Create<HLDMWeapon>("dm_crossbow"));

		if (Should_Give_Grenade)
			weaponsToGive.Add( TypeLibrary.Create<HLDMWeapon>("dm_grenade"));

		//Add Below Strings once weapons are fully functioning

		//if (Should_Give_RPG)
		//	weaponsToGive.Add( TypeLibrary.Create<HLDMWeapon>("hl2_rpg"));

		//if (Should_Give_Slam)
		//	weaponsToGive.Add( TypeLibrary.Create<HLDMWeapon>("hl2_slam"));

		//Anything below here is wip weapons.

		//if (Should_Give_Bugbait)
		//	weaponsToGive.Add( TypeLibrary.Create<HLDMWeapon>("hl2_bugbait"));
	}

	[Event.Tick.Server]
	public void GivePlayersInventory()
	{
		foreach (var client in Game.Clients)
		{
			if (client.Pawn is DeathmatchPlayer player )
			{
				foreach (var weapon in weaponsToGive)
				{
					if (!player.Inventory.Contains(weapon))
						player.Inventory.Add(TypeLibrary.Create<HLDMWeapon>(weapon.GetType()));

					weapon.Delete();
				}

				player.SetAmmo(AmmoType.Pistol, Pistol_Ammo_To_Give);
				player.SetAmmo(AmmoType.Magnum, Magnum_Ammo_To_Give);
				player.SetAmmo(AmmoType.SMG, SMG_Ammo_To_Give);
				player.SetAmmo(AmmoType.SMG_grenade, SMG_AltAmmo_To_Give);
				player.SetAmmo(AmmoType.Buckshot, Shotgun_Ammo_To_Give);
				player.SetAmmo(AmmoType.AR2, AR2_Ammo_To_Give);
				player.SetAmmo(AmmoType.AR2_ball, AR2_AltAmmo_To_Give);
				player.SetAmmo(AmmoType.Crossbow, Crossbow_Ammo_To_Give);
				player.SetAmmo( AmmoType.Grenade, Grenades_To_Give );
				//player.SetAmmo(AmmoType.RPG, RPG_Ammo_To_Give);
				//player.SetAmmo(AmmoType.SLAM, Slams_To_Give);
				//player.SetAmmo(AmmoType.Bugbait, Bugbaits_To_Give);


				player.ActiveChild = player.Inventory.GetSlot(Game.Random.Int(0, player.Inventory.Count() - 1));
			}
		}
	}

}
