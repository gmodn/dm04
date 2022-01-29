
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

public class Ammo : Panel
{
	private Panel AmmoPanel;
	private Label CounterText;
	private Label ReserveText;

	public Ammo()
	{
		AmmoPanel = Add.Panel( "Ammo" );
		AmmoPanel.Add.Label( "AMMO", "AmmoText" );
		CounterText = AmmoPanel.Add.Label( "0", "Counter" );
		ReserveText = AmmoPanel.Add.Label( "0", "Reserve" );
	}

	public override void Tick()
	{
		var player = Local.Pawn;
		if ( player == null ) return;

		var weapon = player.ActiveChild as BaseDmWeapon;
		SetClass( "active", weapon != null );

		if ( weapon == null ) return;

		CounterText.Text = $"{weapon.AmmoClip}";

		var inv = weapon.AvailableAmmo();
		ReserveText.Text = $"{inv}";
		ReserveText.SetClass( "active", inv >= 0 );
	}
}
