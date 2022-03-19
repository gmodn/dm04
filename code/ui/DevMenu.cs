using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

	public class DevMenu : Panel
	{
		public DevMenu( )
		{
			StyleSheet.Load( "/ui/DevMenu.scss" );

			Add.Label( "HL2:DM Made By", "header" );
			Add.Label( "V", "valve" );
			Add.Label( "Deathmatch: 2004 by", "header" );
			Add.Label( "Ian: Coder, UI Design, Mapper, Project Lead", "text" );
			Add.Label( "Loki: Coder, Animgraph Wizard, Model Porter", "text" );
			Add.Label( "BoxTrot: Coder, Model Porter, Mapper", "text" );
			Add.Label( "Rice: Mapper", "text" );
			Add.Label( "Meetle Meek: Mapper", "text" );
			Add.Label( "Super Smol: Programmer", "text" );

			Add.Label( "Playtested by", "header" );
			Add.Label( "Bugtogs", "text" );
			Add.Label( "Kirk", "text" );

			Add.Label( "Special thanks to", "header" );
			Add.Label( "Gunship Mark II and the HL2:MMod Team for Animations and Sound Effects", "text" );
			Add.Label( "Jun Choi for the Remade Weapon Models", "text" );
			Add.Label( "Shadow_RUN for the Viewmodel Rigging", "text" );
			Add.Label( "The Half-Life 2: Remade Assets Team for Models", "text" );
			Add.Label( "Vod Mesa for the Terry HEV suit", "text" );
			Add.Label( "You for supporting me for the past year", "text" );

			Add.Label( "No matter how small your", "header" );
			Add.Label( "contribution was, it was", "header" );
			Add.Label( "truly appreciated", "header" );
			
			Add.Label( "Thanks for making this", "header" );
			Add.Label( "Dream of mine come true", "header" );
			Add.Label( "-Ian", "header" );

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
		


		}
	}
