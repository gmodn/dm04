
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using System;
using System.Threading.Tasks;

public partial class PickupFeed : Panel
{
	public static PickupFeed Current;
	//public Label AmmoIcon;
	//public Label AltIcon;

	public PickupFeed()
	{
		Current = this;
	}

	/// <summary>
	/// An RPC which can be called from the server 
	/// </summary>
	[ClientRpc]
	public static void OnPickup( string text )
	{
		// TODO - icons for weapons?
		// TOPO - icons for ammo?

		Current?.AddEntry( $"\n{text}" );		
	}

	/// <summary>
	/// Spawns a label, waits for half a second and then deletes it
	/// The :outro style in the scss keeps it alive and fades it out
	/// </summary>
	private async Task AddEntry( string text )
	{
		var panel = Current.Add.Label( text );
		//var panel = Current.Add.Label( AmmoIcon );
		await Task.Delay( 500 );
		panel.Delete();
	}

	//public override void Tick()
	//{
		//base.Tick();
		//var player = Local.Pawn as Player;
		//if ( player == null ) return;

		//var weapon = player.ActiveChild as BaseDmWeapon;
		//SetClass( "active", weapon != null );

		//AmmoIcon.Text = $"{weapon.AmmoIcon}";
		//AltIcon.Text = $"{weapon.AltIcon}";
	//}
}
