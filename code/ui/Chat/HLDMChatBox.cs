using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using System;


	public partial class HLDMChatBox : Panel
	{
		static HLDMChatBox Current;

		public Panel Canvas { get; protected set; }
		public TextEntry Input { get; protected set; }

		public HLDMChatBox()
		{
			Current = this;

			StyleSheet.Load( "/ui/chat/HLDMChatBox.scss" );

			Canvas = Add.Panel( "chat_canvas" );

			Input = Add.TextEntry( "" );
			Input.AddEventListener( "onsubmit", () => Submit() );
			Input.AddEventListener( "onblur", () => Close() );
			Input.AcceptsFocus = true;
			Input.AllowEmojiReplace = true;
		}

		void Open()
		{
			AddClass( "open" );
			Input.Focus();
		}

		void Close()
		{
			RemoveClass( "open" );
			Input.Blur();
		}

		public override void Tick()
		{
			base.Tick();

			if ( Sandbox.Input.Pressed( "chat" ) )
			{
				Open();
			}
		}

		void Submit()
		{
			Close();

			var msg = Input.Text.Trim();
			Input.Text = "";

			if ( string.IsNullOrWhiteSpace( msg ) )
				return;

			Say( msg );
		}

		public void AddEntry( string name, string message, string lobbyState = null )
		{
			var e = Canvas.AddChild<HLDMChatEntry>();

			e.Message.Text = message;
			e.NameLabel.Text = name;
		Sound.FromScreen( "sounds/ui/talk.sound" );

			e.SetClass( "noname", string.IsNullOrEmpty( name ) );

			if ( lobbyState == "ready" || lobbyState == "staging" )
			{
				e.SetClass( "is-lobby", true );
			}
		}


		[ConCmd.Client( "hldm_chat_add", CanBeCalledFromServer = true )]
		public static void AddChatEntry( string name, string message, string lobbyState = null )
		{
			Current?.AddEntry( name, message, lobbyState );

			// Only log clientside if we're not the listen server host
			if ( !Game.IsListenServer )
			{
				Log.Info( $"{name}: {message}" );
			}
		}

		[ConCmd.Client( "hldm_chat_addinfo", CanBeCalledFromServer = true )]
		public static void AddInformation( string message )
		{
			Current?.AddEntry( null, message );
		}

		[ConCmd.Server( "hldm_say" )]
		public static void Say( string message )
		{
			// todo - reject more stuff
			if ( message.Contains( '\n' ) || message.Contains( '\r' ) )
				return;

			Log.Info( $"{ConsoleSystem.Caller}: {message}" );
      
			AddChatEntry( To.Everyone, ConsoleSystem.Caller?.Name ?? "Server", message );
		}
	}
