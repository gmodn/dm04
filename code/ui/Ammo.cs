﻿using Sandbox.UI;
using Sandbox.UI.Construct;

public class Ammo : Panel
{
	public Label ReserveAmmo;
	public Label CounterText;

	public Label AltCounterText;
	public Label AltLabel;

	List<Panel> BulletPanels = new List<Panel>();

	public Ammo()
	{
		CounterText = Add.Label( "0", "Counter" );
		ReserveAmmo = Add.Label( "100", "Reserve" );
		AltCounterText = Add.Label( "0", "Alt" );
		AltLabel = Add.Label( "Alt", "AltLabel" );
	}

	int weaponHash;

	public override void Tick()
	{
		var player = Game.LocalPawn as Player;
		if ( player == null ) return;

		var weapon = player.ActiveChild as HLDMWeapon;
		SetClass( "active", weapon != null );

		if ( weapon == null ) return;

		var reserve = weapon.AvailableAmmo();
		ReserveAmmo.Text = $"{reserve}";
		ReserveAmmo.SetClass( "active", reserve >= 0 );

		//Hide UI if the selected weapon does not have certian aspects (Alt ammo, No ammo, ETC)
		if ( weapon.AmmoType == AmmoType.None ) ReserveAmmo.Style.Display = DisplayMode.None;
		else ReserveAmmo.Style.Display = DisplayMode.Flex;
		if ( weapon.AmmoType == AmmoType.None ) CounterText.Style.Display = DisplayMode.None;
		else CounterText.Style.Display = DisplayMode.Flex;
		if ( weapon.SecondaryAmmo == AmmoType.None ) AltCounterText.Style.Display = DisplayMode.None;
		else AltCounterText.Style.Display = DisplayMode.Flex;
		if ( weapon.SecondaryAmmo == AmmoType.None ) AltLabel.Style.Display = DisplayMode.None;
		else AltLabel.Style.Display = DisplayMode.Flex;

		var hash = HashCode.Combine( player, weapon );

		CounterText.Text = $"{weapon.AmmoClip}";
		AltCounterText.Text = $"{weapon.SecondaryAmmoClip}";
	}
}
