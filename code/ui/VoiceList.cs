using Sandbox;
using Sandbox.UI;
using System;
using System.Linq;

public class VoiceList : Panel
{
	public static VoiceList Current { get; internal set; }

	public VoiceList()
	{
		Current = this;
		StyleSheet.Load( "Resource/Styles/VoiceList.scss" );
	}
		
	public void OnVoicePlayed( long steamId, float level )
	{
		var entry = ChildrenOfType<VoiceEntry>().FirstOrDefault( x => x.Friend.Id == steamId );
		if ( entry == null ) entry = new VoiceEntry( this, steamId );

		entry.Update( level );
	}
}
