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
			ConsoleSystem.Run( "dm04_impulse 101" );
			Sound.FromScreen( "buttonclick" );
		} );
		
		}
	}
