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
			caller.Inventory.Add( new GravGun(), true );
			caller.Inventory.Add( new Crowbar(), true );
			caller.Inventory.Add( new StunBaton(), true );
			caller.Inventory.Add( new Pistol(), true );
			caller.Inventory.Add( new Magnum(), true );
			caller.Inventory.Add( new Shotgun(), true );
			caller.Inventory.Add( new Crossbow(), true );
			caller.Inventory.Add( new SMG(), true );
			caller.Inventory.Add( new PulseSMG(), true );
			caller.Inventory.Add( new Grenade(), true );
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
		[ServerCmd( "dm04_giveweapon_pistol" )]
		public static void GivePistol()
		{
			var caller = ConsoleSystem.Caller.Pawn;
			if ( caller == null ) return;
			caller.Inventory.Add( new Pistol(), true );
		}

		[ServerCmd( "dm04_giveweapon_magnum" )]
		public static void GiveMagnum()
		{
			var caller = ConsoleSystem.Caller.Pawn;
			if ( caller == null ) return;
			caller.Inventory.Add( new Magnum(), true );
		}

		[ServerCmd( "dm04_giveweapon_shotgun" )]
		public static void GiveShotgun()
		{
			var caller = ConsoleSystem.Caller.Pawn;
			if ( caller == null ) return;
			caller.Inventory.Add( new Shotgun(), true );
		}

		[ServerCmd( "dm04_giveweapon_crossbow" )]
		public static void GiveCrossbow()
		{
			var caller = ConsoleSystem.Caller.Pawn;
			if ( caller == null ) return;
			caller.Inventory.Add( new Crossbow(), true );
		}

		[ServerCmd( "dm04_giveweapon_smg" )]
		public static void GiveSMG()
		{
			var caller = ConsoleSystem.Caller.Pawn;
			if ( caller == null ) return;
			caller.Inventory.Add( new SMG(), true );
		}

		[ServerCmd( "dm04_giveweapon_pulsesmg" )]
		public static void GivePulseSMG()
		{
			var caller = ConsoleSystem.Caller.Pawn;
			if ( caller == null ) return;
			caller.Inventory.Add( new PulseSMG(), true );
		}

		[ServerCmd( "dm04_giveweapon_grenade" )]
		public static void GiveGrenade()
		{
			var caller = ConsoleSystem.Caller.Pawn;
			if ( caller == null ) return;
			caller.Inventory.Add( new Grenade(), true );
		}

		[ServerCmd( "dm04_giveweapon_gravgun" )]
		public static void GiveGravgun()
		{
			var caller = ConsoleSystem.Caller.Pawn;
			if ( caller == null ) return;
			caller.Inventory.Add( new GravGun(), true );
		}
	}
}
