using Sandbox;
using Sandbox.UI.Construct;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DM04
{
	public partial class DM04Commands
	{
		//General Commands
		[ServerCmd( "dm04_impulse" )]
		public static void RegenStuff()
		{
			var caller = ConsoleSystem.Caller.Pawn;
			if ( caller == null ) return;
			caller.Inventory.Add( new OLD_gravgun(), true );
			caller.Inventory.Add( new hl2_crowbar(), true );
			//caller.Inventory.Add( new hl2_stunbaton(), true );
			caller.Inventory.Add( new hl2_uspmatch(), true );
			caller.Inventory.Add( new hl2_357(), true );
			caller.Inventory.Add( new hl2_smg1(), true );
			caller.Inventory.Add( new hl2_ar2(), true );
			caller.Inventory.Add( new hl2_spas12(), true );
			caller.Inventory.Add( new hl2_crossbow(), true );
			caller.Inventory.Add( new hl2_grenade(), true );
			caller.Inventory.Add( new hl2_rpg(), true );
			caller.Inventory.Add( new hl2_slam(), true );
			caller.Health += 100;
		}

		[ServerCmd( "dm04_healme" )]
		public static void HealMe()
		{
			var caller = ConsoleSystem.Caller.Pawn;
			if ( caller == null ) return;
			caller.Health += 100;
		}

		//Weapon specific commands
		[ServerCmd("dm04_giveweapon_crowbar")]
		public static void GiveCrowbar()
		{
			var caller = ConsoleSystem.Caller.Pawn;
			if (caller == null) return;
			caller.Inventory.Add(new hl2_crowbar(), true);
		}

		[ServerCmd( "dm04_giveweapon_pistol" )]
		public static void GivePistol()
		{
			var caller = ConsoleSystem.Caller.Pawn;
			if ( caller == null ) return;
			caller.Inventory.Add( new hl2_uspmatch(), true );
		}

		[ServerCmd( "dm04_giveweapon_magnum" )]
		public static void GiveMagnum()
		{
			var caller = ConsoleSystem.Caller.Pawn;
			if ( caller == null ) return;
			caller.Inventory.Add( new hl2_357(), true );
		}

		[ServerCmd( "dm04_giveweapon_smg" )]
		public static void GiveSMG()
		{
			var caller = ConsoleSystem.Caller.Pawn;
			if ( caller == null ) return;
			caller.Inventory.Add( new hl2_smg1(), true );
		}

		[ServerCmd( "dm04_giveweapon_pulsesmg" )]
		public static void GivePulseSMG()
		{
			var caller = ConsoleSystem.Caller.Pawn;
			if ( caller == null ) return;
			caller.Inventory.Add( new hl2_ar2(), true );
		}

		[ServerCmd("dm04_giveweapon_shotgun")]
		public static void GiveShotgun()
		{
			var caller = ConsoleSystem.Caller.Pawn;
			if (caller == null) return;
			caller.Inventory.Add(new hl2_spas12(), true);
		}

		[ServerCmd("dm04_giveweapon_crossbow")]
		public static void GiveCrossbow()
		{
			var caller = ConsoleSystem.Caller.Pawn;
			if (caller == null) return;
			caller.Inventory.Add(new hl2_crossbow(), true);
		}

		[ServerCmd( "dm04_giveweapon_grenade" )]
		public static void GiveGrenade()
		{
			var caller = ConsoleSystem.Caller.Pawn;
			if ( caller == null ) return;
			caller.Inventory.Add( new hl2_grenade(), true );
		}

		[ServerCmd("dm04_giveweapon_rpg")]
		public static void GiveRPG()
		{
			var caller = ConsoleSystem.Caller.Pawn;
			if (caller == null) return;
			caller.Inventory.Add(new hl2_rpg(), true);
		}

		[ServerCmd("dm04_giveweapon_slam")]
		public static void GiveSlam()
		{
			var caller = ConsoleSystem.Caller.Pawn;
			if (caller == null) return;
			caller.Inventory.Add(new hl2_slam(), true);
		}

		[ServerCmd( "dm04_devweapons" )]
		public static void GiveDev()
		{
			var caller = ConsoleSystem.Caller.Pawn;
			if ( caller == null ) return;
			caller.Inventory.Add( new devgun(), true );
		}
	}
}
