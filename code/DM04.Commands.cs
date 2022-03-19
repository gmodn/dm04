using Sandbox;
using Sandbox.UI.Construct;
using System;
using System.IO;
using System.Threading.Tasks;


public partial class DeathmatchGame
{
	[ServerCmd("dm04_impulse")]
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

			caller.SetAmmo( AmmoType.Pistol, 150 );
			caller.SetAmmo( AmmoType.Magnum, 12 );

			caller.SetAmmo( AmmoType.SMG, 225 );
			caller.SetAmmo( AmmoType.SMG_grenade, 3 );

			caller.SetAmmo( AmmoType.AR2, 60 );
			caller.SetAmmo( AmmoType.AR2_ball, 3 );

			caller.SetAmmo( AmmoType.Buckshot, 30 );
			caller.SetAmmo( AmmoType.Crossbow, 10 );

			caller.SetAmmo( AmmoType.RPG, 3 );
			caller.SetAmmo( AmmoType.Grenade, 5 );
		}
	}
}

