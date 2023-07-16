using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using System;


	public partial class HLDMAlertBox : Panel
	{
		static HLDMAlertBox Current;

		public Panel Canvas { get; protected set; }

		public HLDMAlertBox()
		{
			Current = this;

			StyleSheet.Load( "/ui/alert/HLDMAlertBox.scss" );

			Canvas = Add.Panel( "chat_canvas" );
		}

		void Close()
		{
			RemoveClass( "open" );
		}

		public override void Tick()
		{
			base.Tick();
		}

		public void AddEntry( string message, string lobbyState = null )
		{
			var e = Canvas.AddChild<HLDMAlertEntry>();

			e.Message.Text = message;
			Sound.FromScreen( "sounds/ui/talk.sound" );

			if ( lobbyState == "ready" || lobbyState == "staging" )
			{
				e.SetClass( "is-lobby", true );
			}
		}


		[ConCmd.Client( "hldm_alert_add", CanBeCalledFromServer = true )]
		public static void AddAlertEntry( string message, string lobbyState = null )
		{
			Current?.AddEntry( message, lobbyState );

			// Only log clientside if we're not the listen server host
			if ( !Game.IsListenServer )
			{
				Log.Info( $"{message}" );
			}
		}

		[ConCmd.Client( "hldm_alert_addinfo", CanBeCalledFromServer = true )]
		public static void AddInformation( string message )
		{
			Current?.AddEntry( null, message );
		}

		[ConCmd.Server( "hldm_alert" )]
		public static void Say( string message )
		{
			// todo - reject more stuff
			if ( message.Contains( '\n' ) || message.Contains( '\r' ) )
				return;

			Log.Info( $"{message}" );
      
			AddAlertEntry( To.Everyone, message );
		}
	}
