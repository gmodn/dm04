
using Sandbox;
using Sandbox.Hooks;
using Sandbox.UI;
using Sandbox.UI.Construct;
using System;

namespace Sandbox.UI
{
	public partial class ChatBoxPlus : Panel
	{
		static ChatBoxPlus Current;

		public Panel Canvas { get; protected set; }
		public TextEntry Input { get; protected set; }

		public ChatBoxPlus()
		{
			Current = this;

			StyleSheet.Load( "ui/ChatBoxPlus.scss" );

			Canvas = Add.Panel( "chat_canvas" );

			Input = Add.TextEntry( "" );
			Input.AddEventListener( "onsubmit", () => Submit() );
			Input.AddEventListener( "onblur", () => Close() );
			Input.AcceptsFocus = true;
			Input.AllowEmojiReplace = true;

			Sandbox.Hooks.Chat.OnOpenChat += Open;
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

		void Submit()
		{
			Close();

			var msg = Input.Text.Trim();
			Input.Text = "";

			if ( string.IsNullOrWhiteSpace( msg ) )
				return;
			
			Say(msg);

			/*if ( Global.Lobby != null )
			{
				Log.Info( "Send Chat" );
				Global.Lobby?.SendChat( msg );
			}
			else
			{
				Say( msg );
			}*/
		}

		public void AddEntry( string name, string message, string avatar, string lobbyState = null )
		{
			var e = Canvas.AddChild<ChatEntryPlus>();
			
			e.Message.Text = message;
			e.NameLabel.Text = name;
			e.Avatar.SetTexture( avatar );

			e.SetClass( "noname", string.IsNullOrEmpty( name ) );
			e.SetClass( "noavatar", string.IsNullOrEmpty( avatar ) );

			if ( lobbyState == "ready" || lobbyState == "staging" )
			{
				e.SetClass( "is-lobby", true );
			}
		}


		[ClientCmd( "chat_add", CanBeCalledFromServer = true )]
		public static void AddChatEntryPlus( string name, string message, string avatar = null, string lobbyState = null )
		{
			Current?.AddEntry( name, message, avatar, lobbyState );

			// Only log clientside if we're not the listen server host
			if ( !Global.IsListenServer )
			{
				Log.Info( $"{name}: {message}" ); 
			}
		}

		[ClientCmd( "chat_addinfo", CanBeCalledFromServer = true )]
		public static void AddInformation( string message, string avatar = null )
		{
			Current?.AddEntry( null, message, avatar );
		}

		[ServerCmd( "say" )]
		public static void Say( string message )
		{
			Assert.NotNull( ConsoleSystem.Caller );

			// todo - reject more stuff
			if ( message.Contains( '\n' ) || message.Contains( '\r' ) )
				return;

			Log.Info( $"{ConsoleSystem.Caller}: {message}" );
			AddChatEntryPlus( To.Everyone, ConsoleSystem.Caller.Name, message, $"avatar:{ConsoleSystem.Caller.PlayerId}" );
		}

		[Event( "lobby.chat" )]
		public static void LobbyChat( Friend friend, string message )
		{
			if ( !Host.IsServer ) return;

			Log.Info( $"Lobby Chat: {message}" );
			AddChatEntryPlus( To.Everyone, friend.Name, message, $"avatar:{friend.Id}", Global.Lobby?.GetMemberData( friend, "status" ) );
		}

	}
}

namespace Sandbox.Hooks
{
	public static partial class Chat
	{
		public static event Action OnOpenChat;

		[ClientCmd( "openchat" )]
		internal static void MessageMode()
		{
			OnOpenChat?.Invoke();
		}

	}
}
