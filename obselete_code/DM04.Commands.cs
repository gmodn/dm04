using Sandbox;
using Sandbox.UI.Construct;
using System;
using System.IO;
using System.Threading.Tasks;


public partial class DeathmatchGame
{
	[ConCmd.Admin( "")]
	public static void Impulse(int impulse)
	{
		var caller = ConsoleSystem.Caller.Pawn as DeathmatchPlayer;

		if ( caller == null )
			return;

		if(impulse == 101)
		{
			caller.Inventory.Add( new hl2_crowbar() );
			caller.Inventory.Add( new hl2_stunstick() );
			caller.Inventory.Add( new hl2_gravgun() );
			caller.Inventory.Add( new hl2_uspmatch() );
			caller.Inventory.Add( new hl2_357() );
			caller.Inventory.Add( new hl2_smg1() );
			caller.Inventory.Add( new hl2_ar2() );
			caller.Inventory.Add( new hl2_spas12() );
			caller.Inventory.Add( new hl2_crossbow() );
			caller.Inventory.Add( new hl2_rpg() );
			caller.Inventory.Add( new hl2_grenade() );
			caller.Inventory.Add( new hl2_slam() );
			caller.Inventory.Add( new hl2_gauss() );
			caller.Inventory.Add( new hl2_egon() );
			caller.Inventory.Add( new hl2_bugbait() );

			caller.SetAmmo( AmmoType.Pistol, 150 );
			caller.SetAmmo( AmmoType.Magnum, 12 );

			caller.SetAmmo( AmmoType.SMG, 225 );
			caller.SetAmmo( AmmoType.SMG_grenade, 3 );

			caller.SetAmmo( AmmoType.AR2, 60 );
			caller.SetAmmo( AmmoType.AR2_ball, 3 );

			caller.SetAmmo( AmmoType.Buckshot, 30 );
			caller.SetAmmo( AmmoType.Crossbow, 4 );
			caller.SetAmmo( AmmoType.Crossbow, 4 );
			caller.SetAmmo( AmmoType.Egon, 300 );

			caller.SetAmmo( AmmoType.RPG, 3 );
			caller.SetAmmo( AmmoType.Grenade, 5 );
			caller.SetAmmo( AmmoType.SLAM, 5 );

			caller.SetAmmo( AmmoType.Bugbait, 95 );
		}
	}
	[ConCmd.Client( "Coomer" )]
	public static void coomer()
	{
		var caller = ConsoleSystem.Caller.Pawn as Player;

		if ( caller == null ) return;

		caller.SetModel( "models/playermodels/coomer/drcoomer.vmdl" );
	}
	[ConCmd.Client( "terry" )]
	public static void terry()
	{
		var caller = ConsoleSystem.Caller.Pawn as Player;

		if ( caller == null ) return;

		caller.SetModel( "models/player/hevsuit_white.vmdl" );
	}
}

