using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

public class Ammo : Panel
{
	private Panel AmmoPanel;
	private Label CounterText;
	private Label ReserveText;
	public Label AmmoIcon;

	private Panel AltPanel;
	private Label AltText;
	public Label AltIcon;

	public Ammo()
	{
		AmmoPanel = Add.Panel( "Ammo" );
		AmmoPanel.Add.Label( "AMMO", "AmmoText" );
		AmmoIcon = AmmoPanel.Add.Label( "p", "icon" );
		CounterText = AmmoPanel.Add.Label( "0", "Counter" );
		ReserveText = AmmoPanel.Add.Label( "0", "Reserve" );

		AltPanel = Add.Panel( "Alt" );
		AltPanel.Add.Label( "ALT", "AmmoText" );
		AltText = AltPanel.Add.Label( "0", "Counter" );
		AltIcon = AltPanel.Add.Label( "p", "icon" );
	}

	public override void Tick()
	{
		var player = Local.Pawn as Player;
		if ( player == null ) return;

		var weapon = player.ActiveChild as HLDMWeapon;
		SetClass( "active", weapon != null );

		if ( weapon == null ) return;

		//temp hide ui if ammotype = none (I'm sure there's a better way to do this)
		if ( weapon.AmmoType == AmmoType.None ) AmmoPanel.Style.Display = DisplayMode.None;
		else AmmoPanel.Style.Display = DisplayMode.Flex;
		if ( weapon.SecondaryAmmo == AmmoType.None ) AltPanel.Style.Display = DisplayMode.None;
		else AltPanel.Style.Display = DisplayMode.Flex;

		CounterText.Text = $"{weapon.AmmoClip}";
		AltText.Text = $"{weapon.SecondaryAmmoClip}";
		AmmoIcon.Text = $"{weapon.AmmoIcon}";
		AltIcon.Text = $"{weapon.AltIcon}";

		var inv = weapon.AvailableAmmo();
		ReserveText.Text = $"{inv}";
		ReserveText.SetClass( "active", inv >= 0 );
	}
}
