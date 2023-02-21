using Sandbox.UI;

class InventoryIcon : Panel
{
	public HLDMWeapon Weapon;
	public Panel Icon;

	public InventoryIcon( HLDMWeapon weapon )
	{
		Weapon = weapon;
		Icon = Add.Panel( "icon" );

		AddClass( weapon.ClassName );
	}

	internal void TickSelection( HLDMWeapon selectedWeapon )
	{
		SetClass( "active", selectedWeapon == Weapon );
		SetClass( "empty", !Weapon?.IsUsable() ?? true );
	}

	public override void Tick()
	{
		base.Tick();

		if ( !Weapon.IsValid() || Weapon.Owner != Game.LocalPawn )
			Delete( true );
	}
}
