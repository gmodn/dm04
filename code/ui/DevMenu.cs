using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

	public class DevMenu : Panel
	{
		public DevMenu( )
		{
			StyleSheet.Load( "/ui/DevMenu.scss" );

			Add.Label( "Deathmatch: 2004 Developer Menu", "header" );

			AddChild<DevMenuButtons>();
		}

		public override void Tick()
		{
			SetClass( "open", Input.Down( InputButton.Menu ) );
		}
	}

	public class DevMenuButtons : Panel
	{
		public DevMenuButtons()
		{
		//
		// General
		//
		Add.Label( "General", "section" );

		Add.Label( "Suicide", "button" ).AddEventListener( "onclick", () =>
		{
				ConsoleSystem.Run( "kill" );
				Sound.FromScreen( "buttonclick" );
		} );

		Add.Label( "Regenerate", "button" ).AddEventListener( "onclick", () =>
		{
			ConsoleSystem.Run( "dm04_impulse" );
			Sound.FromScreen( "buttonclick" );
		} );

		//
		// Weapons
		//

		Add.Label( "Weapons", "section" );

		Add.Label( "Create USP", "button" ).AddEventListener( "onclick", () =>
		{
			ConsoleSystem.Run( "dm04_giveweapon_pistol" );
			Sound.FromScreen( "buttonclick" );
		} );

		Add.Label( "Create Magnum", "button" ).AddEventListener( "onclick", () =>
		{
			ConsoleSystem.Run( "dm04_giveweapon_magnum" );
			Sound.FromScreen( "buttonclick" );
		} );

		Add.Label( "Create GravGun", "button" ).AddEventListener( "onclick", () =>
		{
			ConsoleSystem.Run( "edm04_giveweapon_gravgun" );
			Sound.FromScreen( "buttonclick" );
		} );

		Add.Label( "Create Shotgun", "button" ).AddEventListener( "onclick", () =>
		{
			ConsoleSystem.Run( "dm04_giveweapon_shotgun" );
			Sound.FromScreen( "buttonclick" );
		} );

		Add.Label( "Create SMG", "button" ).AddEventListener( "onclick", () =>
		{
			ConsoleSystem.Run( "dm04_giveweapon_smg" );
			Sound.FromScreen( "buttonclick" );
		} );
		
		Add.Label( "Create Pulse Rifle", "button" ).AddEventListener( "onclick", () =>
		{
			ConsoleSystem.Run( "dm04_giveweapon_pulsesmg" );
			Sound.FromScreen( "buttonclick" );
		} );

		Add.Label( "Create Crossbow", "button" ).AddEventListener( "onclick", () =>
		{
			ConsoleSystem.Run( "dm04_giveweapon_crossbow" );
			Sound.FromScreen( "buttonclick" );
		} );
		}
	}
