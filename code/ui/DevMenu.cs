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
			} );

		//
		// Weapons
		//

		Add.Label( "Weapons", "section" );

		Add.Label( "Create USP", "button" ).AddEventListener( "onclick", () =>
		{
			ConsoleSystem.Run( "ent_create dm04_pistol" );
		} );

		Add.Label( "Create Python", "button" ).AddEventListener( "onclick", () =>
		{
			ConsoleSystem.Run( "ent_create dm04_python" );
		} );

		Add.Label( "Create GravGun", "button" ).AddEventListener( "onclick", () =>
		{
			ConsoleSystem.Run( "ent_create dm04_gravgun" );
		} );

		Add.Label( "Create Shotgun", "button" ).AddEventListener( "onclick", () =>
		{
			ConsoleSystem.Run( "ent_create dm04_shotgun" );
		} );

		Add.Label( "Create SMG", "button" ).AddEventListener( "onclick", () =>
		{
			ConsoleSystem.Run( "ent_create dm04_smg" );
		} );

		Add.Label( "Create Crossbow", "button" ).AddEventListener( "onclick", () =>
		{
			ConsoleSystem.Run( "ent_create dm04_crossbow" );
		} );
		}
	}
