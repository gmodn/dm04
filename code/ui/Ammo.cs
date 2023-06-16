using Sandbox.UI;
using Sandbox.UI.Construct;

public class Ammo : Panel
{
	public Label Inventory;
	public Label CounterText;
	public Label AltCounterText;

	List<Panel> BulletPanels = new List<Panel>();

	public Ammo()
	{
		CounterText = Add.Label( "0", "Counter" );
		Inventory = Add.Label( "100", "inventory" );
	}

	int weaponHash;

	public override void Tick()
	{
		var player = Game.LocalPawn as Player;
		if ( player == null ) return;

		var weapon = player.ActiveChild as DeathmatchWeapon;
		SetClass( "active", weapon != null );

		if ( weapon == null ) return;

		var inv = weapon.AvailableAmmo();
		Inventory.Text = $"{inv}";
		Inventory.SetClass( "active", inv >= 0 );

		var hash = HashCode.Combine( player, weapon );

		CounterText.Text = $"{weapon.AmmoClip}";
		//AltCounterText.Text = $"{weapon.SecondaryAmmoClip}";
	}
}
